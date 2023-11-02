
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Java.Nio.FileNio.Attributes;

namespace CookNook.Model
{
    public class UserDatabase : IUserDatabase
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list

        public ObservableCollection<User> User{ get { return users; } }
              
        public UserDatabase() {
            connString = GetConnectionString();
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

        /// <summary>
        /// Queries the database for users that are followed by another user's Id
        /// </summary>
        /// <param name="userId">The User who is doing the following</param>
        /// <param name="followingId">The user being followed</param>
        /// <returns></returns>
        private List<User> GetFollowedUsers(int userId, int followingId)
        {
            List<User> followedUsers = new List<User>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT username, email, password, profile_picture, app_preferences, dietary_preferences FROM users WHERE user_id IN (SELECT following_id FROM FollowingRecipe WHERE user_id = @userId AND following_id = @followingId)", conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("followingId", followingId);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                followedUsers.Add(new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    ProfilePicture = reader.GetString(3),
                    AppPreferences = new ObservableCollection<string>(reader.GetString(4).Split(',')),
                    DietaryPreferences = new ObservableCollection<string>(reader.GetString(5).Split(',')),
                    AuthorList = new ObservableCollection<Recipe>(),
                    FollowList = new ObservableCollection<User>(),
                    CookBook = new ObservableCollection<Recipe>()
                });
            }
            return followedUsers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public UserSelectionError IsFollowingRecipeById(int userId, int recipeId)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM UserRecipeFollows WHERE user_id = @userId AND recipe_id = @recipeId", conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("recipeId", recipeId);

            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
                return UserSelectionError.RecipeAlreadyFollowed;
            else
                return UserSelectionError.NoError;
        }

        public List<User> GetAllUsers()
        {
            users.Clear();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_picture, app_preferences, dietary_preferences FROM users", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    ProfilePicture = reader.GetString(3),
                    AppPreferences = new ObservableCollection<string>(reader.GetString(4).Split(',')),
                    DietaryPreferences = new ObservableCollection<string>(reader.GetString(5).Split(',')),
                    AuthorList = new ObservableCollection<Recipe>(),
                    FollowList = new ObservableCollection<User>(),
                    CookBook = new ObservableCollection<Recipe>()
                });
            }
            return users.ToList();
        }


        public User GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_picture, app_preferences, dietary_preferences FROM users WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    ProfilePicture = reader.GetString(3),
                    AppPreferences = new ObservableCollection<string>(reader.GetString(4).Split(',')),
                    DietaryPreferences = new ObservableCollection<string>(reader.GetString(5).Split(',')),
                    AuthorList = new ObservableCollection<Recipe>(),
                    FollowList = new ObservableCollection<User>(),
                    CookBook = new ObservableCollection<Recipe>()
                };
            }
            return null; //we can switch this to throw a specific exception
        }

        public User GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_picture, app_preferences, dietary_preferences FROM users WHERE user_id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    ProfilePicture = reader.GetString(3),
                    AppPreferences = new ObservableCollection<string>(reader.GetString(4).Split(',')),
                    DietaryPreferences = new ObservableCollection<string>(reader.GetString(5).Split(',')),
                    AuthorList = new ObservableCollection<Recipe>(),
                    FollowList = new ObservableCollection<User>(),
                    CookBook = new ObservableCollection<Recipe>()
                };
            }
            return null; // or throw a specific exception if you prefer
        }


        public UserAdditionError InsertUser(User inUser)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            try
            {
                cmd.CommandText = "INSERT INTO users (username, email, password, app_preferences, " +
                    "dietary_preferences, profile_picture, author_list, follow_list, cook_book) " +
                    "VALUES (@Username, @Email, @Password, @AppPreferences, @DietaryPreferences, " +
                    "ProfilePicture, AuthorList, FollowList, CookBook)";
                cmd.Parameters.AddWithValue("Username", inUser.Username);
                cmd.Parameters.AddWithValue("Email", inUser.Email);
                cmd.Parameters.AddWithValue("Password", inUser.Password);
                cmd.Parameters.AddWithValue("AppPreferences", inUser.AppPreferences);
                cmd.Parameters.AddWithValue("DietaryPreferences", inUser.DietaryPreferences);
                cmd.Parameters.AddWithValue("ProfilePicture", inUser.ProfilePicture);
                cmd.Parameters.AddWithValue("AuthorList", inUser.AuthorList);
                cmd.Parameters.AddWithValue("FollowList", inUser.FollowList);
                cmd.Parameters.AddWithValue("CookBook", inUser.CookBook);
                cmd.ExecuteNonQuery();
                return UserAdditionError.NoError;
            }
            catch (Exception ex)
            {
                return UserAdditionError.DBAdditionError;
            }

        }

        public UserEditError EditUser(User inUser)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            try
            {
                cmd.CommandText = "UPDATE users SET email = @Email, password = @Password, " +
                    "app_preferences = @AppPreferences, dietary_preferences = @Dietary_Preferences," +
                    " profile_picture = @ProfilePicture," +
                    " author_list = @AuthorList, follow_list = @FollowList, cookbook = @CookBook" +
                    " WHERE username = @Username;";
                cmd.Parameters.AddWithValue("Email", inUser.Email);
                cmd.Parameters.AddWithValue("Password", inUser.Password);
                cmd.Parameters.AddWithValue("AppPreferences", inUser.AppPreferences);
                cmd.Parameters.AddWithValue("DietaryPreferences", inUser.DietaryPreferences);
                cmd.Parameters.AddWithValue("ProfilePicture", inUser.ProfilePicture);
                cmd.Parameters.AddWithValue("AuthorList", inUser.AuthorList);
                cmd.Parameters.AddWithValue("FollowList", inUser.FollowList);
                cmd.Parameters.AddWithValue("CookBook", inUser.CookBook);
                cmd.ExecuteNonQuery();
                return UserEditError.NoError;
            }
            catch (Exception ex)
            {
                return UserEditError.DBEditError;
            }
        }

        public UserDeletionError DeleteUser(User inUser)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            //write the SQL statement to insert the recipe into the database
            cmd.CommandText = "DELETE FROM users WHERE username = @Username";
            cmd.Parameters.AddWithValue("Username", inUser.Username);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                return UserDeletionError.NoError;
            }
            else
            {
                return UserDeletionError.DBDeletionError;
            }
        }

    }
}
