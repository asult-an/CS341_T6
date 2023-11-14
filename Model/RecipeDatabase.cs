
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace CookNook.Model
{
    internal class RecipeDatabase : IRecipeDatabase
    {
        private List<int> authorListIDs = new List<int>();
        private List<int> cookbookIDs = new List<int>();
        private List<Recipe> authorList;
        private List<Recipe> cookbook;
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list
        public List<Recipe> AuthorList { get { return authorList; } set { authorList = value; } }
        public List<Recipe> Cookbook { get { return cookbook; } set { cookbook = value; } }

        public RecipeDeletionError DeleteFromAuthorList(int recipeID)
        {
            authorListIDs.Remove(recipeID);
            DeleteRecipe(recipeID);
            authorList = SelectRecipes(authorListIDs);
            return RecipeDeletionError.NoError;

        }

        public RecipeDeletionError DeleteFromCookbook(int recipeID)
        {
            cookbookIDs.Remove(recipeID);
            cookbook = SelectRecipes(cookbookIDs);
            return RecipeDeletionError.NoError;

        }

        public RecipeAdditionError AddToAuthorList(int recipeID)
        {
            authorListIDs.Add(recipeID);
            authorList = SelectRecipes(authorListIDs);
            return RecipeAdditionError.NoError;

        }

        public RecipeAdditionError AddToCookbook(int recipeID)
        {
            cookbookIDs.Add(recipeID);
            cookbook = SelectRecipes(cookbookIDs);

            return RecipeAdditionError.NoError;

        }

        public List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand(
                "SELECT public.tags.* FROM public.recipe_tags, public.tags WHERE tags.tag_id = recipe_tags.tag_id", conn);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int ingredientId = reader.GetInt32(0);
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
        /// Retrieves the ingredients for a given recipe from the database
        /// </summary>
        /// <param name="recipeID">ID of the recipe to query</param>
        /// <returns>a list of Ingredients found in the recipe</returns>
        public List<Ingredient> GetIngredientsByRecipe(int recipeID)
        {
            
            List<Ingredient> ingredients = new List<Ingredient>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand(
                @"SELECT recipe_ingredients.ingredient_id, recipe_ingredients.quantity,recipe_ingredients, ingredients.name
                FROM public.recipe_ingredients recipe_ingredients, public.ingredients ingredients, public.recipes recipes
                WHERE
                    recipe_ingredients.ingredient_id = ingredients.ingredient_id
                    AND recipe_ingredients.recipe_id = @Recipe_ID", conn);
            cmd.Parameters.AddWithValue("Recipe_ID", recipeID);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int ingredientId = reader.GetInt32(0);
                string name = reader.GetString(1);
                string qty = reader.GetString(2);
                string unit = reader.GetString(3);
                Ingredient ingredient;

                // if the cell was NULL in the database:
                if(string.IsNullOrEmpty(unit))
                {
                    // use the 'unitless ingredient' constructor
                    ingredient = new Ingredient(ingredientId, name, qty);
                } else
                {
                    ingredient = new Ingredient(ingredientId, name, qty, unit);
                }
                ingredients.Add(ingredient);
            }
            reader.Close();
            return ingredients;
        }



        /// <summary>
        /// Queries the associative relationship between a recipe and the users that 
        /// follow it, which is found in the recipe_followers table. 
        /// 
        /// Because this is within the Recipe's domain, we don't go any further than
        /// collecting their Ids and returning them in a list.  
        /// The User domain can handle resolving the full User from the individual Ids
        /// </summary>
        /// <param name="recipeID">the id of the recipe to check for</param>
        /// <returns>List of user Ids that are following the given recipe</returns>
        public List<int> GetRecipeFollowerIds(int recipeID)
        {
            List<int> userIds = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand(
         @"SELECT recipe_followers.user_id
                FROM public.users users, public.recipe_followers recipe_followers
                WHERE 
	                recipe_followers.recipe_id = @Recipe_ID AND 
	                users.user_id = recipe_followers.user_id ", conn);
            cmd.Parameters.AddWithValue("Recipe_ID", recipeID);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userIds.Add(reader.GetInt32(1));
            }
            // not sure how we can incorporate the DB errors into the return type since we're returning a list.
            // We COULD make a new type of Error that wraps an operation, in practice it would be similar to how 
            // Task<T> wrap other methods...
            return userIds;
        }


        /// <summary>
        /// Edits a recipe in the database. 
        /// Note that associated entities (Tags and Ingredientes) are dropped and re-added, 
        /// so take care not to 
        /// </summary>
        /// <param name="inRecipe"></param>
        /// <returns></returns>
        public RecipeEditError EditRecipe(Recipe inRecipe)
        {
            try
            {
                //connect to and open the database
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var transaction = conn.BeginTransaction(); 


                // first we need to handle the base recipe attributes
                using var cmdBaseRecipeAttributes = new NpgsqlCommand(@"UPDATE public.recipes
                        SET name=@Name, description=@Description, cook_time_mins=@CookTimeMins, course=@Course, 
                            rating=@Rating, servings=@Servings, image=@Image, author_id=@AuthorID
                        WHERE recipe_id = @RecipeID", conn);

                cmdBaseRecipeAttributes.Parameters.AddWithValue("RecipeID", inRecipe.ID);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Name", inRecipe.Name);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Description", inRecipe.Description);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("AuthorID", inRecipe.AuthorID);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("IngredientsList", inRecipe.Ingredients);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("IngredientsQty", inRecipe.IngredientsQtyToString());
                cmdBaseRecipeAttributes.Parameters.AddWithValue("CookTimeMins", inRecipe.CookTime);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Course", inRecipe.Course);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Rating", inRecipe.Rating);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Servings", inRecipe.Servings);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Image", inRecipe.Image);
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Tags", inRecipe.TagsToString());
                cmdBaseRecipeAttributes.Parameters.AddWithValue("Followers", inRecipe.FollowersToString());
                
               
                //execute the command
                var numAffected = cmdBaseRecipeAttributes.ExecuteNonQuery();

                // next we take care of ingredients
                using var cmdDeleteIngredients = new NpgsqlCommand("DELETE FROM recipe_ingredients WHERE recipe_id = @RecipeID", conn);
                cmdDeleteIngredients.Parameters.AddWithValue("RecipeID", inRecipe.ID);
                cmdDeleteIngredients.ExecuteNonQuery();

                foreach (Ingredient ing in inRecipe.Ingredients)
                {
                    // insert row in the ingredients table
                    // TODO: handle if null unit
                    var cmdInsertIngredients = new NpgsqlCommand(
                        @"INSERT INTO recipe_ingredients (ingredient_id, quantity, unit) 
                                 VALUES (@IngredientID, @Quantity, @Unit)", conn);
                    cmdInsertIngredients.Parameters.AddWithValue("IngredientID", ing.IngredientId);
                    cmdInsertIngredients.Parameters.AddWithValue("Quantity", ing.Quantity);
                    cmdInsertIngredients.Parameters.AddWithValue("Unit", ing.Unit);

                    // not querying the database, just inserting to it, so we execute a NonQuery:
                    cmdInsertIngredients.ExecuteNonQuery();
                }

                // now we delete tags for this recipe
                using var cmdDeleteTags = new NpgsqlCommand("DELETE FROM recipe_tags WHERE recipe_id = @RecipeID", conn);
                cmdDeleteTags.Parameters.AddWithValue("RecipeID", inRecipe.ID);
                cmdDeleteTags.ExecuteNonQuery();

                // re-add the newly updated tags back in
                foreach (var tag in inRecipe.Tags)
                {
                    var cmdInsertTag = new NpgsqlCommand("INSERT INTO recipe_tags (recipe_id, tag_id) VALUES (@RecipeID, @TagID)", conn);
                    cmdInsertTag.Parameters.AddWithValue("RecipeID", inRecipe.ID);
                    cmdInsertTag.Parameters.AddWithValue("TagID", tag.Id);
                    cmdInsertTag.ExecuteNonQuery();
                }

                //update the observable collection
                authorList = SelectRecipes(authorListIDs);
                cookbook = SelectRecipes(cookbookIDs);
            }
            catch (Npgsql.PostgresException pe)//catch any exceptions thrown by the database access statements
            {
                Console.WriteLine("Update failed, {0}", pe);
                //report result
                return RecipeEditError.DBEditError;
            }
            return RecipeEditError.NoError;
        }
        

        public RecipeAdditionError InsertRecipe(Recipe inRecipe)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();


            // "save" your game! (preserves the state of database until we commit)
            using var transaction = conn.BeginTransaction();


            //write the SQL statement to insert the recipe into the database
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO recipes (name, description, cook_time_mins, " +
                                "course, rating, servings, image, author_id) VALUES " +
                                "(@Name, @Description, @CookTimeMins, @Course, @Rating, @Servings, " +
                                "@Image, @AuthorID) RETURNING recipe_id", conn);

            //extract the relevant data from the recipe specified by the user
            cmd.Parameters.AddWithValue("Name", inRecipe.Name);
            cmd.Parameters.AddWithValue("Description", inRecipe.Description);
            cmd.Parameters.AddWithValue("CookTimeMins", inRecipe.CookTime);
            cmd.Parameters.AddWithValue("Course", inRecipe.Course);
            cmd.Parameters.AddWithValue("Rating", inRecipe.Rating);
            cmd.Parameters.AddWithValue("Servings", inRecipe.Servings);
            cmd.Parameters.AddWithValue("Image", inRecipe.Image);
            cmd.Parameters.AddWithValue("AuthorID", inRecipe.AuthorID);

           
            // first, we need the new ID of that recipe!
            int recipeID = (int)cmd.ExecuteScalar();

            foreach (Ingredient ing in inRecipe.Ingredients)
            {
                // insert row in the ingredients table
                // TODO: handle if null unit
                cmd = new NpgsqlCommand("INSERT INTO ingredients (name, quantity, unit) VALUES (@Name, @Quantity, @Unit)", conn);
                cmd.Parameters.AddWithValue("Name", ing.Name);
                cmd.Parameters.AddWithValue("Quantity", ing.Quantity);
                cmd.Parameters.AddWithValue("Unit", ing.Unit);

                // not querying the database, just inserting to it, so we execute a NonQuery:
                cmd.ExecuteNonQuery();
            }

            // WARNING: Doesn't account for a tag being added that wasn't already in the database!
            // now we can handle any tags involved
            foreach(Tag tag in inRecipe.Tags)
            {
                cmd = new NpgsqlCommand("INSERT INTO recipe_tags (recipe_id, tag_id) VALUES (@RecipeID, @TagID)", conn);
                
                // here we get to use that ID we grabbed earlier
                cmd.Parameters.AddWithValue("RecipeID", recipeID);
                cmd.Parameters.AddWithValue("TagID", tag.Id);
                cmd.ExecuteNonQuery();
            }
            // add the user's ID to the recipe
            //AddToAuthorList(inRecipe.ID);
            
            transaction.Commit();
            return RecipeAdditionError.NoError;
        }

        /// <summary>
        /// Returns ALL recipes from the database
        /// </summary>
        /// <returns></returns>
        public List<Recipe> SelectAllRecipes()
        {
            List<Recipe> outRecipees = new List<Recipe>();

            List<Recipe> outRecipes = new List<Recipe>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            Recipe recipe = new Recipe();
            cmd.CommandText = "SELECT * FROM recipes;";
            reader = cmd.ExecuteReader();
            while (reader.Read()) {
                reader.Read();
                recipe.ID = reader.GetInt32(0);
                recipe.Name = reader.GetString(1);
                recipe.Description = reader.GetString(2);
                recipe.AuthorID = reader.GetInt32(3);
                //recipe.Ingredients = reader.GetString(4);

                recipe.IngredientsQty = reader.GetString(5);
                recipe.CookTime = reader.GetInt32(6);
                // CourseType is needed, so we have to use a helper function to convert it
                recipe.Course = CourseType.Parse(reader.GetString(7));
                recipe.Rating = reader.GetInt32(8);
                recipe.Servings = reader.GetInt32(9);
                recipe.Image = Encoding.ASCII.GetBytes(reader.GetString(10));
                recipe.Tags = GetTagsForRecipe(recipe.ID).ToArray();
                
                // reader.Close();
                outRecipes.Add(recipe);
            }
            return outRecipes;
        }
        


        public List<Recipe> SelectRecipes(List<int> recipeList)
        {
            List<Recipe> outRecipes = new List<Recipe>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            foreach (int recipeID in recipeList)
            {
                // TODO: fully qualify the asterisk 
                var cmd = new NpgsqlCommand(
                    @"SELECT recipe_id, name, description, cook_time_mins, course, 
                            rating, servings, image, author_id FROM public.recipes 
                            WHERE 
                                recipe_id = @Recipe_ID;", conn);

                cmd.Parameters.AddWithValue("Recipe_ID", recipeID);
                
                using (var reader = cmd.ExecuteReader())
                {
                    Debug.Write(recipeID);
                    while (reader.Read())
                    {
                        var ingredients = GetIngredientsByRecipe(recipeID).ToArray();

                        Recipe recipe = new Recipe
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            CookTime = reader.GetInt32(3),
                            Course = CourseType.Parse(reader.GetString(4)),
                            Rating = reader.GetInt32(5),
                            Servings = reader.GetInt32(6),
                            Image = Encoding.ASCII.GetBytes(reader.GetString(7)),
                            AuthorID = reader.GetInt32(8)
                        };

                        outRecipes.Add(recipe);
                    }

                }
            }
            return outRecipes;

        }

        /// <summary>
        /// Selects a recipe by its recipeId
        /// </summary>
        /// <param name="inID"></param>
        /// <returns></returns>
        public Recipe SelectRecipe(int inID)
        {
            Recipe recipe = new Recipe();

            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand(@"SELECT r.recipe_id, name, description, cook_time_mins, course, rating, image servings, author_id
                                FROM recipes AS r
                                JOIN recipe_tags rt ON r.recipe_id = rt.recipe_id
                                WHERE r.recipe_id = @RecipeId AND rt.recipe_id = @RecipeId;", conn);



            cmd.Parameters.AddWithValue("RecipeId", inID);
            using var reader = cmd.ExecuteReader();
            reader.Read();

            recipe.ID = reader.GetInt32(0);
            recipe.Name = reader.GetString(1);
            recipe.Description = reader.GetString(2);
            recipe.AuthorID = reader.GetInt32(3);
            recipe.Ingredients = GetIngredientsByRecipe(inID).ToArray();
            recipe.CookTime = reader.GetInt32(6);
            recipe.Course = CourseType.Parse(reader.GetString(7));
            recipe.Rating = reader.GetInt32(8);
            recipe.Servings = reader.GetInt32(9);
            recipe.Image = Encoding.ASCII.GetBytes(reader.GetString(10));
                
            // get associative data
            recipe.Tags = GetTagsForRecipe(inID).ToArray();
            recipe.FollowerIds = GetRecipeFollowerIds(inID).ToArray();

            return recipe;

        }

        /// <summary>
        /// Overload to accept raw CourseType objects, ripping out their name to use as a parameter for the original method
        /// to help facilitate easier transactions
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public List<Recipe> SelectRecipeByCourse(CourseType course) { return SelectRecipeByCourse(course.Name); }

        public List<Recipe> SelectRecipeByCourse(string course)
        {
            List<int> recipeIDs = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM recipes WHERE course = @Course";
            cmd.Parameters.AddWithValue("Course", course);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIDs.Add(int.Parse(reader.GetString(0)));
            }
            return SelectRecipes(recipeIDs);
        }

        public List<Tag> GetTagsForRecipe(int recipeID)
        {
            List<Tag> tags = new List<Tag>();

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand(
                @"SELECT public.tags.* FROM public.recipe_tags, public.tags 
                        WHERE tags.tag_id = recipe_tags.tag_id AND recipe_id = @Recipe_ID", conn);
            cmd.Parameters.AddWithValue("Recipe_ID", recipeID);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // get the tag_id, and the name
                Tag newTag = new Tag(reader.GetInt32(0), reader.GetString(1));
                tags.Add(newTag);
            }

            return tags;
        }


        /// <summary>
        /// Queries the database for an ingredient by name, returning the existing entry if found.
        /// If there was no match, then the ingredient will be created.
        /// </summary>
        /// <param name="ingredientName"></param>
        /// <returns></returns>
        public Ingredient GetOrCreateIngredient(string ingredientName)
        {
            Ingredient outIngredient;

            // query the database to see if it exists
            using var conn = new NpgsqlConnection(connString);
            conn.Open();


            // to handle existing ingredients, we can leverage RETURNING stmnt to return the existing
            // ingredient_id, should a conflict occur
            var cmd = new NpgsqlCommand(@"INSERT INTO ingredients (name) VALUES (@Name) ON CONFLICT (name) 
                                        DO UPDATE SET name = @Name RETURNING name, ingredient_id", conn);

            cmd.Parameters.AddWithValue("Name", ingredientName);
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
            return outIngredient;

        }


        public List<Recipe> SelectRecipeByCooktime(int cooktime)
        {
            List<int> recipeIDs = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand(@"SELECT r.recipe_id, name, description, cook_time_mins, course, rating, image servings, author_id
                                FROM recipes AS r
                                JOIN recipe_tags rt ON r.recipe_id = rt.recipe_id
                                WHERE r.cook_time_mins = @Cooktime;", conn);

            cmd.Parameters.AddWithValue("Cooktime", cooktime);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIDs.Add(int.Parse(reader.GetString(0)));
            }
            return SelectRecipes(recipeIDs);
        }

        public RecipeDeletionError DeleteRecipe(int inID)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand(@"DELETE FROM recipes WHERE recipe_id = @RecipeID", conn);
            cmd.Parameters.AddWithValue("RecipeID", inID);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                return RecipeDeletionError.NoError;
            }
            else
            {
                return RecipeDeletionError.DBDeletionError;
            }
        }

        // I added this method to get all the ID's of the recipes in the database
        // so I can make a list of recipe ID's to pass to SelectRecipes() Method

        public List<int> GetAllRecipeIds()
        {
            List<int> recipeIds = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT recipe_id FROM recipes", conn);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIds.Add(reader.GetInt32(0));
            }
            reader.Close();
            return recipeIds;
        }

        public static String GetConnectionString()
        {
            //initialize the string builder
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            //set the properties of the string builder
            connStringBuilder.Host = "third-sphinx-13032.5xj.cockroachlabs.cloud";
            connStringBuilder.Port = PORT_NUMBER;
            connStringBuilder.SslMode = SslMode.VerifyFull;
            connStringBuilder.Username = dbUsername;
            connStringBuilder.Password = dbPassword;
            connStringBuilder.Database = "defaultdb";
            connStringBuilder.ApplicationName = "";
            connStringBuilder.IncludeErrorDetail = true;
            //return the completed string
            return connStringBuilder.ConnectionString;
        }
    }
}
