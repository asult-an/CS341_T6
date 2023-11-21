using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IIngredientLogic
    {
        /// <summary>
        /// Inserts a new ingredient into the database, as long as an
        /// existing ingredient is already present 
        /// </summary>
        /// <param name="ingredientId">UNCHANGED: used to anchor the existing row</param>
        /// <param name="name">the name of the ingredient</param>
        /// <param name="unit">Nullable unit field, in case ingredient is unitless</param>
        /// <returns>IngredientError</returns>
        IngredientAdditionError CreateIngredient(Int64 ingredientId, string name, string? unit);

        /// <summary>
        /// Update an ingredient 
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        IngredientUpdateError UpdateIngredient(Ingredient ingredient);

        /// <summary>
        /// Deletes an ingredient from the database, if it exists.
        /// If an error occurs, the error should be propogated up so the UI
        /// can smoothly inform the user.
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        IngredientDeleteError DeleteIngredient(Int64 ingredientId);


        /// <summary>
        /// Fetch a particular ingredient by its Id.
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns>An ingredient object</returns>
        Ingredient GetIngredientById(Int64 ingredientId);

        List<Ingredient> GetIngredientRange(Int64[] ingredientIds);

        /// <summary>
        /// Given a recipeId, queries all the ingredients in the database
        /// that are associated with that particular recipe. 
        /// If an error occurs, the error should be propogated up so the UI
        /// can smoothly inform the user.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        List<Ingredient> GetIngredientsInRecipe(Int64 recipeId);


        /// <summary>
        /// Fetches all ingredients from the database
        /// </summary>
        /// <returns>A list of Ingredient type</returns>
        List<Ingredient> GetAllIngredients();

        /// <summary>
        /// Checks for an existing Ingredient before attempting to add it.
        /// If the ingredient is found, then it will return that Ingredient.
        /// If it's not found, and the data is valid, then we add it to the db.
        /// </summary>
        /// <remarks>
        /// Note that the dynamic return type allows for IngredientAdditionErrors
        /// in the event that the data is invalid
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns>Either the Ingredent (existing/inserted) or a IngredientAdditionError</returns>
        dynamic GetOrCreateIngredient(string name, string quantity, string? unit);

    }
}
