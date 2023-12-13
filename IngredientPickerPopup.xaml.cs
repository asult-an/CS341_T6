//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CommunityToolkit.Maui.Views;
//using Microsoft.Maui.Controls;
//using CookNook.Controls;
//using CookNook.Model;
//using System.ComponentModel;
//using System.Diagnostics;

//namespace CookNook;

//public sealed partial class IngredientPickerPopup : Popup
//{
//    public AutocompletePicker AutocompletePickerControl { get; private set; }
//    private const int DEFAULT_WIDTH = 300;
//    private const int DEFAULT_HEIGHT = 400;

//    /// <summary>
//    /// Listens to the custom control's own event, and fires an event to tell the RecipeIngredientPage to close me
//    /// </summary>
//    public event EventHandler<IngredientSelectionEventArgs> SelectionConfirmedEventHandler;

   
//    //public IngredientPickerPopup(List<Ingredient> choices)
//    public IngredientPickerPopup(IEnumerable<Ingredient> choices)
//    {
//        try
//        {

//            InitializeComponent();
//            // The AutocompletePicker could have its own XAML layout loaded here
//            AutocompletePickerControl = this.FindByName<AutocompletePicker>("IngredientPicker");


//            AutocompletePickerControl.Loaded += (sender, args) =>
//            {
//                Debug.WriteLineIf((AutocompletePickerControl == null), "(ERROR) [IngredientPickerPopup] " +
//                    "Could not find the custom Picker control!");
//                // since ingredients come from the strategy, we initalize the strategy first
//                AutocompletePickerControl.AutocompleteStrategy = new IngredientAutocompleteStrategy(choices);
//                Debug.WriteLine($"[IngredientPickerPopup] Strategy set with data: {AutocompletePickerControl.AutocompleteStrategy}");
//                // wait for the control to load before setting any properties
//                AutocompletePickerControl.ItemsSource = choices;
//                Debug.WriteLine($"[IngredientPickerPopup] ItemsSource: {AutocompletePickerControl.ItemsSource}");

//            };

//            // user can tap outside modal to close, but selecting ingredient should close it

//            // if we want to set the size...
//            this.Size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);

//            Content = AutocompletePickerControl;
        
        
//            // Subscribe to the SelectionConfirmedEventHandler, so we can inform the AddRecipeIngredientPage
//            AutocompletePickerControl.SelectionConfirmedHandler += (sender, args) =>
//            {
//                // if the event wasn't null, we'll send it over to the page
//                SelectionConfirmedEventHandler?.Invoke(this, args);
//            };
//            //{
//            //    SelectionConfirmedEventHandler?.Invoke(this, new IngredientSelectionEventArgs(selctedIngredient));
//            //    // Optionally close the popup
            
//            //});
//        }
//        catch (Exception ex)
//        {
//            // log to the console
//            Console.WriteLine(ex.Message);
//            Console.WriteLine(ex.InnerException.Message);
//            throw ex.InnerException;
//        }
//    }



//    /// <summary>
//    /// Capture the selection the user makes, so we can send it as part of the event that 
//    /// AddRecipeIngredientsPage is listening to
//    /// </summary>
//    /// <param name="ingredient"></param>
//    public void OnIngredientSelected(Ingredient ingredient)
//    {
//        // fire the event handler, if it's not null, so that the subscribed page hears it
//        SelectionConfirmedEventHandler?.Invoke(this,
//                                        new IngredientSelectionEventArgs(ingredient));
//    }
//}

///// <summary>
///// Very simple class, just defines the details of what happens when
///// the user selects an ingredient on the popup: since it has to be 
///// raised to send the information out as Event Arguments
///// </summary>
//public class IngredientSelectionEventArgs : EventArgs
//{
//    /// <summary>
//    /// The ingredient that was selected
//    /// </summary>
//    public Ingredient SelectedIngredient { get; private set;  }
//    public IngredientSelectionEventArgs(Ingredient selectedIngredient)
//    {
//        SelectedIngredient = selectedIngredient;
//    }
//}