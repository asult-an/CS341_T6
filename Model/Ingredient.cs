using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
	/// <summary>
	/// Represents an ingredient.  
	/// Note that it allows null quantity and unit , as some ingredients may not have a quantity or unit.
	/// </summary>
    public class Ingredient
    {
		private int ingredientId;
		private string name;
		private string quantity;
		private string? unit;

		/// <summary>
		/// The unit of the ingredient, such as "cup" or "teaspoon"
		/// May be null when name of ingredient is used as unit (e.g: "eggs")
		/// </summary>
		public string Unit
		{
			get { 
				// if there's no unit, either return name of ingredient or N/A
				if (unit == null)
				{
                    if (name == null)
					{
                        return "N/A";
                    }
                    return name;
                }
				return unit; 
			}
			set { unit = value; }
		}


		/// <summary>
		/// The quantity of the ingredient, such as 2 or 1/2.
		/// Stored as a string in case the user enters a fraction
		/// </summary>
		public string Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}


		/// <summary>
		/// Returns the name of the ingredient
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}


		public int IngredientId
		{
			get { return ingredientId; }
			set { ingredientId = value; }
		}


		public Ingredient(string name, string quantity)
		{
			// keeps Id at -1 so we can tell if it hasn't been fetched from the database after being added 
			this.ingredientId = -1;
			this.Name = name;
			this.quantity = quantity;
		}

		public Ingredient(string name, string quantity, )

	}
}
