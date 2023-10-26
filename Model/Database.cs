
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal class Database : IDatabase
    {

        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list
        public ObservableCollection<Recipe> Recipes { get { return recipes; } }
        public RecipeDeletionError DeleteRecipe(Recipe inRecipe)
        {
            throw new NotImplementedException();
        }

        public RecipeEditError EditRecipe(Recipe inRecipe)
        {
            throw new NotImplementedException();
        }

        public RecipeAdditionError InsertRecipe(Recipe inRecipe)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            //write the SQL statement to insert the airport into the database
            cmd.CommandText = "INSERT INTO recipes (recipe_id, name, description, author, ingredients_list, " +
                "ingredients_qty, cook_time_mins, course, rating, servings, image, tags, followers) VALUES " +
                "(@Recipe_ID, @Name, @Description, @Author, @IngredientsList, @IngredientsQty, @CookTimeMins, " +
                "@Course, @Rating, @Servings, @Image, @Tags, @Followers)";
            //extract the relevant data from the airport specified by the user
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

            return RecipeAdditionError.NoError;
        }

        public List<Recipe> SelectAllRecipes()
        {
            throw new NotImplementedException();
        }

        public Recipe SelectRecipe()
        {
            throw new NotImplementedException();
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

        public User SelectUser()
        {
            throw new NotImplementedException();
        }

        public UserAdditionError InsertUser(User inUser)
        {
            throw new NotImplementedException();
        }

        public UserEditError EditUser(User inUser)
        {
            throw new NotImplementedException();
        }

        public UserDeletionError DeleteUser(User inUser)
        {
            throw new NotImplementedException();
        }

    }
}
