using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Interfaces
{

    /// <summary>
    /// handles more intensive operations on the cookbook, so anything like setting colors based on preferences will be done here
    /// </summary>
    internal interface ICookbookPageLogic
    {
        /// <summary>
        /// Used when the FIRST item in a NEW COOKBOOK LIST is created
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recipeID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageAdditionError AddRecipeToCookbookPage(long userID, long recipeID, string pageName);

        /// <summary>
        /// Removes an indiviual recipe from a particular Cookbook Page
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recipeID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageDeletionError RemoveRecipeFromCookbookPage(long userID, long recipeID, string pageName);


        /// <summary>
        /// Removes a cookbook page from the table
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageDeletionError DeleteCookbookPage(long userID, string pageName);

        /// <summary>
        /// retreives all cookbook pages that a particular user has authored
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CookbookPageModel> GetCookbookPagesForUser(long userID);

        /// <summary>
        /// Grabs all the recipe_ids that belong to a given cookbook page
        /// </summary>
        /// <param name="cookbookPageID"></param>
        /// <returns></returns>
        public List<long> GetRecipeIdsForCookbookPage(long cookbookPageID);


        /// <summary>
        /// Creates a new cookbook page for a user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public CookbookPageAdditionError CreateCookbookPage(long userID, string pageName);
    }

}
