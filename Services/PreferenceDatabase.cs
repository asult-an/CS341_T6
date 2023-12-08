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

            var cmdBasePref = new NpgsqlCommand(@"SELECT pref_id, is_preferred, title
                                                 FROM user_settings WHERE user_id == @userId;", conn);
            cmdBasePref.Parameters.AddWithValue("userId", userId);
            // first, we'll need to get the properties stored in the DietPreference container-like model

            // we need to use a transaction since we'll separately query the affected ingredients and recipes
            using var transaction = conn.BeginTransaction();
           

            // GetAffectedRecipes

            // GetAffectedIngredients

            // 

        }

        public bool UpdatePreferencesFor(long userId, List<DietPreference> preferences)
        {
            return TODO_IMPLEMENT_ME;
        }

        /// <summary>
        /// Builds a list of preference Ids that are owned by this user, then using those ids,
        /// queries the affected ingredients table for any matching preference ids.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="prequery">Pre-defined preference: passing this parameter alters the flow so 
        /// rather than returning ALL preference ids, it only looks for ingredients affected by just that 
        /// preference id. Otherwise we default it to null.</param>
        /// <returns></returns>
        public List<DietAffectedIngredient> GetAffectedIngredients(long userId, DietPreference preference = null)
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
            if (preference == null)
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
                        reader.Read();

                        // unpack the tuple-like result into its model 
                        DietAffectedIngredient result = new DietAffectedIngredient(
                            reader.GetInt32(0), reader.GetBoolean(1)
                            );
                        // add the object to the result list
                        outIngredients.Add(result);

                    }
                }
            }
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
        public List<DietAffectedRecipe> GetAffectedRecipes(long userId, DietPreference preference = null)
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

            // If a preference was NOT used, then we want to return ALL affected Recipes.
            if (preference == null)
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

        public bool DeleteAllPreferencesFor(long userId)
        {

        }
    }
}
