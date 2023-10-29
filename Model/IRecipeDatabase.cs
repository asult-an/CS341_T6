using CookNook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal interface IRecipeDatabase
    {
        public RecipeDeletionError DeleteFromAuthorList(int recipeID);
        public RecipeDeletionError DeleteFromCookbook(int recipeID);
        public RecipeAdditionError AddToAuthorList(int recipeID);
        public RecipeAdditionError AddToCookbook(int recipeID);


        public ObservableCollection<Recipe> SelectAllRecipes(List<int> recipeList);
        public Recipe SelectRecipe(int recipeID);
        public ObservableCollection<Recipe> SelectRecipeByCourse(string course);
        public ObservableCollection<Recipe> SelectRecipeByCooktime(int cooktime);

        public RecipeAdditionError InsertRecipe(Recipe inRecipe);
        public RecipeDeletionError DeleteRecipe(int inID);
        public RecipeEditError EditRecipe(Recipe inRecipe);
    }
}
