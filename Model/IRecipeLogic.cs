using System.Collections.ObjectModel;

namespace CookNook.Model
{
    public interface IRecipeLogic
    {
        RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, string inAuthor,
            ObservableCollection<string> inIngredients, ObservableCollection<string> inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            String inTags, String inFollowers);

        RecipeAdditionError AddRecipe(Recipe recipe);

        RecipeEditError EditRecipe(Recipe recipe);

        RecipeDeletionError DeleteRecipe(Recipe recipe);

        Recipe FindRecipe(int id);
    }
}

