using System.Collections.ObjectModel;

namespace CookNook.Model
{
    public interface IRecipeLogic
    {
        // this method may be redundant
        public RecipeAdditionError CreateRecipe(string inName, string inDescription, int inAuthorId,
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
        public List<Recipe> SelectRecipes(List<int> recipeList);


        /// <summary>
        /// search for a recipe by its Id
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        Recipe FindRecipe(int recipeID);

        /// <summary>
        /// Polls the recipe_followers relation for all rows with a given recipeID 
        /// </summary>
        /// <param name="recipeID"></param>
        /// <returns></returns>
        List<int> GetFollowerIds(int recipeID);
 

        /// <summary>
        /// Polls the recipe_ingredients relation for all rows with a given recipeID
        /// </summary>
        /// <param name="recipeID">Id to search for</param>
        /// <returns></returns>
        List<Ingredient> GetIngredientsByRecipe(int recipeID);

        public List<Ingredient> GetAllIngredients();

        /// <summary>
        /// Creates an ingredient if it doesn't already exist.  Returns the entered 
        /// ingredient if found, otherwise adds into the database.
        /// </summary>
        /// <param name="ingredientName"></param>
        /// <returns></returns>
        public Ingredient GetOrCreateIngredient(string ingredientName);


        List<Tag> GetTagsForRecipe(int recipeID);

    }
}

