﻿using CookNook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IUserDatabase
    {
        /// <summary>
        /// Grabs all users from the database, seldom used.
        /// </summary>
        /// <returns>Collection of Users in a List.</returns>
        List<User> GetAllUsers();

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
        User GetUserById(int id);

        /// <summary>
        /// Queries the junction table from user-recipe to see if a certain
        /// recipe is being followed by a particular user.
        /// </summary>
        /// <param name="userId">ID of the user to look for.</param>
        /// <param name="recipeId">ID of the recipe we're checking.</param>
        /// <returns>NoError if successful, else an error.</returns>
        UserSelectionError IsFollowingRecipeById(int userId, int recipeId);

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
        /// Removes a user from the database, if they exist.
        /// </summary>
        /// <param name="inUser">User object to be deleted.</param>
        /// <returns>Any errors that occurred during the deletion operation.</returns>
        UserDeletionError DeleteUser(User inUser);
    }
}
