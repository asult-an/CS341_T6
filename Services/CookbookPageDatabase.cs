using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using CookNook.Model.Interfaces;
using Npgsql;

namespace CookNook.Services
{
    /// <summary>
    /// Representative of a cookbook page.  
    /// </summary>
    internal class CookbookPageDatabase : ICookbookPageDatabase
    {
        // TODO: do we want to use a property to store the list of pages so they can change without re-querying?
        // TODO: mechanism for updating cached pages?
        private List<CookbookPageModel> cookbookPages;

        /// <summary>
        /// Constructor that instructs the database to load all cookbook pages for a particular user
        /// </summary>
        /// <param name="userId"></param>
        public CookbookPageDatabase(long userId)
        {
            cookbookPages = GetCookbookPagesForUser(userId);
        }

        public CookbookPageAdditionError AddRecipeToCookbookPage(long recipeID, long cookbookPageID)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            // add a new row to the cookbook_contents table with both ids as their column
            var cmd = new NpgsqlCommand("INSERT INTO cookbook_contents (recipe_id, list_id) VALUES @RecipeID, @ListID", conn);
            cmd.Parameters.AddWithValue("@RecipeID", recipeID);
            cmd.Parameters.AddWithValue("@ListID", cookbookPageID);
            try
            {
                var reader = cmd.ExecuteReader();

                // check if we affected a row
                if (reader.RecordsAffected == 1)
                {
                    reader.Close();
                    return CookbookPageAdditionError.NoError;
                }
                else
                {
                    reader.Close();
                    return CookbookPageAdditionError.Unpsecified;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return CookbookPageAdditionError.Unpsecified;
            }
        }

        public CookbookPageDeletionError RemoveRecipeFromCookbookPage(long recipeID, long cookbookPageID)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("DELETE FROM cookbook_contents WHERE recipe_id = @RecipeID AND list_id = @ListID", conn);   
            cmd.Parameters.AddWithValue("@RecipeID", recipeID);
            cmd.Parameters.AddWithValue("@ListID", cookbookPageID);

            var reader = cmd.ExecuteReader();

            if (reader.RecordsAffected == 1)
            {
                reader.Close();
                return CookbookPageDeletionError.NoError;
            }
            else
            {
                reader.Close();
                Debug.Write($"Failed to delete recipe {recipeID} from page {cookbookPageID}");
                return CookbookPageDeletionError.Unspecified;
            }

        }


        [Obsolete]        
        // TODO: this might belong better in RecipeLogic...?
        /// <summary>
        ///  returns all recipies associated with this cookbookpage's ID
        /// </summary>
        /// <param name="cookbookPageID"></param>
        /// <returns>a List of recipeIds that can be handled by RecipeDatabase</returns>
        public List<long> GetRecipesOnCookbookPage(long cookbookPageID)
        {
            //   List<Recipe> results = new List<Recipe>();
            List<long> recipeIds = new List<long>();
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            // using the recipe_id stored, select all recipes that have their id present
            var cmd = new NpgsqlCommand("SELECT recipe_id FROM cookbook_contents WHERE list_id = @ListID", conn);

            cmd.Parameters.AddWithValue("@ListID", cookbookPageID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIds.Add(reader.GetInt64(0));
            }
            return recipeIds;
        }

        public List<long> GetRecipeIdsForCookbookPage(long cookbookPageID)
        {

            //   List<Recipe> results = new List<Recipe>();
            List<long> recipeIds = new List<long>();
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            // using the recipe_id stored, select all recipes that have their id present
            var cmd = new NpgsqlCommand("SELECT recipe_id FROM cookbook_contents WHERE list_id = @ListID", conn);

            cmd.Parameters.AddWithValue("@ListID", cookbookPageID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIds.Add(reader.GetInt64(0));
            }
            return recipeIds;
        }

        /// <summary>
        /// returns all CookbookPages belonging to a particualr user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CookbookPageModel> GetCookbookPagesForUser(long userID)
        {
            

            // TODO:
            throw new NotImplementedException();
        }

        public CookbookPageModel GetCookbookPageByID(long cookbookPageID)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            // using the recipe_id stored, select all recipes that have their id present
            var cmd = new NpgsqlCommand("SELECT list_id, user_id, page_title FROM user_cookbook_pages WHERE list_id = @ListID", conn);

            cmd.Parameters.AddWithValue("@ListID", cookbookPageID);
            // might need to add the user's id as part of the query here...

            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Debug.Write($"No cookbook page found with id {cookbookPageID}");
                return null;
            }

            // unpack results
            long listId = reader.GetInt64(0);
            long userId = reader.GetInt64(1);
            string pageName = reader.GetString(2);

            // structure into the model:
            var model = new CookbookPageModel(pageName, userId, listId);
            return model;
        }
    }
}
