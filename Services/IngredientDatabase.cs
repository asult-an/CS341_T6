using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.Services;

namespace CookNook.Services
{
    internal class IngredientDatabase : IIngredientDatabase
    {
        //TODO: implement RecipeLogic/RecipeDb interfaces

        //TODO: TagLogic/TagDB + interfaces


        /// <summary>
        /// Gets all ingredients from the database and returns them as a list
        /// </summary>
        /// <returns></returns>
        public List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT ingredient_id, name FROM ingredients;", conn);
                
                //"SELECT public.tags.* FROM public.recipe_tags, public.tags WHERE tags.tag_id = recipe_tags.tag_id", conn);

                //@"SELECT recipe_ingredients.ingredient_id, ingredients.name, recipe_ingredients.quantity, recipe_ingredients
                //FROM public.recipe_ingredients recipe_ingredients, public.ingredients ingredients, public.recipes recipes", conn);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Int64 ingredientId = reader.GetInt64(0);
                string name = reader.GetString(1);
                //string qty = reader.GetString(2);
                //string unit = reader.GetString(3);
                Ingredient ingredient = new Ingredient(ingredientId, name);


                //// if the cell was NULL in the database:
                //if (string.IsNullOrEmpty(unit))
                //{
                //    // use the 'unitless ingredient' constructor
                //    ingredient = new Ingredient(ingredientId, name, qty);
                //}
                //else
                //{
                //    ingredient = new Ingredient(ingredientId, name, qty, unit);
                //}
                ingredients.Add(ingredient);
            }

            reader.Close();
            return ingredients;
        }

        /// <summary>
        /// Selects an ingredient from the Ingredients table by its name and returns it
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Nullable ingredient</returns>
        public Ingredient GetIngredientByName(string name)
        {
            Ingredient? queriedIngredient = null;
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            // we can leverage the ANY() operator to query for a range of ids
            var cmd = new NpgsqlCommand("SELECT * FROM ingredients WHERE name = @Name", conn);
            cmd.Parameters.AddWithValue("Name", name);
            using var reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows == false)
                // already set to null, so just return early
                return queriedIngredient;

            Int64 ingredientId = reader.GetInt64(0);
            string ingredientName = reader.GetString(1);
            queriedIngredient = new Ingredient(ingredientId, ingredientName, null);
            
            reader.Close();
            return queriedIngredient;
        }


        /// <summary>
        /// Retrieves a list of ingredients from the database
        /// NOTE: all quantity fields will be null since they require a recipe
        /// </summary>
        /// <param name="ingredientIds"></param>
        /// <returns></returns>
        public List<Ingredient> GetIngredientRange(long[] ingredientIds)
        {
            List<Ingredient> results = new List<Ingredient>();

            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            // we can leverage the ANY() operator to query for a range of ids
            var cmd = new NpgsqlCommand("SELECT * FROM ingredients WHERE ingredient_id = ANY(@IngredientIds)", conn);
            cmd.Parameters.AddWithValue("IngredientIds", ingredientIds);
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Int64 ingredientId = reader.GetInt64(0);
                string name = reader.GetString(1);
                // quantity is only seen when specifying how many are in a recipe, but it's nullable
                string qty = reader.GetString(2);
                //string unit = reader.GetString(3);
                Ingredient ingredient;
                
                // if the cell was NULL in the database:
                if (string.IsNullOrEmpty(qty))
                {
                    // use the 'unitless ingredient' constructor
                    ingredient = new Ingredient(name, null);
                }
                else
                {
                    ingredient = new Ingredient(ingredientId, name, qty);
                }

                results.Add(ingredient);
            }

            reader.Close();
            return results;
        }


        // TODO: this might make more sense to be defined on a RecipeList class
        /// <summary>
        /// Returns an ingredient by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public Ingredient GetIngredientByName(string name)
        //{
        //}

        /// <summary>
        /// Retrieves the ingredients for a given recipe from the database
        /// </summary>
        /// <param name="recipeID">ID of the recipe to query</param>
        /// <returns>a list of Ingredients found in the recipe</returns>
        public List<Ingredient> GetIngredientsFromRecipe(Int64 recipeId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(
                @"SELECT recipe_ingredients.ingredient_id, recipe_ingredients.quantity,recipe_ingredients, ingredients.name
                FROM public.recipe_ingredients recipe_ingredients, public.ingredients ingredients, public.recipes recipes
                WHERE
                    recipe_ingredients.ingredient_id = ingredients.ingredient_id
                    AND recipe_ingredients.recipe_id = @Recipe_ID", conn);
            cmd.Parameters.AddWithValue("Recipe_ID", recipeId);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Int64 ingredientId = reader.GetInt64(0);
                string name = reader.GetString(1);
                string qty = reader.GetString(2);
                string unit = reader.GetString(3);
                Ingredient ingredient;

                // if the cell was NULL in the database:
                if (string.IsNullOrEmpty(unit))
                {
                    // use the 'unitless ingredient' constructor
                    ingredient = new Ingredient(ingredientId, name, qty);
                }
                else
                {
                    ingredient = new Ingredient(ingredientId, name, qty, unit);
                }
                ingredients.Add(ingredient);
            }
            reader.Close();
            return ingredients;
        }

        /// <summary>
        /// Adds an ingredient-recipe relation to the recipe_ingredients table
        /// </summary>
        /// <remarks>NOTE: the ingredient must already exist for the operation to succeed</remarks>
        /// <param name="recipeId">recipe's id</param>
        /// <param name="ingredientId">ingredient's id</param>
        /// <returns></returns>
        public IngredientAdditionError AddIngredientToRecipe(long recipeId, long ingredientId)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"INSERT INTO recipe_ingredients (recipe_id, ingredient_id) VALUES (@Recipe_ID, @Ingredient_ID)", conn);
            cmd.Parameters.AddWithValue("Recipe_ID", recipeId);
            cmd.Parameters.AddWithValue("Ingredient_ID", ingredientId);
            var reader = cmd.ExecuteReader();
            reader.Close();

            // return the appropriate error if the insert failed
            if (reader.RecordsAffected == 0)
            {
                return IngredientAdditionError.DBAdditionError;
            }
            return IngredientAdditionError.NoError;
        }

        /// <summary>
        /// returns a singular ingredient by its ID.
        /// NOTE: without a recipe_Id, the unit and quantity will be null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ingredient GetIngredientById(long id)
        {
            Ingredient ingredient = null;
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(
                               @"SELECT * FROM ingredients WHERE ingredient_id = @Ingredient_ID", conn);
            cmd.Parameters.AddWithValue("Ingredient_ID", id);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Int64 ingredientId = reader.GetInt64(0);
                string name = reader.GetString(1);
                //string qty = reader.GetString(2);
                //string unit = reader.GetString(3);

                // if the cell was NULL in the database:
                ingredient = new Ingredient(name, null);
            }

            reader.Close();
            return ingredient;
        }

        /// <summary>
        /// Updates an ingredient in the database
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IngredientUpdateError UpdateIngredient(long ingredientId, string name)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            //var cmd = new NpgsqlCommand(@"UPDATE ingredients SET name = @Name, quantity = @Quantity, unit = @Unit WHERE ingredient_id = @Ingredient_ID", conn);
            var cmd = new NpgsqlCommand(@"UPDATE ingredients SET name = @Name WHERE ingredient_id = @Ingredient_ID", conn);
            cmd.Parameters.AddWithValue("Ingredient_ID", ingredientId);
            cmd.Parameters.AddWithValue("Name", name);
            //cmd.Parameters.AddWithValue("Quantity", quantity);
            //cmd.Parameters.AddWithValue("Unit", unit);

            // if no rows were affected, the ingredient id probably doesn't exist
            try
            {
                // if no rows were returned...
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return IngredientUpdateError.IngredientNotFound;
                }
                return IngredientUpdateError.NoError;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine($"Unspecified error updating ingredient: {e}");
                return IngredientUpdateError.DBUpdateError;
            }
        }


        /// <summary>
        /// Adds a new ingredient name into the ingredients table to be used when 
        /// creating new recipes.
        /// </summary>
        /// <param name="name">name of the ingredient</param>
        /// <returns></returns>
        public IngredientAdditionError CreateIngredient(string name)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();
            var cmd = new NpgsqlCommand(@"INSERT INTO ingredients (name) VALUES (@Name)", conn);
            
            // attempt to insert into the database, throwing an error on failed insertions
            try
            {
                cmd.Parameters.AddWithValue("Name", name);
                cmd.ExecuteNonQuery();
                return IngredientAdditionError.NoError;
            }
            catch (Exception e)
            {
                //return new IngredientAdditionError(e.Message);
                Console.WriteLine($"Error occurred adding an ingredient! {e}");
                Debug.WriteLine(e.Message);
                return IngredientAdditionError.DBAdditionError;
            }
        }

        /// <summary>
        /// Queries the database for an ingredient by name, returning the existing entry if found.
        /// If there was no match, then the ingredient will be created.
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        public dynamic GetOrCreateIngredient(Ingredient ingredient)
        {
            Ingredient outIngredient;

            // query the database to see if it exists
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();


            // to handle existing ingredients, we can leverage RETURNING stmnt to return the existing
            // ingredient_id, should a conflict occur
            var cmd = new NpgsqlCommand(@"INSERT INTO ingredients (name) VALUES (@Name) ON CONFLICT (name) 
                                        DO UPDATE SET name = @Name RETURNING name, ingredient_id", conn);

            cmd.Parameters.AddWithValue("Name", ingredient.Name);
            using var reader = cmd.ExecuteReader();
            reader.Read();

            // our results is simple, only having the id and name: 
            outIngredient = new Ingredient(reader.GetString(0), reader.GetString(1));
            /*
             * SELECT recipe_ingredients.ingredient_id, ingredients.name, recipe_ingredients.quantity, recipe_ingredients.unit
               FROM public.recipe_ingredients recipe_ingredients, public.ingredients ingredients, public.recipes recipes
               WHERE
               recipe_ingredients.ingredient_id = ingredients.ingredient_id
               AND recipe_ingredients.recipe_id = recipes.recipe_id
             */
            reader.Close();

            return outIngredient;
        }

        public IngredientDeleteError RemoveIngredient(long ingredientId)
        {
            using var conn = new NpgsqlConnection(DbConn.ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"DELETE FROM ingredients WHERE ingredient_id = @Ingredient_ID", conn);
            cmd.Parameters.AddWithValue("Ingredient_ID", ingredientId);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            int rowsAffected = reader.RecordsAffected;
            reader.Close();

            if (rowsAffected == 0)
            {
                return IngredientDeleteError.DBDeletionError;
            }
            else
            {
                return IngredientDeleteError.NoError;
            }
        }
    }
}
