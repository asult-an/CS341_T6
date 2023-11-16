using System.Collections.ObjectModel;

namespace CookNook.Model
{
    public interface IRecipeLogic
    {

        RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthorID,
            string inIngredients, string inIngredientsQty,

            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            string inTags, string inFollowers);

        RecipeAdditionError AddRecipe(Recipe recipe);

        RecipeEditError EditRecipe(Recipe recipe);

        RecipeDeletionError DeleteRecipe(Recipe recipe);

        Recipe FindRecipe(int id);
    }
}

