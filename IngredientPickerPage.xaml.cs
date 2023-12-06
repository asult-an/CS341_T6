using CookNook.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IngredientPickerPage : ContentPage
{
    public AutocompletePicker AutocompletePickerControl { get; private set; }
    private const int DEFAULT_WIDTH = 300;
    private const int DEFAULT_HEIGHT = 400;

    /// <summary>
    /// Listens to the custom control's own event, and fires an event on close that contains
    /// all selected ingredients in the EventArgs
    /// </summary>
    public event EventHandler<SelectionConfirmedEventArgs> SelectionConfirmedEvent;

    public IngredientPickerPage(IEnumerable<Ingredient> choices)
    {
        try
        {
            InitializeComponent();
            // The AutocompletePicker could have its own XAML layout loaded here
            
            AutocompletePickerControl = this.FindByName<AutocompletePicker>("IngredientPicker");

            if (AutocompletePickerControl == null)
            {
                Debug.WriteLine("[IngredientPickerPage] AutocompletePickerControl not found.");
            }
            else
            {
                AutocompletePickerControl.AutocompleteStrategy = new IngredientAutocompleteStrategy(choices);
                Debug.WriteLine("[IngredientPickerPage] AutocompleteStrategy set.");

                AutocompletePickerControl.ItemsSource = choices; // For debugging, set a static list
                Debug.WriteLine("[IngredientPickerPage] ItemsSource set.");
            }


            if (AutocompletePickerControl == null)
            {
                Debug.WriteLine("[IngredientPickerPage] Error: AutocompletePickerControl is null.");
                return; // Abort if control is null
            }


            // user can tap outside modal to close, but selecting ingredient should close it

            // if we want to set the size...
            // this.DesiredSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);


            // Subscribe to the SelectionConfirmedEvent, so we can inform the AddRecipeIngredientPage
            AutocompletePickerControl.SelectionConfirmedHandler += (sender, args) =>
            {
                // if the event wasn't null, we'll send it over to the page
                SelectionConfirmedEvent?.Invoke(this, args);
            };
            //{
            //    SelectionConfirmedEvent?.Invoke(this, new SelectionConfirmedEventArgs(selctedIngredient));
            //    // Optionally close the popup

            //});
        }
        catch (Exception ex)
        {
            // log to the console
            Debug.WriteLine($"[IngredientPickerPage] (ERROR!) Construction error: {ex.InnerException ?? ex}");
            throw ex.InnerException ?? ex;
        }
    }


    /// <summary>
    /// Capture the selection the user makes, so we can send it as part of the event that 
    /// AddRecipeIngredientsPage is listening to
    /// </summary>
    /// <param name="ingredient"></param>
    public void OnIngredientSelected(IEnumerable<Ingredient> ingredients)
    {
        // fire the event handler, if it's not null, so that the subscribed page hears it
        SelectionConfirmedEvent?.Invoke(this,
            new SelectionConfirmedEventArgs(ingredients));
    }

    public delegate void SelectionConfirmedHandler(IEnumerable<Ingredient> selectedIngredients);

}



/// <summary>
/// Very simple class, just defines the details of what happens when
/// the user selects an ingredient on the popup: since it has to be 
/// raised to send the information out as Event Arguments
/// </summary>
public class SelectionConfirmedEventArgs : EventArgs
{
    /// <summary>
    /// The ingredient(s) that was selected
    /// </summary>
    public IEnumerable<Ingredient> SelectedIngredients { get; private set; }
    public SelectionConfirmedEventArgs(IEnumerable<Ingredient> selectedIngredients)
    {
        SelectedIngredients = selectedIngredients;
    }
}