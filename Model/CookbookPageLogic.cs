using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model.Interfaces;
using CookNook.Services;
using Npgsql;

namespace CookNook.Model
{
    internal class CookbookPageLogic : ICookbookPageLogic
    {
        /** NOTE TO ALL DEVELOPERS:
         * Take note here of what's known as the Composition pattern.
         * Here, our concrete implementaion of ICookbookPageLogic is composed of two other objects
         * since their responsibilities overlap with one another
         */
        private readonly ICookbookPageDatabase cookbookPageDatabase;
        private readonly IRecipeLogic recipeLogic;

        public CookbookPageLogic(ICookbookPageDatabase cookbookPageDatabase, IRecipeLogic recipeLogic)
        {
            this.cookbookPageDatabase = cookbookPageDatabase;
            this.recipeLogic = recipeLogic;
        }

        //public CookbookPageLogic(long userId) { }

        /// <summary>
        /// Associates a recipe with a cookbook page
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recipeID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageAdditionError AddRecipeToCookbookPage(long userID, long recipeID, string pageName)
        {
            // if the name of the page is null or empty, return an error:
            if(string.IsNullOrEmpty(pageName))
            {
                Debug.Write(@$"AddRecipeToCookbookPage: page title ({pageName}) is null or empty!");
                return CookbookPageAdditionError.InvalidListProvided;
            }

            // if the recipeID is invalid, return an error:
            if(recipeID < 0)
            {
                Debug.Write(@$"AddRecipeToCookbookPage: recipeID ({recipeID}) is invalid!");
                return CookbookPageAdditionError.InvalidRecipeProvided;
            }

            // if the userID is invalid, return an error:
            if(userID < 0)
            {
                Debug.Write(@$"AddRecipeToCookbookPage: userID ({userID}) is invalid!");
                return CookbookPageAdditionError.Unspecified;
            }

            var pageOfOperation = cookbookPageDatabase.GetCookbbookPageByName(userID, pageName);

            // now we can get the id of the list, so add the recipes to that list
            return cookbookPageDatabase.AddRecipeToCookbookPage(recipeID, pageOfOperation.ListId);
        }


        /// <summary>
        /// Removes a recipe from a particular cookbook page
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recipeID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageDeletionError RemoveRecipeFromCookbookPage(long userID, long recipeID, string pageName)
        {
            //TODO: refactor santiy checks into methods 
            if (string.IsNullOrEmpty(pageName))
            {
                Debug.Write(@$"AddRecipeToCookbookPage: page title ({pageName}) is null or empty!");
                return CookbookPageDeletionError.InvalidListProvided;
            }

            // if the recipeID is invalid, return an error:
            if (recipeID < 0)
            {
                Debug.Write(@$"AddRecipeToCookbookPage: recipeID ({recipeID}) is invalid!");
                return CookbookPageDeletionError.InvalidRecipeProvided;
            }

            // if the userID is invalid, return an error:
            if (userID < 0)
            {
                Debug.Write(@$"AddRecipeToCookbookPage: userID ({userID}) is invalid!");
                return CookbookPageDeletionError.Unspecified;
            }

            var pageOfOperation = cookbookPageDatabase.GetCookbbookPageByName(userID, pageName);
            return cookbookPageDatabase.RemoveRecipeFromCookbookPage(recipeID, pageOfOperation.ListId);
        }

        public CookbookPageDeletionError DeleteCookbookPage(long userID, string pageName)
        {
            throw new NotImplementedException();
        }

        public List<CookbookPageModel> GetCookbookPagesForUser(long userID)
        {
            return cookbookPageDatabase.GetCookbookPagesForUser(userID);
        }

        /// <summary>
        /// Through RecipeLogic, constructs all recipes belonging to a cookbook page
        /// </summary>
        /// <param name="cookbookPageID"></param>
        /// <returns></returns>
        public List<long> GetRecipeIdsForCookbookPage(long cookbookPageID)
        {
            return cookbookPageDatabase.GetRecipeIdsForCookbookPage(cookbookPageID);
        }


        /// <summary>
        ///  returns all recipies associated with this cookbookpage's ID
        /// </summary>
        /// <param name="cookbookPageID"></param>
        /// <returns>a List of recipeIds that can be handled by RecipeDatabase</returns>
        public List<Recipe> GetRecipesOnCookbookPage(long cookbookPageID)
        {
            // 1. get the recipes on the page
            var recipeIds = cookbookPageDatabase.GetRecipeIdsForCookbookPage(cookbookPageID);

            // 2. pass the list of recipes into RecipeLogic
            var recipes = recipeLogic.SelectRecipes(recipeIds);
            return recipes;
        }

        public CookbookPageAdditionError CreateCookbookPage(long userID, string pageName)
        {   
            throw new NotImplementedException();
        }
    }
}
