using CookNook;
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
        /// Grabs all users from the database, seldom used
        /// </summary>
        /// <returns>Collection of Users in a List</returns>
        public List<User> GetAllUsers();

        /// <summary>
        /// Fetch a particular user by their email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Error if mismatched credentials, or if no user
        /// with the credentials could be found</returns>
        public UserSelectionError GetByEmail(string email);

        /// <summary>
        /// Fetch a particular user by their userId
        /// </summary>
        /// <param name="id">the id of the User, not their Username</param>
        /// <returns></returns>
        public UserSelectionError GetUserById(int id);



        /// <summary>
        /// Queries the junction table from user-recipe to see if a certain
        /// recipe is being followed by a particular user
        /// </summary>
        /// <param name="userId">Id of the user to look for</param>
        /// <param name="recipeId">Id of the recipe we're checking</param>
        /// <returns>NoError if successful, else an error</returns>
        public UserSelectionError IsFollowingRecipeByid(int userId, int recipeId);

        /// <summary>
        /// Adds a user to the database if they aren't already present
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns>Error if the user couldn't be inserted</returns>
        public UserAdditionError InsertUser(User inUser);

        /// <summary>
        /// Modifies a user: useful for personal information changes or 
        /// when a password must be updated
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        public UserEditError EditUser(User inUser);

        /// <summary>
        /// Removes a user from the database, if they exist.
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        public UserDeletionError DeleteUser(User inUser);
    }
}
