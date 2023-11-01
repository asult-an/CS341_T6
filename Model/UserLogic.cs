using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace CookNook.Services
namespace CookNook.Model
{
    internal class UserLogic : IUserDatabase
    {
        // place for the injected datbase instance to load into
        private readonly IUserDatabase userDatabase;

        // since users can interact with recipies, inject RecipeLogic
        // may not use it now, but by doing this we can send recipe data to users
        // e.g notify user of followed recipe...?
        private readonly IRecipeLogic recipeLogic;

        // Unlike Lab3, actually implement Dependency Injection from DB to BL

        /// <summary>
        /// TEMPORARY USE ONLY:
        /// RecipeLogic needed to handle Recipe
        /// Creates a new UserService object with an instance of the database loaded
        /// via DI.  
        /// </summary>
        /// <param name="userDatabase"></param>
        [Obsolete]
        public UserLogic(IUserDatabase userDatabase)
        {
            this.userDatabase = userDatabase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDatabase"></param>
        /// <param name="recipeLogic"></param>
        public UserLogic(IUserDatabase userDatabase, IRecipeLogic recipeLogic)
        {

        }


        public bool RegisterNewUser(string username, string email)
        {
            return false;
        }
        
        /// <summary>
        /// Attempt to follow a recipe if not already following.  If
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public UserSelectionError FollowRecipe(int userId, int recipeId)
        {
            /* if recipeLogic injected:
             * check if recipeID is valid
             */

            // check if the recipe is already followed
            return userDatabase.IsFollowingRecipeById(userId, recipeId);

        }

        public List<User> GetAllUsers()
        {
            return userDatabase.GetAllUsers();
        }

        public UserSelectionError GetUserByEmail(string email)
        {
            return userDatabase.GetUserByEmail(email);
        }

        public UserSelectionError GetUserById(int id)
        {

            return userDatabase.GetUserById(id);
        }

        public UserAdditionError InsertUser(User inUser)
        {
            
        }

        public UserEditError EditUser(User inUser)
        {


        }

        public UserDeletionError DeleteUser(User inUser)
        {

        }
    }
}
