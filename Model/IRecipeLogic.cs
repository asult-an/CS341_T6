using System.Collections.ObjectModel;

namespace CookNook.Model
{
    public interface IRecipeLogic
    {
        RecipeAdditionError CreateRecipe(int inId, string inName, string inDescription, int inAuthor,
            ObservableCollection<string> inIngredients, ObservableCollection<String> inIngredientsQty,
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            ObservableCollection<string> inTags, ObservableCollection<string> inFollowers);

        RecipeAdditionError AddRecipe(Recipe recipe);

        RecipeEditError EditRecipe(Recipe recipe);

        RecipeDeletionError DeleteRecipe(Recipe recipe);

        Recipe FindRecipe(int id);
    }
}

