using CookNook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IRecipeDatabase
    {
        public RecipeDeletionError DeleteFromAuthorList(int recipeID);
        public RecipeDeletionError DeleteFromCookbook(int recipeID);
        public RecipeAdditionError AddToAuthorList(int recipeID);
        public RecipeAdditionError AddToCookbook(int recipeID);
        public RecipeAdditionError InsertRecipe(Recipe inRecipe);
        public RecipeDeletionError DeleteRecipe(int inID);
        public RecipeEditError EditRecipe(Recipe inRecipe);


        public List<Ingredient> GetAllIngredients();

        /// <summary>
        /// Polls the recipe_ingredients relation for all rows with a given recipeID
        /// </summary>
        /// <param name="recipeID">Id to search for</param>
        /// <returns></returns>
        public List<Ingredient> GetIngredientsByRecipe(int recipeID);

        public List<int> GetRecipeFollowerIds(int recipeID);


        public List<Recipe> SelectAllRecipes();
        
        public List<Recipe> SelectRecipes(List<int> recipeList);
        public List<Recipe> SelectRecipeByCourse(string course);

        public Recipe SelectRecipe(int recipeID);

        //public List<Recipe> GetRecipesByCourseType(CourseType courseType);

        public List<Tag> GetTagsForRecipe(int recipeID);

        // public List<Tag> GetTagsByUser(int userID);

        //public List<Ingredient> GetIngredientsByUser(int userID);

        public Ingredient GetOrCreateIngredient(string ingredientName);

        public List<Recipe> SelectRecipeByCooktime(int cooktime);

    }
}
