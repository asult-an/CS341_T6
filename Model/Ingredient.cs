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


		/// <summary>
		/// Constructs a new 'Unitless ingredient' which will have a `unit` 
		/// of NULL. Useful for ingredients that feature unique naming conventions, such 
		/// as 'eggs', 'cans', 'apple(s)', etc.
		/// 
		/// IngredientId Defaults to an ID of -1, so be sure to modify before inserting
		/// </summary>
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		public Ingredient(string name, string quantity)
		{
			// keeps Id at -1 so we can tell if it hasn't been fetched from the database after being added 
			this.ingredientId = -1;
			this.Name = name;
			this.quantity = quantity;
			this.unit = null;
		}

		/// <summary>
		/// Constructor for an ingredient that requires a unit and a quantity
		/// e.g '2 tbsp (of) Milk', '3.5 cups (of) Flour', etc
		/// </summary>
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		/// <param name="unit"></param>
		public Ingredient(string name, string quantity, string unit)
		{
			this.Name = name;
			this.quantity = quantity;
			this.unit = unit;
		}

        /// <summary>
        /// Full constructor for a 'unitless' ingredient, exposing the ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        public Ingredient(int id, string name, string quantity)
        {
            this.ingredientId = id;
            this.Name = name;
            this.quantity = quantity;
            this.unit = null;
        }

        /// <summary>
        /// Full constructor for an ingredient, exposing the ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        public Ingredient(int id, string name, string quantity, string unit)
        {
            this.ingredientId = id;
            this.Name = name;
            this.quantity = quantity;
            this.unit = unit;
        }


        /// <summary>
        /// Empty constructor for an ingredient, made solely to allow the more 
        /// readable bracketed object construction syntax
        /// </summary>
        public Ingredient(){}

        public static Ingredient ParseFromJson(string ingredientJSON)
        {
            
			// needs to be implemented

            throw new FormatException("Invalid CourseType!");
        }
	}
}
