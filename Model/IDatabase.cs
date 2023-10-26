using CookNook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal interface IDatabase
    {
        public List<Recipe> SelectAllRecipes();
        public Recipe SelectRecipe();
        public RecipeAdditionError InsertRecipe(Recipe inRecipe);
        public RecipeDeletionError DeleteRecipe(Recipe inRecipe);
        public RecipeEditError EditRecipe(Recipe inRecipe);

        public User SelectUser();
        public UserAdditionError InsertUser(User inUser);
        public UserEditError EditUser(User inUser);
        public UserDeletionError DeleteUser(User inUser);
    }
}
