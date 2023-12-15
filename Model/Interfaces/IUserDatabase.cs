using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IUserDatabase
    {
        /// <summary>
        /// Authenticates a user by their username and password
        /// </summary>
        /// <param name="username">The username being used to log in</param>
        /// <param name="password">the password entered to check against the one used at registration</param>
        /// <returns></returns>
        public UserAuthenticationError AuthenticateUser(string username, string password);

        /* ===================== [ GETTERS ] ====================== */
        /// <summary>
        /// allows grabbing a subset of users by their ids, useful in 
        /// follower resolving
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        List<User> GetUsersById(List<Int64> userIds);

        /// <summary>
        /// Fetch a particular user by their email address.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>User object if found, null otherwise.</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Fetch a particular user by their userId.
        /// </summary>
        /// <param name="id">The ID of the User, not their Username.</param>
        /// <returns>User object if found, null otherwise. .</returns>
        User GetUserById(Int64 id);

        
        /// <summary>
        /// Retrieves a user by their username from the database
        /// </summary>
        /// <param name="username">username to search for</param>
        /// <returns></returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Query user_following_user table for all followed_user_ids where 
        /// follower_user_id matches the supplied userId.  Then, we call
        /// GetUserRange to return the rest
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Int64> GetFollowers(Int64 userId);
        
        /* ============== [ DATA MANIPULATION METHODS ] ================= */
        /**
         * These methods should use the User{...}Error from ErrorReporting to 
         * report back to the appropriate BusinessLogic whether or not the 
         * operation was completed successfully or had to abort
         */ 
        /// <summary>
        /// Queries the junction table from user-recipe to see if a certain
        /// recipe is being followed Iby a particular user.
        /// </summary>
        /// <param name="userId">ID of the user to look for.</param>
        /// <param name="recipeId">ID of the recipe we're checking.</param>
        /// <returns>NoError if successful, else an error.</returns>
        UserSelectionError IsFollowingRecipeById(Int64 userId, Int64 recipeId);

        /// <summary>
        /// Adds a user to the database if they aren't already present.
        /// </summary>
        /// <param name="inUser">User object to be added.</param>
        /// <returns>Error if the user couldn't be inserted.</returns>
        UserAdditionError InsertUser(User inUser);

        /// <summary>
        /// Modifies a user: useful for personal information changes or 
        /// when a password must be updated.
        /// </summary>
        /// <param name="inUser">User object with updated information.</param>
        /// <returns>Any errors that occurred during the edit operation.</returns>
        UserEditError EditUser(User inUser);

        /// <summary>
        /// Attempts to add a row into user_following_user table
        /// </summary>
        /// <param name="userId">userId of the follower </param>
        /// <param name="followerId">userId of the followed user</param>
        /// <returns>Noerror on success, else AlreadyFollowingUser</returns>
        UserSelectionError FollowUser(Int64 userId, Int64 followerId);

        /// <summary>
        /// Attempts to remove a row into user_following_user table
        /// </summary>
        /// <param name="userId">userId of the follower </param>
        /// <param name="followerId">userId of the followed user</param>
        /// <returns>NoError on success, else NoUserWithId</returns>
        UserSelectionError UnfollowUser(Int64 userId, Int64 followerId);

        /// <summary>
        /// To modify user settings, a call to the user_settings table is made
        /// where we update all followed_user_id by the follower_user_id
        /// </summary>
        /// <param name="userId">the id of the user we're polling</param>
        /// <param name="appPrefs">json collection of the settings</param>
        /// <returns></returns>
        UserEditError UpdateUserInfo(string userId, List<string> appPrefs);

        /// <summary>
        /// Removes a user from the database, if they exist.
        /// </summary>
        /// <param name="inUser">User object to be deleted.</param>
        /// <returns>Any errors that occurred during the deletion operation.</returns>
        UserDeletionError DeleteUser(User inUser);

        /// <summary>
        /// Gets the profile picture for a user, doesnt return anything else
        /// </summary>
        /// <param name="userID"> ID for pic </param>
        /// <returns> The picture </returns>
        public byte[] GetProfilePicture(Int64 userID);

        public bool SetProfilePicture(Int64 userID, byte[] imageBytes);


    }
}
