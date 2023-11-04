using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class UserDatabase : IUserDatabase
    {

        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;

        private IRecipeLogic recipeDB = new RecipeLogic();

        public UserDatabase(IRecipeLogic recipeLogic)
        {
            this.recipeDB = recipeLogic;
        }


        public static string GetConnectionString()
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


        public UserSelectionError UnfollowUser(int userId, int followedUserId)
        {
            // if a row exists in user_following_user table where both userId and followingId match the
            // supplied parameters, remove it.
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // add a row into user_following_table 
            var cmdChk = new NpgsqlCommand(@"SELECT COUNT(*) 
                                                    FROM user_following_user 
                                                    WHERE follower_user_id = @userId AND followed_user_id = @followedUserId", conn);
            cmdChk.Parameters.AddWithValue("followed_user_id", followedUserId);
            cmdChk.Parameters.AddWithValue("follower_user_id", userId);

            // if the count was 0, they weren't being followed
            int count = Convert.ToInt16(cmdChk.ExecuteScalar());

            if (count == 0)
            {
                //Console.WriteLine("Tried to unfollow someone who isn't followed");
                Console.WriteLine("Not following user");
                return UserSelectionError.UserAlreadyUnfollowed;
            }

            // otherwise perform the add now that we've checked
            var cmd = new NpgsqlCommand(@"DELETE FROM
                                                 user_following_user 
                                                 WHERE (follower_user_id = @userId, followed_user_id = @followedUserId)", conn);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"User with id {userId} has followed {followedUserId}");
                return UserSelectionError.NoError;
            }

            Console.WriteLine($"Failed to sub id {userId} to user {followedUserId}");
            return UserSelectionError.NoUserWithId;



        }


        /// <summary>
        /// Issues a select and insert statement dependent on the result of the first query
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followedUserId"></param>
        /// <returns></returns>
        public UserSelectionError FollowUser(int userId, int followedUserId)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // add a row into user_following_table 
            var cmdChk = new NpgsqlCommand(@"SELECT COUNT(*) 
                                                    FROM user_following_user 
                                                    WHERE follower_user_id = @UserId AND followed_user_id = @FollowedUserId", conn);
            cmdChk.Parameters.AddWithValue("UserId", userId);
            cmdChk.Parameters.AddWithValue("FollowedUserId", followedUserId);

            // if the count was 0, they weren't being followed
            int count = Convert.ToInt16(cmdChk.ExecuteScalar());

            if (count > 0)
            {
                //Console.WriteLine("Tried to unfollow someone who isn't followed");
                Console.WriteLine("Already following user");
                return UserSelectionError.UserAlreadyFollowed;
            }

            // otherwise perform the add now that we've checked
            var cmd = new NpgsqlCommand(@"INSERT INTO user_following_user 
                                                (follower_user_id, followed_user_id) VALUES (@UserId, @FollowedUserId)", conn);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"User with id {userId} has followed {followedUserId}");
                return UserSelectionError.NoError;
            }

            Console.WriteLine($"Failed to sub id {userId} to user {followedUserId}");
            return UserSelectionError.NoUserWithId;

        }

        /// <summary>
        /// Counts the number of followers a user has by quering the user_following_user table for 
        /// rows where followed_user_id matches userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followers"></param>
        /// <returns>List of the userIds following the User</returns>
        public int GetFollowerCount(int userId)
        {
            List<string> followers = new List<string>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // build a list of user ids from the user_following_users table
            var cmd = new NpgsqlCommand(@"SELECT COUNT(*) 
                                                    FROM user_following_user 
                                                    WHERE follower_user_id = @UserId", conn);
            cmd.Parameters.AddWithValue("UserId", userId);


            using var reader = cmd.ExecuteReader();
            
            // tally up each row the user has
            while (reader.Read())
            {
                followers.Add(reader.GetString(0)); // assuming the followed_user_id is of type string
            }

            return followers.Count; 
            //return GetUsersById(followers);
        }
    

        /// <summary>
        /// Selects a user by their userId
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUserById(int userID)
        {
            User user = null;
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"SELECT u.username, u.email, u.password, u.profile_pic,
                                        ARRAY_TO_STRING(user_settings.settings, ',') AS settings,
                                        ARRAY_TO_STRING(dp.preferences, ',') AS dietary_preferences
                                  FROM users AS u
                                  LEFT JOIN user_settings ON u.id = user_settings.user_id
                                  LEFT JOIN dietary_preferences AS dp ON u.id = dp.user_id
                                  WHERE u.id = @UserId", conn);
            cmd.Parameters.AddWithValue("UserId", userID);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    Username = reader.GetString(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    ProfilePicture = reader.GetString(3),
                    AppPreferences = new List<string>(reader.GetString(4).Split(',')),
                    DietaryPreferences = new List<string>(reader.GetString(5).Split(','))
                };
            }

            return user;
        }

        public UserAdditionError InsertUser(User user)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // since multiple tables 
            using var transaction = conn.BeginTransaction();
            try
            {
                // handle user info
                using (var cmd = new NpgsqlCommand(@"INSERT INTO users(username, email, password, profile_pic) 
                                                            VALUE(@Username, @Email, @Password, @ProfilePic", conn))
                {
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Email", user.Email);
                    cmd.Parameters.AddWithValue("Password", user.Password);
                    cmd.Parameters.AddWithValue("ProfilePic", user.ProfilePicture);
                    // set automatically by database on inserts
                    //cmd.Parameters.AddWithValue("UsreId", user.Id);
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

                // clear and update following list
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

                // commit the transaction
                transaction.Commit();
                return UserAdditionError.NoError;
            }
            catch
            {
                // Rollback any changes if an error occurs
                transaction.Rollback();
                return UserAdditionError.DBAdditionError;

            }

        }


        /// <summary>
        /// Updates a User in the database: touching the following tables in the process:
        /// (users, user_settings, dietary_preferences)
        /// </summary>
        /// <remarks>Since the User model is complex and requires multiple join operations, 
        /// we can alter our syntax from individual commands into using a full-blown transaction.
        /// 
        /// This offers the ability to rollback our attempted changes should something go wrong</remarks>
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
                
                // clear and update following list
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

                // commit the transaction
                transaction.Commit();
                return UserEditError.NoError;
            }
            catch
            {
                // Rollback any changes if an error occurs
                transaction.Rollback();
                return UserEditError.DBEditError;

            }
        }

        /// <summary>
        /// Queries the user_following_user table for rows where the 'followed_user_id' matches
        /// the userId.  Since a User stores a lot of properties, we only bother getting the 
        /// userIds from the result.
        /// </summary>
        /// <param name="userId">userId of the user being FOLLOWED</param>
        /// <returns>A list of UserIds belonging to followers of the passed userId</returns>
        public List<int> GetFollowerIds(int userId)
        {
            List<int> followerIds = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

        }
    
    

        /// <summary>
        /// Selects a range of users by their ids, and fully maps out the resulting User with its joined tables
        /// </summary>
        /// <param name="userIds">Ids to match in the result</param>
        /// <returns>List of populated Users if any could be found</returns>
        public List<User> GetUsersById(List<int> userIds)
        {
            List<User> outUsers = new List<User>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command

            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;

            foreach (int userId in userIds)
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

                cmd.Parameters.AddWithValue("Recipe_ID", userId);
                var reader = cmd.ExecuteReader();

                // ugly double nesting here, but should be okay with small ranged queries
                while (reader.Read())
                {
                    user.Username = reader.GetString(0);
                    user.Email = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.ProfilePicture = reader.GetString(3);

                    user.AppPreferences = new List<string>(reader.GetString(4).Split(','));
                    user.DietaryPreferences = new List<string>(reader.GetString(5).Split(','));
                    
                    // the integer columns get handled differently since they get parsed
                    user.AuthorList= new List<int>(Array.ConvertAll(reader.GetString(6).Split(','), int.Parse));
                    user.Following = new List<int>(Array.ConvertAll(reader.GetString(7).Split(','), int.Parse));
                    outUsers.Add(user);
                }
            }
            return outUsers;
        }

        /// <summary>
        /// Updates the user's preferences
        /// </summary>
        /// <param name="userId">the userId the settings belongs to</param>
        /// <param name="appPrefs">json-ified representation of the settings config</param>
        /// <returns></returns>
        public UserEditError UpdateUserInfo(int userId, string appPrefs)
        {

        }


        /// <summary>
        /// Deletes a user from the database.  Requires--at minimum--the USERNAME and the EMAIL
        /// of the user to be defined in order to justify the assumption that the request comes
        /// from the owner of the account/an admin
        /// </summary>
        /// <param name="targetForDelete">The partially/fully defined user-object</param>
        /// <returns>UserDeletionError based on operation: NoError if successful</returns>
        public UserDeletionError DeleteUser(User targetForDelete)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command

            //connect the database to the command

            //initialize a new SQL command
            using (var cmd = new NpgsqlCommand(@"DELETE FROM users WHERE email = @Email AND username = @Username", conn))
            {
                cmd.Parameters.AddWithValue("Email", targetForDelete.Email);
                cmd.Parameters.AddWithValue("Username", targetForDelete.Username);

                //If we want the user to enter password to confirm deletion, we can add that as a param here to check it
                //cmd.Parameters.AddWithValue("Password", targetForDelete.Password);
                cmd.ExecuteNonQuery();


                // shouldn't need this since CASCADE ON DELETE is set up, but left it here for now just in case we need it
                //foreach (var pref in targetForDelete.DietaryPreferences)
                //{

                //    // update the dietary preferences
                //    using (var cmdDiet = new NpgsqlCommand("DELETE FROM dietary_preferences WHERE user_id = @UserId", conn))
                //    {
                //        cmdDiet.Parameters.AddWithValue("UserId", targetForDelete.Id);
                //        cmdDiet.ExecuteNonQuery();
                //    }

                //    // clear and update following list
                //    using (var cmdFollow = new NpgsqlCommand("DELETE FROM user_following_user WHERE follower_user_id = @UserId", conn))
                //    {
                //        cmdFollow.Parameters.AddWithValue("UserId", targetForDelete.Id);
                //        cmdFollow.ExecuteNonQuery();
                //    }

                //}

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return UserDeletionError.UserNotFound;
            
                return UserDeletionError.NoError;
            }
        }
    }
}
