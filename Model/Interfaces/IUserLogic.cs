﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Interfaces
{
    public interface IUserLogic
    {
        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="inUser">the user to delete</param>
        /// <returns></returns>
        public UserDeletionError DeleteUser(User inUser);

        /// <summary>
        /// Inserts a new user into the database, so long as their email is not already in use
        /// </summary>
        /// <param name="username">the username the user wishes to enter to log in and appear as</param>
        /// <param name="password">the secret password the user is going to use to log in</param>
        /// <param name="email">email to check for upon registration</param>
        /// <returns></returns>
        UserAdditionError RegisterNewUser(string username, string password, string email);

        /// <summary>
        /// Wrapping method that should surround RegisterNewUser to check for errors
        /// </summary>
        /// <param name="username">username entered</param>
        /// <param name="email">email entry input</param>
        /// <param name="password">input from password entry</param>
        /// <param name="confirmPassword">input entered into confirm password box</param>
        /// <returns></returns>
        UserAdditionError TryRegisterNewUser(string username, string email, string password, string confirmPassword);

        /// <summary>
        /// Updates a user's information in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(long id);

        public User GetUserByEmail(string email);

        public User GetUserByUsername(string username);

        public List<User> GetUsersById(List<long> userIds);

        // Note: this will be moved to a FollowerService next sprint
        // public GetFollowedUsers(long userId);
        
        // public UserEditError UpdateUserInfo(long id, string username, string password, string imgPath);

        public UserAuthenticationError AuthenticateUser(string username, string password);

        // public UserAuthenticationError AuthenticateUser(string username, string password, string email);


        public UserSelectionError FollowUser(long followerId, long followedId);

        public UserSelectionError UnfollowUser(long followerId, long followedId);

        public UserSelectionError FollowRecipe(long userId, long recipeId);

        public UserSelectionError UnfollowRecipe(long userId, long recipeId);

        public bool IsFollowingUser(long followerId, long followedId);

        public bool IsFollowingRecipe(long userId, long recipeId);

        public byte[] GetProfilePic(User user);

        public void SetProfilePic(User user, byte[] pic);
    }
}
