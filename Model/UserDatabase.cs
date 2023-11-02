using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class UserDatabase 
    {
        private List<string> followersList = new List<string>();
        private List<string> followingList = new List<string>();

        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;

        private IRecipeLogic recipeDB = new RecipeDatabase();

        public UserDatabase(IRecipeLogic recipeLogic)
        {
            this.recipeDB = recipeLogic;
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

        public ObservableCollection<User> Follow(string userID)
        {
            if (followingList.Contains(userID))
            {
                Console.WriteLine("Tried to follow someone who is already followed");
                return null;
            }
            followingList.Add(userID);
            return SelectAllUsers(followingList);
        }

        public ObservableCollection<User> Unfollow(string userID)
        {
            if (followingList.Contains(userID))
            {
                followingList.Add(userID);
                return SelectAllUsers(followingList);

            }
            Console.WriteLine("Tried to unfollow someone who isn't followed");
            return null;

        }

        public ObservableCollection<User> LoadFollowers(List<string> followers)
        {
            return SelectAllUsers(followers);
        }

        public User SelectUser(int userID)
        {
            throw new NotImplementedException();
        }

        public UserAdditionError InsertUser(User inUser)
        {
            return UserAdditionError.NoError;
        }
        /// <summary>
        /// Updates a users info in the User table
        /// </summary>
        /// <param name="user">The updated user information</param>
        /// <returns></returns>
        public UserEditError EditUser(User user)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // since multiple tables 
            using var transaction = conn.BeginTransaction();
            try
            {
                // handle user info
                using (var cmd = new NpgsqlCommand("UPDATE users SET username = @Username, email = @Email, password = @Password, profile_pic = @ProfilePic WHERE id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Email", user.Email);
                    cmd.Parameters.AddWithValue("Password", user.Password);
                    cmd.Parameters.AddWithValue("ProfilePic", user.ProfilePicture);
                    cmd.Parameters.AddWithValue("UserId", user.Id);
                    cmd.ExecuteNonQuery();
                }

                // now do the settings
                using (var cmd = new NpgsqlCommand("UPDATE user_settings SET settings = @Settings WHERE user_id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("Settings", string.Join(",", user.AppPreferences));
                    cmd.Parameters.AddWithValue("UserId", user.Id);
                    cmd.ExecuteNonQuery();
                }

                // and update the dietary preferences
                using (var cmd = new NpgsqlCommand("DELETE FROM dietary_preferences WHERE user_id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("UserId", user.Id);
                    cmd.ExecuteNonQuery();
                }

                foreach (var pref in user.DietaryPreferences)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO dietary_preferences(user_id, preferences) VALUES(@UserId, @Preference)", conn))
                    {
                        cmd.Parameters.AddWithValue("UserId", user.Id);
                        cmd.Parameters.AddWithValue("Preference", pref);
                        cmd.ExecuteNonQuery();
                    }
                }

                // clear & update following list
                using (var cmd = new NpgsqlCommand("DELETE FROM user_following_user WHERE follower_user_id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("UserId", user.Id);
                    cmd.ExecuteNonQuery();
                }

                foreach (var followingUserId in user.Following)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO user_following_user(follower_user_id, followed_user_id) VALUES(@UserId, @FollowingUserId)", conn))
                    {
                        cmd.Parameters.AddWithValue("UserId", user.Id);
                        cmd.Parameters.AddWithValue("FollowingUserId", followingUserId);
                        cmd.ExecuteNonQuery();
                    }
                }
                // authored recipes and Cookbook updates are excluded here as they get handled elsewhere

                // commit the transaction
                transaction.Commit();
            }
            catch
            {
                // Rollback any changes if an error occurs
                transaction.Rollback();
                return UserEditError.
            }
        }

        public List<User> SelectAllUsers(List<string> userIDs)
        {
            ObservableCollection<User> outUsers = new ObservableCollection<User>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command

            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            foreach (string userID in userIDs)
            {
                User user = new User();
                // append the ranges from junction tables by using ARRAY_AGG as we do down here
                cmd.CommandText = @"SELECT
                                    u.username, u.email, u.password, u.profile_pic,
                                    user_settings.settings, dp.preferences AS dietary_preferences,
                                    ARRAY_AGG(DISTINCT uf.followed_user_id) AS followers,
                                    ARRAY_AGG(DISTINCT r.recipe_id) AS authored_recipes,
                                    ARRAY_AGG(DISTINCT dp.user_id) AS preferences
                                FROM users AS u
                                LEFT JOIN user_following_user AS uf ON u.id = uf.follower_user_id
                                LEFT JOIN public.recipes AS r ON u.id = r.author_id
                                LEFT JOIN public.user_settings AS user_settings ON u.id = user_settings.user_id
                                LEFT JOIN public.dietary_preferences AS dp ON u.id = dp.user_id
                                GROUP BY u.id, u.username, u.email, u.password, u.profile_pic, user_settings.settings, dp.preferences;";

                cmd.Parameters.AddWithValue("Recipe_ID", userID);
                reader = cmd.ExecuteReader();

                // ugly double nesting here, but should be okay with small ranged queries
                while (reader.Read())
                {
                    user.Username = reader.GetString(0);
                    user.Email = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.AppPreferences = CreateStringCollection(reader.GetString(3));
                    user.DietaryPreferences = CreateStringCollection(reader.GetString(4));
                    user.ProfilePicture = reader.GetString(5);
                    user.AuthorList = RecipeDB.SelectAllRecipes(CreateIntList(reader.GetString(6)));
                    user.Followers = CreateStringList(reader.GetString(7));
                    user.Following = CreateStringList(reader.GetString(8));
                    outUsers.Add(user);
                }
            }
            return outUsers;
        }
    }
}
