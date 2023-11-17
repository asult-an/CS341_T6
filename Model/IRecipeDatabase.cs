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
        public RecipeDeletionError DeleteFromAuthorList(Int64 recipeID);
        public RecipeDeletionError DeleteFromCookbook(Int64 recipeID);
        public RecipeAdditionError AddToAuthorList(Int64 recipeID);
        public RecipeAdditionError AddToCookbook(Int64 recipeID);
        public RecipeAdditionError InsertRecipe(Recipe inRecipe);
        public RecipeDeletionError DeleteRecipe(Int64 inID);
        public RecipeEditError EditRecipe(Recipe inRecipe);


        public List<Ingredient> GetAllIngredients();

        /// <summary>
        /// Polls the recipe_ingredients relation for all rows with a given recipeID
        /// </summary>
        /// <param name="recipeID">Id to search for</param>
        /// <returns></returns>
        public List<Ingredient> GetIngredientsByRecipe(Int64 recipeID);

        public List<Int64> GetRecipeFollowerIds(Int64 recipeID);


        public List<Recipe> SelectAllRecipes();
        
        public List<Recipe> SelectRecipes(List<Int64> recipeList);
        public List<Recipe> SelectRecipeByCourse(string course);

        public Recipe SelectRecipe(Int64 recipeID);

        //public List<Recipe> GetRecipesByCourseType(CourseType courseType);

        public List<Tag> GetTagsForRecipe(Int64 recipeID);

        // public List<Tag> GetTagsByUser(int userID);

        //public List<Ingredient> GetIngredientsByUser(int userID);

        public Ingredient GetOrCreateIngredient(string ingredientName);

        public List<Recipe> SelectRecipeByCooktime(int cooktime);

        /// <summary>
        /// Returns all recipes authored by a given user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Recipe> GetRecipesByUserId(Int64 userID);
    }
}
