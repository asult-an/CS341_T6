
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

        private List<int> authorListIDs;
        private List<int> cookbookIDs;
        private ObservableCollection<Recipe> authorList;
        private ObservableCollection<Recipe> cookbook;
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list
        public ObservableCollection<Recipe> AuthorList { get { return authorList; } set { authorList = value; } }
        public ObservableCollection<Recipe> Cookbook { get { return cookbook; } set { cookbook = value; } }

        public RecipeDeletionError DeleteFromAuthorList(int recipeID)
        {
            authorListIDs.Remove(recipeID);
            DeleteRecipe(recipeID);
            authorList = SelectAllRecipes(authorListIDs);
            return RecipeDeletionError.NoError;
          
        }

        public RecipeDeletionError DeleteFromCookbook(int recipeID)
        {
            cookbookIDs.Remove(recipeID);
            cookbook = SelectAllRecipes(cookbookIDs);
            return RecipeDeletionError.NoError;

        }

        public RecipeAdditionError AddToAuthorList(int recipeID)
        {
            authorListIDs.Add(recipeID);
            authorList = SelectAllRecipes(authorListIDs);
            return RecipeAdditionError.NoError;

        }

        public RecipeAdditionError AddToCookbook(int recipeID)
        {
            cookbookIDs.Add(recipeID);
            cookbook = SelectAllRecipes(cookbookIDs);
            return RecipeAdditionError.NoError;

        }

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
                recipe.Author = reader.GetInt32(3);
                recipe.Ingredients = reader.GetString(4);
                recipe.IngredientsQty = reader.GetString(5);
                recipe.CookTime = reader.GetInt32(6);
                recipe.Course = reader.GetString(7);
                recipe.Rating = reader.GetInt32(8);
                recipe.Servings = reader.GetInt32(9);
                recipe.Image = reader.GetString(10);
                recipe.Tags = reader.GetString(11);
                recipe.Followers = reader.GetString(12);
                outRecipes.Add(recipe);
            }
            return outRecipes;
    
        }

        public Recipe SelectRecipeByID(int inID)
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
            recipe.Author = reader.GetInt32(3);
            recipe.Ingredients = reader.GetString(4);
            recipe.IngredientsQty = reader.GetString(5);
            recipe.CookTime = reader.GetInt32(6);
            recipe.Course = reader.GetString(7);
            recipe.Rating = reader.GetInt32(8);
            recipe.Servings = reader.GetInt32(9);
            recipe.Image = reader.GetString(10);
            recipe.Tags = reader.GetString(11);
            recipe.Followers = reader.GetString(12);

            return recipe;
            
        }

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
        /* Sort by tag (same for ingredients):
         * taglist is List<int>
         * 
         * retreive tag list + id for all recipes: map(int id:string tagList)
         * parse tag list ---> map(int id:List tagList)
         * search for desired tag in each list, add to output list: .Add(int id)
         * SelectAllRecipes(List<int> ids)
         * 
         * 
         */
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
                return RecipeDeletionError.NoError;
            }
            else
            {
                return RecipeDeletionError.DBDeletionError;
            }
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
