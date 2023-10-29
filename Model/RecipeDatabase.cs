
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CookNook.Model
{
    internal class RecipeDatabase : IRecipeDatabase
    {
        //private recipe database fields
        //list of recipe IDs that a user has created
        private List<int> authorListIDs = new List<int>();
        //list of recipe IDs that a user has saved
        private List<int> cookbookIDs = new List<int>();
        //list of recipes that a user has created
        private ObservableCollection<Recipe> authorList;
        //list of recipes that a user has saved
        private ObservableCollection<Recipe> cookbook;
        //database connection string, username, password, port number
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public properties to access AuthorList and Cookbook
        public ObservableCollection<Recipe> AuthorList { get { return authorList; } set { authorList = value; } }
        public ObservableCollection<Recipe> Cookbook { get { return cookbook; } set { cookbook = value; } }

        /// <summary>
        /// Deletes recipe from authorlistIDs, updates authorlist and database
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        public RecipeDeletionError DeleteFromAuthorList(int recipeID)
        {
            authorListIDs.Remove(recipeID);
            DeleteRecipe(recipeID);
            authorList = SelectAllRecipes(authorListIDs);
            return RecipeDeletionError.NoError;
          
        }
        /// <summary>
        /// Deletes recipe from cookbookIDs, updates cookbook and database
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns>RecipeDeletionError : the result of the recipe modification</returns>
        public RecipeDeletionError DeleteFromCookbook(int recipeID)
        {
            cookbookIDs.Remove(recipeID);
            cookbook = SelectAllRecipes(cookbookIDs);
            return RecipeDeletionError.NoError;

        }
        /// <summary>
        /// Adds recipe to authorlistIDs, updates authorlist and database
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns>RecipeDeletionError : the result of the recipe modification</returns>
        public RecipeAdditionError AddToAuthorList(int recipeID)
        {
            authorListIDs.Add(recipeID);
            authorList = SelectAllRecipes(authorListIDs);
            return RecipeAdditionError.NoError;

        }
        /// <summary>
        /// Adds recipe to cookbookIDs, updates cookbook and database
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        public RecipeAdditionError AddToCookbook(int recipeID)
        {
            cookbookIDs.Add(recipeID);
            cookbook = SelectAllRecipes(cookbookIDs);
            return RecipeAdditionError.NoError;

        }
        /// <summary>
        /// Edits the fields of a given recipe, updates database, cookbook, and authorlist
        /// </summary>
        /// <param name="inRecipe"></param>
        /// <returns>RecipeEditError : the result of the recipe modificaiton</returns>
        public RecipeEditError EditRecipe(Recipe inRecipe)
        {
            try
            {
                //connect to and open the database
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                //initialize a new SQL command and connect it to the database
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                //write the SQL statement with the following paramters
                cmd.CommandText = "UPDATE recipes SET name = @name, description = @Description, " +
                    "ingredients_list = @IngredientsList, ingredients_qty = @IngredientsQty, cook_time_mins = @CookTimeMins," +
                    " course = @Course, rating = @Rating, servings = @Servings," +
                    " image = @Image, tags = @Tags, followers = @Followers" +
                    " WHERE recpie_id = @ID;";
                cmd.Parameters.AddWithValue("Recipe_ID", inRecipe.ID);
                cmd.Parameters.AddWithValue("Name", inRecipe.Name);
                cmd.Parameters.AddWithValue("Description", inRecipe.Description);
                cmd.Parameters.AddWithValue("Author", inRecipe.Author);
                cmd.Parameters.AddWithValue("IngredientsList", inRecipe.IngredientsToString());
                cmd.Parameters.AddWithValue("IngredientsQty", inRecipe.IngredientsQtyToString());
                cmd.Parameters.AddWithValue("CookTimeMins", inRecipe.CookTime);
                cmd.Parameters.AddWithValue("Course", inRecipe.Course);
                cmd.Parameters.AddWithValue("Rating", inRecipe.Rating);
                cmd.Parameters.AddWithValue("Servings", inRecipe.Servings);
                cmd.Parameters.AddWithValue("Image", inRecipe.Image);
                cmd.Parameters.AddWithValue("Tags", inRecipe.TagsToString());
                cmd.Parameters.AddWithValue("Followers", inRecipe.FollowersToString());
                //execute the command
                var numAffected = cmd.ExecuteNonQuery();
                //update the observable collection
                authorList = SelectAllRecipes(authorListIDs);
                cookbook = SelectAllRecipes(cookbookIDs);
            }
            catch (Npgsql.PostgresException pe)//catch any exceptions thrown by the database access statements
            {
                Console.WriteLine("Update failed, {0}", pe);
                //report result
                return RecipeEditError.DBEditError;
            }
            return RecipeEditError.NoError;
        }
        /// <summary>
        /// Adds a new recipe to the database and the user's authorlist
        /// </summary>
        /// <param name="inRecipe"></param>
        /// <returns>RecipeAdditionError : the result of the recipe addition</returns>
        public RecipeAdditionError InsertRecipe(Recipe inRecipe)
        {
            
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            //write the SQL statement to insert the recipe into the database
            cmd.CommandText = "INSERT INTO recipes (recipe_id, name, description, author, ingredients_list, " +
                "ingredients_qty, cook_time_mins, course, rating, servings, image, tags, followers) VALUES " +
                "(@Recipe_ID, @Name, @Description, @Author, @IngredientsList, @IngredientsQty, @CookTimeMins, " +
                "@Course, @Rating, @Servings, @Image, @Tags, @Followers)";
            //extract the relevant data from the recipe specified by the user
            cmd.Parameters.AddWithValue("Recipe_ID", inRecipe.ID);
            cmd.Parameters.AddWithValue("Name", inRecipe.Name);
            cmd.Parameters.AddWithValue("Description", inRecipe.Description);
            cmd.Parameters.AddWithValue("Author", inRecipe.Author);
            cmd.Parameters.AddWithValue("IngredientsList", inRecipe.IngredientsToString());
            cmd.Parameters.AddWithValue("IngredientsQty", inRecipe.IngredientsQtyToString());
            cmd.Parameters.AddWithValue("CookTimeMins", inRecipe.CookTime);
            cmd.Parameters.AddWithValue("Course", inRecipe.Course);
            cmd.Parameters.AddWithValue("Rating", inRecipe.Rating);
            cmd.Parameters.AddWithValue("Servings", inRecipe.Servings);
            cmd.Parameters.AddWithValue("Image", inRecipe.Image);
            cmd.Parameters.AddWithValue("Tags", inRecipe.TagsToString());
            cmd.Parameters.AddWithValue("Followers", inRecipe.FollowersToString());
            //execute the insert command
            cmd.ExecuteNonQuery();

            AddToAuthorList(inRecipe.ID);
            return RecipeAdditionError.NoError;
        }
        /// <summary>
        /// Generates an ObservableCollection of recipes from a list of recipe IDs
        /// </summary>
        /// <param name="recipeList"></param>
        /// <returns>ObservableCollection<Recipe> an ObservableCollection of recipes matching the given IDs</returns>
        public ObservableCollection<Recipe> SelectAllRecipes(List<int> recipeList)
        {
            ObservableCollection<Recipe> outRecipes = new ObservableCollection<Recipe>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            foreach (int recipeID in recipeList)
            {
                Recipe recipe = new Recipe();
                cmd.CommandText = "SELECT * FROM recipes WHERE recipe_id = @Recipe_ID";
                cmd.Parameters.AddWithValue("Recipe_ID", recipeID);
                reader = cmd.ExecuteReader();
                reader.Read();
                recipe.ID = reader.GetInt32(0);
                recipe.Name = reader.GetString(1);
                recipe.Description = reader.GetString(2);
                recipe.Author = reader.GetString(3);
                recipe.Ingredients = CreateStringCollection(reader.GetString(4));
                recipe.IngredientsQty = CreateStringCollection(reader.GetString(5));
                recipe.CookTime = reader.GetInt32(6);
                recipe.Course = reader.GetString(7);
                recipe.Rating = reader.GetInt32(8);
                recipe.Servings = reader.GetInt32(9);
                recipe.Image = reader.GetString(10);
                recipe.Tags = CreateStringCollection(reader.GetString(11));
                recipe.Followers = CreateStringCollection(reader.GetString(12));
                outRecipes.Add(recipe);
            }
            return outRecipes;
    
        }
        /// <summary>
        /// Queries the database for a recipe given a recipe ID
        /// </summary>
        /// <param name="inID"></param>
        /// <returns>Recipe : the recipe matching the given ID</returns>
        public Recipe SelectRecipe(int inID)
        {
            Recipe recipe = new Recipe();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM recipes WHERE recipe_id = @Recipe_ID";
            cmd.Parameters.AddWithValue("Recipe_ID", inID);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            
            recipe.ID = reader.GetInt32(0);
            recipe.Name = reader.GetString(1);
            recipe.Description = reader.GetString(2);
            recipe.Author = reader.GetString(3);
            recipe.Ingredients = CreateStringCollection(reader.GetString(4));
            recipe.IngredientsQty = CreateStringCollection(reader.GetString(5));
            recipe.CookTime = reader.GetInt32(6);
            recipe.Course = reader.GetString(7);
            recipe.Rating = reader.GetInt32(8);
            recipe.Servings = reader.GetInt32(9);
            recipe.Image = reader.GetString(10);
            recipe.Tags = CreateStringCollection(reader.GetString(11));
            recipe.Followers = CreateStringCollection(reader.GetString(12));

            return recipe;
            
        }
        /// <summary>
        /// Queries the database for recipes given a course value
        /// </summary>
        /// <param name="course"></param>
        /// <returns>ObservableCollection<Recipe> an ObservableCollection of recipes matching the given attribute</returns>
        public ObservableCollection<Recipe> SelectRecipeByCourse(string course)
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
            return SelectAllRecipes(recipeIDs);
        }
        
        /// <summary>
        /// Queries the database for recipes given a cooktime value
        /// </summary>
        /// <param name="cooktime"></param>
        /// <returns>ObservableCollection<Recipe> an ObservableCollection of recipes matching the given attribute</returns>
        public ObservableCollection<Recipe> SelectRecipeByCooktime(int cooktime)
        {
            List<int> recipeIDs = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM recipes WHERE cook_time_mins = @Cooktime";
            cmd.Parameters.AddWithValue("Cooktime", cooktime);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                recipeIDs.Add(int.Parse(reader.GetString(0)));
            }
            return SelectAllRecipes(recipeIDs);
        }

        /// <summary>
        /// Deletes a recipe from the database 
        /// </summary>
        /// <param name="inID"></param>
        /// <returns>RecipeDeletionError : the result of the deletion</returns>
        public RecipeDeletionError DeleteRecipe(int inID)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            //write the SQL statement to insert the recipe into the database
            cmd.CommandText = "DELETE FROM recipes WHERE recipe_id = @RecipeID";
            cmd.Parameters.AddWithValue("RecipeID", inID);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                DeleteFromAuthorList(inID);
                DeleteFromCookbook(inID);
                return RecipeDeletionError.NoError;
            }
            else
            {
                return RecipeDeletionError.DBDeletionError;
            }
        }

        /// <summary>
        /// Creates an ObservableCollection<string> given a comma separated string list
        /// </summary>
        /// <param name="rawList"></param>
        /// <returns>ObservableCollection<string></returns>
        private ObservableCollection<string> CreateStringCollection(string rawList)
        {
            List<string> list = rawList.Split(", ").ToList<String>();
            return new ObservableCollection<string>(list);

        }

        /// <summary>
        /// Generates the database connection string
        /// </summary>
        /// <returns>string: the database connection string</returns>
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
