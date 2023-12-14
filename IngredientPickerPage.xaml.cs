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
    public event EventHandler<IngredientSelectionEventArgs> SelectionConfirmedEventHandler;

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

            // Subscribe our event logic to the event handler, so we can inform the AddRecipeIngredientPage
            AutocompletePickerControl.SelectionConfirmedHandler += AutocompletePickerControl_SelectionConfirmed;

        }
        catch (Exception ex)
        {
            // log to the console
            Debug.WriteLine($"[IngredientPickerPage] (ERROR!) Construction error: {ex.InnerException ?? ex}");
            throw ex.InnerException ?? ex;
        }
    }


    /// <summary>
    /// Function to initiate event logic during SelectionConfirmed events received from the 
    /// AutocompletePickerControl, so that we can pass the ingredients back to the AddRecipeIngredientPage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void AutocompletePickerControl_SelectionConfirmed(object sender, IngredientSelectionEventArgs e)
    {
       
        Debug.WriteLine($"[IngredientPickerPage] Event received with {e.SelectedIngredients.Count()} ingredients.");        

        // fire event 
        SelectionConfirmedEventHandler?.Invoke(this, e);
        Debug.WriteLine($"[IngredientPickerPage] EventHandler invoked");

        // close the page now that we've fired the event
        await Navigation.PopModalAsync();
       
        //if (Navigation.ModalStack.Contains(this))
        // check if this page is actually on the navigation modal stack
        //{
        //    Debug.WriteLine($"[IngredientPickerPage] Page is on the modal stack, attempting to close.");
        //    await Navigation.PopModalAsync();
        //}
        //else
        //{
        //    Debug.WriteLine($"[IngredientPickerPage] Page is NOT on the modal stack, cannot close.");
        //}
    }

}



/// <summary>
/// Very simple class, just defines the details of what happens when
/// the user selects an ingredient on the popup: since it has to be 
/// raised to send the information out as Event Arguments
/// </summary>
public class IngredientSelectionEventArgs : EventArgs
{
    /// <summary>
    /// The ingredient(s) that was selected
    /// </summary>
    public IEnumerable<Ingredient> SelectedIngredients { get; private set; }
    public IngredientSelectionEventArgs(IEnumerable<Ingredient> selectedIngredients)
    {
        SelectedIngredients = selectedIngredients;
    }
}