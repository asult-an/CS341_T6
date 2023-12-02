using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;

namespace CookNook.XMLHelpers
{
    /// <summary>
    /// This class handles the programmatic decision of which DataTemplate to use to display a 
    /// given Ingredient on a page (most likely in some sort of CollectionView). 
    /// 
    /// Ingredients will require few templates for now, but what DOES need to be handled is:
    /// - Ingredients that would trigger an allergic reaction, per the user's dietary preferences
    /// - "Unitless Ingredients" which do not need their unit field to be displayed 
    /// - Standard Ingredient
    /// </summary>
    public class IngredientTemplateSelector : DataTemplateSelector
    {
        // DataTemplate repertoire 
        public DataTemplate unitlessIngredientTemplate { get
            {
                // grab the DataTemplate already defined from the XAML
                return (DataTemplate)Application.Current.Resources["UnitlessIngredientTemplate"];
            }
        }

        public DataTemplate standardIngredientTemplate { get
            {
                // grab the DataTemplate already defined from the XAML
                return (DataTemplate)Application.Current.Resources["IngredientTemplate"];
            }
        }

        // public DataTemplate likedIngredientTemplate { get; set; }
        // public DataTemplate dislikedIngredientTemplate { get; set; }
        // public DataTemplate allergenIngredientTemplate { get; set; }

        /// <summary>
        /// Checks information about the incoming item, and uses the analysis to determine
        /// which DataTemplate makes the most sense to return.
        /// </summary>
        /// <param name="item">The incoming Ingredient that will be checked</param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var ingredient = item as Ingredient;

            if (ingredient == null)
                return null;

            
            // check if the item is unitless (unit is null)
            if (string.IsNullOrEmpty(ingredient.Unit))
            {
                return unitlessIngredientTemplate;
            }
            else
            {
                // if the item is NOT unitless, then we can use the standard ingredient DataTemplate
                return standardIngredientTemplate;
            }
            Debug.WriteLine("[IngredientTemplateSelector] (ERROR)! No template was selected!");
            return new DataTemplate();
        }
    }
}
