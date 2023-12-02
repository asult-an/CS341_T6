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

        public List<Int64> GetRecipeFollowerIds(Int64 recipeID);

        public List<Recipe> SelectAllRecipes();
        
        public List<Recipe> SelectRecipes(List<Int64> recipeList);
        public List<Recipe> SelectRecipeByCourse(string course);

        public Recipe SelectRecipe(Int64 recipeID);

        //public List<Recipe> GetRecipesByCourseType(CourseType courseType);

        public List<Tag> GetTagsForRecipe(Int64 recipeID);

        // public List<Tag> GetTagsByUser(int userID);

        //public List<Ingredient> GetIngredientsByUser(int userID);

        public List<Recipe> SelectRecipeByCooktime(int cooktime);

        /// <summary>
        /// Returns all recipes authored by a given user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Recipe> GetRecipesByUserId(Int64 userID);

        public List<Int64> GetAllRecipeIds();

        public ObservableCollection<Recipe> CookbookRecipes(long userID);
    }
}
