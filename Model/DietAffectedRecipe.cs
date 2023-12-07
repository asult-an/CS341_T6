using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    /// <summary>
    /// Represents a recipe subjected to the constraints of a user's preference.
    /// To avoid decoupling or data de-synchronization, we only store the referenced recipe's ID
    /// </summary>
    public class DietAffectedRecipe
    {

        /// <summary>
        /// The Id of the recipe that the affected recipe is storing
        /// </summary>
        public long RecipeId { get; private set; }

        public bool IsPreferred { get; private set; }


        /// <summary>
        /// Constructs a new DietAffectedRecipe to represent a row from 'preferences_recipes'
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="isPreferred"></param>
        public DietAffectedRecipe(long recipeId, bool isPreferred)
        {
            RecipeId = recipeId;
            IsPreferred = isPreferred;
        }
    }
}
