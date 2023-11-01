
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

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
            // query the FollowingRecipe table for <userId, followingId>

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public UserSelectionError IsFollowingRecipeById(int userId, int recipeId)
        {
            // [SQL] check if an entry in FollowingUsers exists for <userId, recipeId>

            
            // if found, return RecipeSelectionError.RecipeAlreadyFollowed

            // else return NoError
            return UserSelectionError.NoError;

        }

        public List<User> GetAllUsers()
        {
            users.Clear();
            try
            {
                //connect to db 
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // write our query
                using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_pic FROM users",
                    conn);
                using var reader = cmd.ExecuteReader();
                
                // might not want to user a while loop here

                // Q: 

                while(reader.Read())
                {
                    //unpack object
                    // debug as strings for now
                    string username = reader.GetString(0);
                    string email = reader.GetString(1);
                    string password = reader.GetString(2);
                    // our preferences must be cast to a collection
                    
                    string profile_pic = reader.GetString(3);


                    User newUser = new User
                    {
                        

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

       
        public UserSelectionError GetUserByEmail(string email)
        {
            // check if the email is present in the users
            User added = null;
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // write our query to find a user by email
            using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_pic FROM users" +
                "WHERE email = @email", conn);

            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
            // [T] 
                
            }

            // [F]
            return UserSelectionError.NoUserWithEmail;
        }

        public UserSelectionError GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserAdditionError InsertUser(User inUser)
        {
            throw new NotImplementedException();
        }

        public UserEditError EditUser(User inUser)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // write our query
                using var cmd = new NpgsqlCommand("SELECT username, email, password, profile_pic FROM users",
                    conn);
                using var reader = cmd.ExecuteReader();

            }
        }

        public UserDeletionError DeleteUser(User inUser)
        {
            throw new NotImplementedException();
        }

    }
}
