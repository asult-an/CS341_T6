using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Android.Preferences;
using CookNook.Model;
using CookNook.Model.Interfaces;
using Npgsql;

namespace CookNook.Services
{
    class PreferenceDatabase : IPreferenceDatabase
    {
        private string connString;

        private List<DietPreference> myPreferences;

        /// <summary>
        /// The settings specific to the user signed in
        /// </summary>
        public List<DietPreference> MyPreferences
        {
            get { return myPreferences; }
            set { myPreferences = value; }
        }



        public PreferenceDatabase() {
            this.connString = DbConn.ConnectionString;
        }

        public Task<List<DietPreference>> GetPreferencesForUserAsync(long userId)
        {
            List<DietPreference> outPreferences = new List<DietPreference>();
            using var conn = new NpgsqlConnection();
            conn.Open();

            // first, we'll need to get the properties stored in the DietPreference container-like model
            var cmdBasePref = new NpgsqlCommand(@"SELECT pref_id, is_preferred, title
                                                 FROM user_settings WHERE user_id == @UserId;", conn);
            cmdBasePref.Parameters.AddWithValue("UserId", userId);
            
            // TODO: see if a transaction is better suited for this

            // get the results so we can move on to the affected entities
            using (var reader = cmdBasePref.ExecuteReader())
            {
                Debug.WriteLine($"[PreferenceDatabase] Reading preferences for {userId}...");
                while (reader.Read())
                {
                    /* we'll only have the title and pref_id here, the collections are empty.
                     * Thus, we'll only store its id and worry about the rest in a future query op */
                    // preferenceIds.Add(reader.GetInt32(0));
                    DietPreference userPreference = new DietPreference(
                        reader.GetInt32(0),   
                        reader.GetString(2), 
                        null, null);
                }
            }
            // at this point, all of this user's named preferenceIds are in a List
            
            // now for each preference, we have to resolve the affected entities
            // foreach (var preferenceId in preferenceIds)
            foreach (var preference in outPreferences)
            {
                // GetAffectedRecipes
                preference.AffectedRecipes.AddRange(GetAffectedRecipes(userId, preference.DietPrefId));

                // GetAffectedIngredients
                preference.AffectedIngredients.AddRange(GetAffectedIngredients(userId, preference.DietPrefId));
            }
            
            // TODO: proper async
            return Task.FromResult(outPreferences);

        }

        public bool UpdatePreferencesFor(long userId, List<DietPreference> preferences)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Begin a transaction
            using var transaction = conn.BeginTransaction();
            try
            {
                // Delete existing preferences for the user
                var cmdDeleteExisting = new NpgsqlCommand(@"DELETE FROM preference_recipes WHERE pref_id IN 
                                                    (SELECT pref_id FROM user_settings WHERE user_id = @UserId);
                                                    DELETE FROM preference_ingredients WHERE pref_id IN 
                                                    (SELECT pref_id FROM user_settings WHERE user_id = @UserId);", conn);
                cmdDeleteExisting.Parameters.AddWithValue("UserId", userId);
                cmdDeleteExisting.ExecuteNonQuery();

                // all new preferences need to get re-added
                foreach (var preference in preferences)
                {
                    // insert into user_settings
                    var cmdInsertPref = new NpgsqlCommand(@"INSERT INTO user_settings (user_id, pref_id, is_preferred, title)
                                                    VALUES (@UserId, @PrefId, @IsPreferred, @Title);", conn);
                    cmdInsertPref.Parameters.AddWithValue("UserId", userId);
                    cmdInsertPref.Parameters.AddWithValue("PrefId", preference.DietPrefId);
                    cmdInsertPref.Parameters.AddWithValue("Title", preference.Title);

                    // since each preference affects 0-N recipes AND ingredients, we still have to issue 2 more queries
                    cmdInsertPref.ExecuteNonQuery();

                    // handling preference's affected ingredients
                    foreach (DietAffectedRecipe recipe in preference.AffectedRecipes)
                    {
                        var cmdInsertRecipes = new NpgsqlCommand(@"INSERT INTO preference_recipes
                                                                    (pref_id, recipe_id)
                                                                    VALUES(@PrefId, @UserId);");
                        cmdInsertRecipes.Parameters.AddWithValue("PrefId", preference.DietPrefId);
                        
                        // this is a bit redundant, but that's a future sprint problem
                        cmdInsertRecipes.Parameters.AddWithValue("UserId", userId);
                        cmdInsertRecipes.ExecuteNonQuery();
                    }

                    // handling preference's affected recipes
                    foreach (DietAffectedRecipe pref in preference.AffectedRecipes)
                    {
                        var cmdInsertRecipes = new NpgsqlCommand(@"INSERT INTO preference_recipes
                                                                    (pref_id, recipe_id)
                                                                    VALUES(@PrefId, @UserId);");
                        cmdInsertRecipes.Parameters.AddWithValue("PrefId", preference.DietPrefId);

                        // this is a bit redundant, but that's a future sprint problem
                        cmdInsertRecipes.Parameters.AddWithValue("UserId", userId);
                        cmdInsertRecipes.ExecuteNonQuery();
                    }
                }
                Debug.WriteLine("[PreferenceDatabase] Updated preferences on user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                Debug.WriteLine($"[PreferenceDatabase] Error updating user preference: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Builds a list of preference Ids that are owned by this user, then using those ids,
        /// queries the affected ingredients table for any matching preference ids.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="prequery">Pre-defined preference's id: passing this parameter alters the flow so 
        /// rather than returning ALL preference ids, it only looks for ingredients affected by just that 
        /// preference id. Otherwise we default it to null.</param>
        /// <returns></returns>
        public List<DietAffectedIngredient> GetAffectedIngredients(long userId, long? prefId = null)
        {
            List<long> preferenceIds = new List<long>();
            List<DietAffectedIngredient> outIngredients = new List<DietAffectedIngredient>();
            
            using var conn = new NpgsqlConnection();

            // this command's result is used to identify the range of pref_id needed from preferences_ingredients,
            // so it's also responsible for populating preferenceIds
            var cmdUserIdentifier = new NpgsqlCommand(@"SELECT user_id, pref_id, is_preferred, title
                                                      FROM user_settings WHERE user_id == @UserId", conn);
            
            // this command's result will be used to populate outIngredients
            var cmdIngredients = new NpgsqlCommand(@"SELECT pref_id, ingredient_id
                                                   FROM preference_ingredients WHERE pref_id == @PrefId", conn);
            conn.Open();

            // If a preference was NOT used, then we want to return ALL affected ingredients.
            if (prefId != null)
            {
                // if the ids were already populated, skip that and use the existing pref_id
                preferenceIds.Add(prefId.Value);
            } 
            else
            {
                // first, we'll need to get the properties stored in the DietPreference container-like model
                cmdUserIdentifier.Parameters.AddWithValue("UserId", userId);
                var reader = cmdUserIdentifier.ExecuteReader();

                // add the resolved preferenceIds to the collection
                while (reader.Read())
                {
                    reader.Read();
                    long preferenceId = reader.GetInt32(2);
                    preferenceIds.Add(preferenceId);
                }
            }

            // for all preference Ids in preferenceIds:
            foreach (long preferenceId in preferenceIds)
            {
                // need the id in the query
                cmdIngredients.Parameters.AddWithValue("PrefId", preferenceId);

                using (var reader = cmdIngredients.ExecuteReader())
                {
                    // for all ingredients affected by this preference...
                    while (reader.Read())
                    {
                        // make sure we're at the -1th index before we start reading
                        // reader.Read();

                        // unpack the tuple-like result into its model 
                        DietAffectedIngredient result = new DietAffectedIngredient(
                            reader.GetInt32(0), reader.GetBoolean(1)
                            );
                        // add the object to the result list
                        outIngredients.Add(result);

                    }
                }
            }
            Debug.WriteLine($"[PreferenceDatabase] Fetched preferences on user {userId}");
            return outIngredients;
        }


        /// <summary>
        /// Builds a list of preference Ids that are owned by this user, then using those ids,
        /// queries the affected recipe table for any matching preference ids.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="prequery">Pre-defined preference: passing this parameter alters the flow so 
        /// rather than returning ALL preference ids, it only looks for recipe affected by just that 
        /// preference id. Otherwise we default it to null.</param>
        /// <returns></returns>
        public List<DietAffectedRecipe> GetAffectedRecipes(long userId, long? prefId = null)
        {
            List<long> preferenceIds = new List<long>();
            List<DietAffectedRecipe> outRecipes = new List<DietAffectedRecipe>();

            using var conn = new NpgsqlConnection();

            // this command's result is used to identify the range of pref_id needed from preferences_Recipes,
            // so it's also responsible for populating preferenceIds
            var cmdUserIdentifier = new NpgsqlCommand(@"SELECT user_id, pref_id, is_preferred, title
                                                      FROM user_settings WHERE user_id == @UserId", conn);

            // this command's result will be used to populate outRecipes
            var cmdRecipes = new NpgsqlCommand(@"SELECT pref_id, Recipe_id
                                                   FROM preference_recipes WHERE pref_id == @PrefId", conn);
            conn.Open();

            // if we can avoid executing cmdUserIdentifier, then by all means!
            if (prefId != null)
            {
                preferenceIds.Add(prefId.Value);
            }
            // If a preference was NOT used, then we want to return ALL affected Recipes.
            else
            {
                // first, we'll need to get the properties stored in the DietPreference container-like model
                cmdUserIdentifier.Parameters.AddWithValue("UserId", userId);
                var reader = cmdUserIdentifier.ExecuteReader();

                // add the resolved preferenceIds to the collection
                while (reader.Read())
                {
                    reader.Read();
                    long preferenceId = reader.GetInt32(2);
                    preferenceIds.Add(preferenceId);
                }
            }

            // for all preference Ids in preferenceIds:
            foreach (long preferenceId in preferenceIds)
            {
                // need the id in the query
                cmdRecipes.Parameters.AddWithValue("PrefId", preferenceId);

                using (var reader = cmdRecipes.ExecuteReader())
                {
                    // for all Recipes affected by this preference...
                    while (reader.Read())
                    {
                        // make sure we're at the -1th index before we start reading
                        reader.Read();

                        // unpack the tuple-like result into its model 
                        DietAffectedRecipe result = new DietAffectedRecipe(
                            reader.GetInt32(0), reader.GetBoolean(1)
                            );
                        // add the object to the result list
                        outRecipes.Add(result);

                    }
                }
            }
            return outRecipes;
        }

        /// <summary>
        /// Identifies the owner of the preference requesting to be deleted and compares it to the supplied
        /// userId before proceeding with the deletion.  If the user_ids don't match, the deletion is aborted.
        /// Otherwise, all entries in `preference_recipes` and `preference_ingredients` are deleted on `prefId`
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="prefId"></param>
        /// <returns></returns>
        public bool DeleteUserPreference(long userId, long prefId)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try {
                // check that the user owns this preference
                var cmdVerify = new NpgsqlCommand(@"SELECT COUNT(*) FROM user_settings 
                                            WHERE user_id = @UserId AND pref_id = @PrefId", conn);
                cmdVerify.Parameters.AddWithValue("UserId", userId);
                cmdVerify.Parameters.AddWithValue("PrefId", prefId);

                var count = (long)cmdVerify.ExecuteScalar();
                // have to check the results a bit differently since we're within a transaction
                if (count == 0)
                {
                    // preference does not belong to the user, abort the operation
                    return false;
                }

                // first we'll handle preference_recipes
                var cmdDeleteRecipes = new NpgsqlCommand(@"DELETE FROM preference_recipes 
                                                  WHERE pref_id = @PrefId", conn);
                cmdDeleteRecipes.Parameters.AddWithValue("PrefId", prefId);
                cmdDeleteRecipes.ExecuteNonQuery();

                // now we can take care of preference_ingredients
                var cmdDeleteIngredients = new NpgsqlCommand(@"DELETE FROM preference_ingredients 
                                                       WHERE pref_id = @PrefId", conn);
                cmdDeleteIngredients.Parameters.AddWithValue("PrefId", prefId);
                cmdDeleteIngredients.ExecuteNonQuery();

                // commit the transaction
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                Debug.WriteLine($"[PreferenceDatabase] Error deleting user preference: {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// Seldom called: deletes all of a user's preferences in `preference_recipes` and `preference_ingredients`
        /// /// The userId is only needed since the pref_id can be resolved from user_settings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteAllPreferencesFor(long userId)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Begin a transaction
            using var transaction = conn.BeginTransaction();
            try
            {
                // prep commands to delete from preference_recipes and preference_ingredients on their UserId
                var cmdDeleteAll = new NpgsqlCommand(@"DELETE FROM preference_recipes WHERE pref_id IN 
                                                        (SELECT pref_id FROM user_settings WHERE user_id = @UserId);
                                                    DELETE FROM preference_ingredients WHERE pref_id IN 
                                                        (SELECT pref_id FROM user_settings WHERE user_id = @UserId);", conn);
                cmdDeleteAll.Parameters.AddWithValue("UserId", userId);
                cmdDeleteAll.ExecuteNonQuery();

                // Commit the transaction
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                Debug.WriteLine($"Error deleting all preferences for user {userId}: {ex.Message}");
                return false;
            }
        }
    }
}
