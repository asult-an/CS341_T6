using System.Diagnostics;
using CookNook.Model.Interfaces;

namespace CookNook.Model;
public class IngredientLogic : IIngredientLogic
{
    private IIngredientDatabase ingredientDatabase;
    const int MAX_INGREDIENT_NAME_LENGTH = 50;

    /// <summary>
    /// Constructs a new IngredientLogic object injecting an IIngredientDatabase.
    /// </summary>
    /// <param name="ingredientDatabase"></param>
    public IngredientLogic(IIngredientDatabase ingredientDatabase)
    {
        this.ingredientDatabase = ingredientDatabase;
    }

    /// <summary>
    /// Gets an ingredient by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Ingredient GetIngredientByName(string name)
    {
        var isSaneOperation = PerformSanityChecks(name);
        // abort if error
        if (isSaneOperation != IngredientAdditionError.NoError)
        {
            Debug.Write($"Error getting ingredient {name}!");
            // return IngredientSelectionError.IngredientNotFound;
            return null;
        }

        return ingredientDatabase.GetIngredientByName(name);
        // return IngredientSelectionError.NoError;
    }


    /// <summary>
    /// Performs sanity checks for adding an ingredient to the database.
    /// Checks include existing ingredient, and empty name.
    /// </summary>
    /// <param name="name">Name of the ingredient to check for</param>
    /// <returns>Type of IngredientAdditionError based on result of checks</returns>
    private IngredientAdditionError PerformSanityChecks(string name)
    {
        // existing ingredient 
        if (ingredientDatabase.GetIngredientByName(name) != null)
            return IngredientAdditionError.IngredientAlreadyExists;

        // empty name
        if (string.IsNullOrEmpty(name))
            return IngredientAdditionError.DBAdditionError;
        
        // name too long
        if (name.Length > MAX_INGREDIENT_NAME_LENGTH)
        {
            Debug.Write($"Ingredient name {name} is too long!");
            return IngredientAdditionError.BadParameters;
        }

        // empty units are allowed, unless being added to a recipe
        IngredientAdditionError error = ingredientDatabase.CreateIngredient(name);
        switch (error)
        {
            case IngredientAdditionError.DBAdditionError:
                return IngredientAdditionError.DBAdditionError;
                break;

            case IngredientAdditionError.IngredientAlreadyExists:
                return IngredientAdditionError.IngredientAlreadyExists;
                break;
            default:
                return IngredientAdditionError.NoError;
        }
    }


    /// <summary>
    /// Takes a name and creates an ingredient in the database.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IngredientAdditionError CreateIngredient(string name)
    {
        // sanity checks:   
        return PerformSanityChecks(name);
    }

    /// <summary>
    /// Create an ingredient for a recipe, with a quantity and unit
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="name"></param>
    /// <param name="unit"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    //public IngredientAdditionError CreateIngredientForRecipe(Int64 recipeId, string name, string? unit, string quantity)
    //{
    //    // sanity checks
    //    //var isSaneOperation = PerformSanityChecks(name);
    //    // only the empty sanity check is needed here, so we'll hard-code it:
    //    if (string.IsNullOrEmpty(name))
    //    {
    //        Debug.Write($"Invalid ingredient name {name} !");
    //        return IngredientAdditionError.DBAdditionError;
    //    }

    //    if (string.isNullOrEmpty(quantity))
    //    {
    //        // if there was a problem, we'll return the error directly
    //        Debug.Write($"Error creating ingredient { name } on recipe { recipeId }!");
    //        return isSaneOperation;
    //    }

    //    // first, we'll need to make sure the ingredient exists
    //    var ingredient = ingredientDatabase.GetIngredientByName(name);

    //    // if we get here, we know the ingredient doesn't exist, so we can create it
    //    return ingredientDatabase.AddIngredientToRecipe(recipeId);
    //}

    /// <summary>
    /// Updates an ingredient's name
    /// </summary>
    /// <param name="ingredient"></param>
    /// <returns></returns>
    public IngredientUpdateError UpdateIngredient(Ingredient ingredient)
    {
        var isSaneOperation = PerformSanityChecks(ingredient.Name);
        if (isSaneOperation != IngredientAdditionError.NoError)
        {
            // if there was a problem, we'll return the error directly
            Debug.Write($"Error updating ingredient {ingredient.Name}!");
            return IngredientUpdateError.DBUpdateError;
        }
        return ingredientDatabase.UpdateIngredient(ingredient.IngredientId, ingredient.Name);
    }

    /// <summary>
    /// WARNING: No safety checks in place
    /// </summary>
    /// <param name="ingredientId"></param>
    /// <returns></returns>
    public IngredientDeleteError DeleteIngredient(long ingredientId)
    {
        // TODO: check if ID exists before inserting
        if (ingredientId < 0)
        {
            Debug.Write($"Invalid ingredient_id {ingredientId} used on DELETE statement!");
            return IngredientDeleteError.DBDeletionError;
        }
        return ingredientDatabase.RemoveIngredient(ingredientId);
    }


    public Ingredient GetIngredientById(long ingredientId)
    {
        if (ingredientId < 0)
        {
            Debug.Write($"Invalid ingredient_id {ingredientId} used on DELETE statement!");
            return null;
        }

        return ingredientDatabase.GetIngredientById(ingredientId);
    }

    //public List<Ingredient> GetIngredientRange(long[] ingredientIds)
    //{
    //    // make sure no negative ids exist in the array
    //    if(ingredientIds.Any(x => x < 0))
    //    {
    //        Debug.Write($"Invalid ingredient_id {ingredientIds} used on DELETE statement!");
    //        return null;
    //    }
    //    return ingredientDatabase.GetIngredientRange(ingredientIds);
    //}

    /// <summary>
    /// Returns a list of ingredients present in a recipe
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public List<Ingredient> GetIngredientsInRecipe(long recipeId)
    {
        if (recipeId < 0)
        {
            Debug.Write($"Invalid recipe_id {recipeId} used on DELETE statement!");
            return null;
        }
        // we don't need an observable collection if it's not being used on the UI
        // TODO: ask if this is unecessary
        return new List<Ingredient>(ingredientDatabase.GetIngredientsFromRecipe(recipeId));
    }

    /// <summary>
    /// Gets all ingredients for the user to select from: i.e when choosing recipes, autocomplete
    /// </summary>
    /// <returns></returns>
    public List<Ingredient> GetAllIngredients()
    {
        return ingredientDatabase.GetAllIngredients();
    }
}