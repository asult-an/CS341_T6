using System.Collections.ObjectModel;

namespace CookNook.Model
{
    public interface IRecipeLogic
    {
        // this method may be redundant
        public RecipeAdditionError CreateRecipe(string inName, string inDescription, Int64 inAuthorId,

            string inIngredients, string inIngredientsQty,

            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            string inTags, string inFollowers);

        RecipeAdditionError AddRecipe(Recipe recipe);

        RecipeEditError EditRecipe(Recipe recipe);

        RecipeDeletionError DeleteRecipe(Recipe recipe);

        public List<Recipe> SelectRecipeByCooktime(int cooktime);

        // forgot what this does
        //public RecipeAdditionError AddToCookbook(int recipeID);

        /// <summary>
        /// returns ALL recipes
        /// </summary>
        /// <returns></returns>
        public List<Recipe> SelectAllRecipes();

        /// <summary>
        /// Selects a subset of recipes based on their Ids
        /// </summary>
        /// <param name="recipeList"></param>
        /// <returns></returns>
        public List<Recipe> SelectRecipes(List<Int64> recipeList);


        /// <summary>
        /// search for a recipe by its Id
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        Recipe FindRecipe(Int64 recipeID);

        /// <summary>
        /// Polls the recipe_followers relation for all rows with a given recipeID 
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        List<Int64> GetFollowerIds(Int64 recipeID);

        List<Tag> GetTagsForRecipe(Int64 recipeID);

        public ObservableCollection<Recipe> FeedRecipes();

        public ObservableCollection<Recipe> CookBookRecipes(long userID);
        void AddRating(int rating, long recipeId);
        int GetRating(long recipeId);
    }
}

