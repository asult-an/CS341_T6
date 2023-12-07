using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CookNook.Model;
using CookNook.Model.Interfaces;
using Npgsql;

namespace CookNook.Services
{
    internal class RecipeDatabase : IRecipeDatabase
    {
        private IIngredientDatabase ingredientDatabase;
        // private IIngredientLogic ingredientLogic;

        public RecipeDatabase()
        {
            this.ingredientDatabase = new IngredientDatabase();
        }

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="ingredientDatabase"></param>
        public RecipeDatabase(IIngredientDatabase ingredientDatabase)
        {
            this.ingredientDatabase = ingredientDatabase;
        }

        private List<Int64> authorListIDs = new List<Int64>();
        private List<Int64> cookbookIDs = new List<Int64>();
        
        // is this supposed to store recipes created by followed users?
        private List<Recipe> authorList;
        
        private List<Recipe> cookbook;
        private string connString = DbConn.ConnectionString;
        
        // TODO: Users need a way to upde

        //create public property to access airport list
        public List<Recipe> AuthorList { get { return authorList; } set { authorList = value; } }
        public List<Recipe> Cookbook { get { return cookbook; } set { cookbook = value; } }

        public RecipeDeletionError DeleteFromAuthorList(Int64 recipeID)
        {
            authorListIDs.Remove(recipeID);
            DeleteRecipe(recipeID);
            authorList = SelectRecipes(authorListIDs);
            return RecipeDeletionError.NoError;

        }

        public RecipeDeletionError DeleteFromCookbook(Int64 recipeID)
        {
            cookbookIDs.Remove(recipeID);
            cookbook = SelectRecipes(cookbookIDs);
            return RecipeDeletionError.NoError;

        }

        public RecipeAdditionError AddToAuthorList(Int64 recipeID)
        {
            authorListIDs.Add(recipeID);
            authorList = SelectRecipes(authorListIDs);
            return RecipeAdditionError.NoError;

        }

        public RecipeAdditionError AddToCookbook(Int64 recipeID)
        {
            cookbookIDs.Add(recipeID);
            cookbook = SelectRecipes(cookbookIDs);

            return RecipeAdditionError.NoError;

        }

        /// <summary>
        /// Gets all recipes that a user has authored
        /// </summary>
        /// <param name="userID">The userID of the author to query</param>
        /// <returns>List of type Recipe</returns>
        public List<Recipe> GetRecipesByUserId(Int64 userID)
        {
            List<Recipe> outRecipes = new List<Recipe>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // TODO: fully qualify the asterisk 
            var cmd = new NpgsqlCommand(
                @"SELECT recipe_id, name, description, cook_time_mins, image FROM public.recipes 
                            WHERE author_id = @User_ID;", conn);

            cmd.Parameters.AddWithValue("User_ID", userID);

            using (var reader = cmd.ExecuteReader())
            {
                Debug.Write($"UserID from GetRecepeByUserId: ${userID}");
                while (reader.Read())
                {
                    Recipe recipe = new Recipe
                    {
                        ID = reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        CookTime = reader.GetInt32(3),
                        AuthorID = userID,
                    };

                    if (!reader.IsDBNull(4))
                    {
                        long dataLength = reader.GetBytes(4, 0, null, 0, 0); // first need to get length of image
                        byte[] imageData = new byte[dataLength];
                        reader.GetBytes(4, 0, imageData, 0, (int)dataLength); //  then read the image data
                        recipe.Image = imageData;
                    }
                    else
                    {
                        recipe.Image = null;
                    }

                    outRecipes.Add(recipe);
                }

            }
            Debug.WriteLine("\n\n\n\n");
            return outRecipes;
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
        public List<Int64> GetRecipeFollowerIds(Int64 recipeID)
        {
            List<Int64> userIds = new List<Int64>();
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
            cmd.Parameters.AddWithValue("Course", inRecipe.Course.Name);
            cmd.Parameters.AddWithValue("Rating", inRecipe.Rating);
            cmd.Parameters.AddWithValue("Servings", inRecipe.Servings);
            cmd.Parameters.AddWithValue("Image", inRecipe.Image);
            cmd.Parameters.AddWithValue("AuthorID", inRecipe.AuthorID);


            // first, we need the new ID of that recipe!
            var result = cmd.ExecuteScalar();
            Int64 recipeID = Int64.Parse(result.ToString());

            if(result == null)
            {
                Debug.WriteLine("Null result: ", result);
                return RecipeAdditionError.DBAdditionError; 
            }
            // todo: FIX NAME BINDING
            else
            {
                Debug.WriteLine("Result: ", result);
            }
            

            foreach (Ingredient ing in inRecipe.Ingredients)
            {
                // insert row in the ingredients table
                // TODO: don't add existing ingredients when adding a new recipe
                // TODO: handle if null unit
                Int64 ingredientId;

                // Check if ingredient exists and get ID, else insert and get new ID
                cmd = new NpgsqlCommand("SELECT ingredient_id FROM ingredients WHERE name = @Name", conn);
                cmd.Parameters.AddWithValue("Name", ing.Name);
                var ingResult = cmd.ExecuteScalar();
                // if the ingredient DOESN'T exist:
                if (ingResult == null)
                {
                    // Method used to bypass autogenerated id during demo:
                    //cmd = new NpgsqlCommand("INSERT INTO ingredients (name) VALUES (@Name) RETURNING ingredient_id", conn);
                    
                    cmd = new NpgsqlCommand("INSERT INTO ingredients (ingredient_id, name) VALUES (nextval('ingredients_id_seq'), @Name) RETURNING ingredient_id", conn);

                    cmd.Parameters.AddWithValue("Name", ing.Name);
                    var newIngResult = cmd.ExecuteScalar();
                    ingredientId = Int64.Parse(newIngResult.ToString());
                }
                else if (ingResult != null)
                {
                    // otherwise just use the existing ingredient'd ID
                    ingredientId = Int64.Parse(ingResult.ToString());

                    // add the new recipe_ingredients
                }
                else
                {
                    return RecipeAdditionError.DBAdditionError;
                }
                
                ing.IngredientId = ingredientId;
                // if (ingResult != null)
                // TODO:
                // SQL has to carry the NULL for Unit when dealing with unitless ingredients

                // now that we know the ingredient is in the table, we can enter it's recipe-specific information
                cmd = new NpgsqlCommand("INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity, unit) " +
                                        "VALUES (@RecipeID, @IngredientID, @Quantity, @Unit)", conn);
                cmd.Parameters.AddWithValue("RecipeID", recipeID);
                cmd.Parameters.AddWithValue("IngredientID", ingredientId);
                cmd.Parameters.AddWithValue("Quantity", ing.Quantity);
                // wipe `Unit` if necessary
                if (ing.Unit == ing.Name)
                    ing.Unit = null;
               
                cmd.Parameters.AddWithValue("Unit", ing.Unit);

                // not querying the database, just inserting to it, so we execute a NonQuery:
                cmd.ExecuteNonQuery();
            }

            // WARNING: Doesn't account for a tag being added that wasn't already in the database!
            // now we can handle any tags involved
            // Until we resolve tags properly, disabled for the sprint 3 demo 
            /**
            foreach(Tag tag in inRecipe.Tags)
            {
                int tagId;
                // Check if tag exists and get ID, else insert and get new ID
                cmd = new NpgsqlCommand("SELECT tag_id FROM tags WHERE name = @Name", conn);
                cmd.Parameters.AddWithValue("Name", tag.DisplayName);
                var tagResult = cmd.ExecuteScalar();

                if (tagResult == null)
                {
                    cmd = new NpgsqlCommand("INSERT INTO tags (name) VALUES (@Name) RETURNING tag_id", conn);
                    cmd.Parameters.AddWithValue("Name", tag.DisplayName);
                    var newTagResult = cmd.ExecuteScalar();
                    tagId = int.Parse(newTagResult.ToString());
                }
                else
                {
                    tagId = int.Parse(tagResult.ToString());

                }


                cmd = new NpgsqlCommand("INSERT INTO recipe_tags (recipe_id, tag_id) VALUES (@RecipeID, @TagID)", conn);
                
                // here we get to use that ID we grabbed earlier
                cmd.Parameters.AddWithValue("RecipeID", recipeID);
                cmd.Parameters.AddWithValue("TagID", tag.Id);
                cmd.ExecuteNonQuery();
            }
            */
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
                recipe.ID = reader.GetInt64(0);
                recipe.Name = reader.GetString(1);
                recipe.Description = reader.GetString(2);   
                recipe.AuthorID = reader.GetInt64(3);
                recipe.Ingredients = ingredientDatabase.GetIngredientsFromRecipe(recipe.ID);

                //recipe.IngredientsQty = reader.GetString(5);
                recipe.CookTime = reader.GetInt32(6);
                // CourseType is needed, so we have to use a helper function to convert it
                recipe.Course = CourseType.Parse(reader.GetString(7));
                recipe.Rating = reader.GetInt32(8);
                recipe.Servings = reader.GetInt32(9);
                //TDOD: updade image reader to GetBytes(10) rather than GetString(10)
                recipe.Image = Encoding.ASCII.GetBytes(reader.GetString(10));
                recipe.Tags = GetTagsForRecipe(recipe.ID).ToArray();
               
                // TODO: this probably isn't parsing properly
            //    recipe.Followers = reader.GetString(12);
                recipe.FollowerIds = GetRecipeFollowerIds(recipe.ID).ToArray();
                reader.Close();

                outRecipes.Add(recipe);
            }

            return outRecipes;
        }


        public List<Recipe> SelectRecipes(List<Int64> recipeList)
        {
            List<Recipe> outRecipes = new List<Recipe>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            foreach (Int64 recipeID in recipeList)
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
                        // ask the ingredientDatabase for this recipes ingredints
                        var ingredients = ingredientDatabase.GetIngredientsFromRecipe(recipeID);

                        Recipe recipe = new Recipe
                        {
                            ID = reader.GetInt64(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            CookTime = reader.GetInt32(3),
                            Course = CourseType.Parse(reader.GetString(4)),
                            Rating = reader.GetInt32(5),
                            Servings = reader.GetInt32(6),
                            AuthorID = reader.GetInt64(8),
                            Ingredients = ingredients
                        };

                        if (!reader.IsDBNull(7))
                        {
                            long dataLength = reader.GetBytes(7, 0, null, 0, 0); // first need to get length of image
                            byte[] imageData = new byte[dataLength];
                            reader.GetBytes(7, 0, imageData, 0, (int)dataLength); //  then read the image data
                            recipe.Image = imageData;
                        }
                        else
                        {
                            recipe.Image = null;
                        }

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
        /// <returns>The recipe if found, else null</returns>
        public Recipe SelectRecipe(Int64 inID)
        {
            Recipe recipe = new Recipe();

            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand(@"SELECT r.recipe_id, name, description, cook_time_mins, course, rating, image servings, author_id
                                FROM recipes AS r
                                JOIN recipe_tags rt ON r.recipe_id = rt.recipe_id
                                WHERE rt.recipe_id = @RecipeId 
                                    AND @RecipeId = ", conn);
            cmd.Parameters.AddWithValue("RecipeId", inID);
            using var reader = cmd.ExecuteReader();
            reader.Read();

            // check if nothing was found:
            if (reader.HasRows == false)
                return null;

            recipe.ID = reader.GetInt64(0);
            recipe.Name = reader.GetString(1);
            recipe.Description = reader.GetString(2);

            recipe.AuthorID = reader.GetInt32(3);
            recipe.Ingredients = ingredientDatabase.GetIngredientsFromRecipe(inID);
            recipe.CookTime = reader.GetInt32(6);
            recipe.Course = CourseType.Parse(reader.GetString(7));

            recipe.Rating = reader.GetInt32(8);

            recipe.Servings = reader.GetInt32(9);
            //TODO switch to reader.GetBytes
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
            List<Int64> recipeIDs = new List<Int64>();
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
                recipeIDs.Add(Int64.Parse(reader.GetString(0)));
            }
            return SelectRecipes(recipeIDs);
        }

        public List<Tag> GetTagsForRecipe(Int64 recipeID)
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
                Tag newTag = new Tag(reader.GetInt64(0), reader.GetString(1));
                tags.Add(newTag);
            }

            return tags;
        }

        public List<Recipe> SelectRecipeByCooktime(int cooktime)
        {
            List<Int64> recipeIDs = new List<Int64>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand(@"SELECT r.recipe_id, name, description, cook_time_mins, course, rating, image, servings, author_id
                                FROM recipes AS r
                                JOIN recipe_tags rt ON r.recipe_id = rt.recipe_id
                                WHERE r.cook_time_mins = @Cooktime;", conn);

            cmd.Parameters.AddWithValue("Cooktime", cooktime);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIDs.Add(Int64.Parse(reader.GetString(0)));
            }
            return SelectRecipes(recipeIDs);
        }

        public RecipeDeletionError DeleteRecipe(Int64 inID)
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

        public List<Int64> GetAllRecipeIds()
        {
            List<Int64> recipeIds = new List<Int64>();
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

        //This method gets the recipes for a user cookbook
        
        public ObservableCollection<Recipe> CookbookRecipes(long userID)
        {
            //Calls GetRecipeByID method to retreive recipes from the database
            ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
            List<Recipe> recipesList = GetRecipesByUserId(userID);
            foreach (Recipe r in recipesList)
            {
                recipes.Add(r);
            }
            return recipes;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT recipe_id, name, description, cook_time_mins, image FROM recipes WHERE author_id=@UserID", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var recipe = new Recipe
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                CookTime = reader.GetInt32(3),
                                Image = reader.IsDBNull(4) ? null : Encoding.ASCII.GetBytes(reader.GetString(4))
                            };

                            recipes.Add(recipe);
                        }
                    }
                }
            }
            return recipes;
        }


    }
}
