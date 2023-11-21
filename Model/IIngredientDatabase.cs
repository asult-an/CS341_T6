using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IIngredientDatabase
    {
        /// <summary>
        /// returns all ingredients in the database
        /// 
        /// </summary>
        /// <returns></returns>
        List<Ingredient> GetAllIngredients();


        /// <summary>
        /// Fetch a particular ingredient by its name.
        /// </summary>
        /// <param name="name">Ingredient name.</param>
        /// <returns>Ingredient object if found, null otherwise.</returns>
        Ingredient GetIngredientByName(string name);
        
        /// <summary>
        /// Fetch all ingredients that belong to a particular recipe
        /// </summary>
        /// <param name="recipeId">recipe id</param>
        /// <returns>List of Ingredient object if found, null otherwise</returns>
        List<Ingredient> GetIngredientsFromRecipe(Int64 recipeId);
        
 
        /// <summary>
        /// Fetch a particular ingredient by its id.
        /// </summary>
        /// <param name="id">Ingredient id.</param>
        /// <returns>Ingredient object if found, null otherwise.</returns>
        Ingredient GetIngredientById(Int64 id);

        /// <summary>
        /// Updates an ingredient by its id
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        IngredientUpdateError UpdateIngredient(Int64 ingredientId, string name, string quantity, string? unit);

        /// <summary>
        /// Inserts a new ingredient into the database
        /// </summary>
        /// <param name="ingredient">the new inredient to be added</param>
        /// <returns></returns>
        IngredientAdditionError CreateIngredient(Ingredient ingredient);
        
        /// <summary>
        /// SImilar to CreateIngredient, except in cases where a duplicate ingredient is 
        /// inserted, the existing ingredient is instead returned
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        IngredientAdditionError GetOrCreateIngredient(Ingredient ingredient);


        /// <summary>
        /// Remove an ingredient by its ingredientId
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        IngredientDeleteError RemoveIngredient(Int64 ingredientId);

    }
}
