using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using CookNook.Controls;
using CookNook.Model;

namespace CookNook;

public class IngredientPickerPopup : Popup
{
    public AutocompletePicker AutocompletePickerControl { get; private set; }
    private const int DEFAULT_WIDTH = 300;
    private const int DEFAULT_HEIGHT = 400;

    public event EventHandler<IngredientSelectedEventArgs> IngredientSelectedEvent;

    public IngredientPickerPopup()
    {
        // The AutocompletePicker could have its own XAML layout loaded here
        AutocompletePickerControl = new AutocompletePicker();

        // TODO: means of exiting: close button, gesture,
        // whatever works, just something hacky for now


        // if we want to set the size...
        this.Size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);

        this.Content = AutocompletePickerControl;

        //autocompletePicker.SelectedIngredient += new Action<object, object>((sender, selectedIngredient) =>
        //{
        //    // Raise an event when an ingredient is selected
        //    IngredientSelected?.Invoke(this, new IngredientSelectedEventArgs(selectedIngredient));
        //    // Optionally close the popup
        //    this.Close();
        //});
    }


    /// <summary>
    /// Capture the selection the user makes, so we can send it as part of the event that 
    /// AddRecipeIngredientsPage is listening to
    /// </summary>
    /// <param name="ingredient"></param>
    public void OnIngredientSelected(Ingredient ingredient)
    {
        // fire the event handler, if it's not null
        IngredientSelectedEvent?.Invoke(this,
                                        new IngredientSelectedEventArgs(ingredient));
    }
}

/// <summary>
/// Very simple class, just defines the details of what happens when
/// the user selects an ingredient on the popup: since it has to be 
/// raised to send the information out as Event Arguments
/// </summary>
public class IngredientSelectedEventArgs : EventArgs
{
    /// <summary>
    /// The ingredient that was selected
    /// </summary>
    public Ingredient SelectedIngredient { get; }
    public IngredientSelectedEventArgs(Ingredient selectedIngredient)
    {
        SelectedIngredient = selectedIngredient;
    }
}