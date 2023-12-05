using CookNook.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;

namespace CookNook;

public partial class IngredientPickerPage : ContentPage
{
    public AutocompletePicker AutocompletePickerControl { get; private set; }
    private const int DEFAULT_WIDTH = 300;
    private const int DEFAULT_HEIGHT = 400;

    /// <summary>
    /// Listens to the custom control's own event, and fires an event to tell the RecipeIngredientPage to close me
    /// </summary>
    public event EventHandler<IngredientSelectedEventArgs> IngredientSelectedEvent;

    public IngredientPickerPage(IEnumerable<Ingredient> choices)
    {
        try
        {
            InitializeComponent();
            // The AutocompletePicker could have its own XAML layout loaded here
            AutocompletePickerControl = this.FindByName<AutocompletePicker>("IngredientPicker");


            AutocompletePickerControl.Loaded += (sender, args) =>
            {
                Debug.WriteLineIf((AutocompletePickerControl == null), "(ERROR) [IngredientPickerPopup] " +
                                                                       "Could not find the custom Picker control!");
                // since ingredients come from the strategy, we initalize the strategy first
                AutocompletePickerControl.AutocompleteStrategy = new IngredientAutocompleteStrategy(choices);
                Debug.WriteLine($"[IngredientPickerPopup] Strategy set with data: {AutocompletePickerControl.AutocompleteStrategy}");
                // wait for the control to load before setting any properties
                AutocompletePickerControl.ItemsSource = choices;
                Debug.WriteLine($"[IngredientPickerPopup] ItemsSource: {AutocompletePickerControl.ItemsSource}");

            };

            // user can tap outside modal to close, but selecting ingredient should close it

            // if we want to set the size...
            this.DesiredSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);

            Content = AutocompletePickerControl;


            // Subscribe to the IngredientSelectedEvent, so we can inform the AddRecipeIngredientPage
            AutocompletePickerControl.IngredientSelected += (sender, args) =>
            {
                // if the event wasn't null, we'll send it over to the page
                IngredientSelectedEvent?.Invoke(this, args);
            };
            //{
            //    IngredientSelectedEvent?.Invoke(this, new IngredientSelectedEventArgs(selctedIngredient));
            //    // Optionally close the popup

            //});
        }
        catch (Exception ex)
        {
            // log to the console
            Console.WriteLine("Outer: " + ex.Message);
            Console.WriteLine("Inner: " + ex.InnerException.Message);
            throw ex.InnerException;
        }
    }
        /// <summary>
        /// Capture the selection the user makes, so we can send it as part of the event that 
        /// AddRecipeIngredientsPage is listening to
        /// </summary>
        /// <param name="ingredient"></param>
        public void OnIngredientSelected(Ingredient ingredient)
        {
            // fire the event handler, if it's not null, so that the subscribed page hears it
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
    public Ingredient SelectedIngredient { get; private set; }
    public IngredientSelectedEventArgs(Ingredient selectedIngredient)
    {
        SelectedIngredient = selectedIngredient;
    }
}