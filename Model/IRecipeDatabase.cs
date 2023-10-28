using CookNook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal interface IRecipeDatabase
    {
        public List<Recipe> SelectAllRecipes();
        public Recipe SelectRecipe();
        public RecipeAdditionError InsertRecipe(Recipe inRecipe);
        public RecipeDeletionError DeleteRecipe(Recipe inRecipe);
        public RecipeEditError EditRecipe(Recipe inRecipe);
    }
}
