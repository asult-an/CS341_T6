using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    /// <summary>
    /// Represents an ingredient subjected to the constraints of a user's preference.
    /// To avoid decoupling or data de-synchronization, we only store the referenced ingredient's ID
    /// </summary>
    public class DietAffectedIngredient
    {

        /// <summary>
        /// The Id of the recipe that the affected ingredient is storing
        /// </summary>
        public long IngredientId { get; private set; }

        public bool IsPreferred { get; private set; }


        /// <summary>
        /// Constructs a new DietAffectedRecipe to represent a row from 'preferences_recipes'
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="isPreferred"></param>
        public DietAffectedIngredient(long ingredientId, bool isPreferred)
        {
            IngredientId = ingredientId;
            IsPreferred = isPreferred;
        }
    }
}
