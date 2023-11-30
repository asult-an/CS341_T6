using System.Collections.ObjectModel;
using System.ComponentModel;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook.Controls;

/// <summary>
/// Defines a custom control that allows an 
/// </summary>
public partial class AutocompletePicker : ContentView, INotifyPropertyChanged
{
	//private IAutocompleteStrategy<Ingredient> autocompleteStrategy;

	//public IAutocompleteStrategy<Ingredient> 
		private IAutocompleteStrategy<Ingredient> autocompleteStrategy;
	private Ingredient selectedIngredient;
	private readonly List<Ingredient> ingredients;

    /// <summary>
    /// Whenever the user selects an ingredient, we need to let other important classes know
	/// So we define a handler to "broadcast" when that happens
    /// </summary>
    public event EventHandler<IngredientSelectedEventArgs> IngredientSelected;


    public IAutocompleteStrategy<Ingredient> AutocompleteStrategy
	{
		get { return autocompleteStrategy; }
		set { autocompleteStrategy = value; }
	}

	public Ingredient SelectedIngredient
	{
		get { return selectedIngredient; }
		set { selectedIngredient = value; }
	}

	public AutocompletePicker(IAutocompleteStrategy<Ingredient> autocompleteStrategy, List<Ingredient> ingredientList)
	{
		InitializeComponent();
		this.autocompleteStrategy = autocompleteStrategy;
	}

	public AutocompletePicker() { }

	/// <summary>
	/// Fires when the user types text, provided the system isn't under too much load
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void IngredientEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		// TODO: To avoid overwhelming the system, we need some sort of cooldown or debouncing

		// on text changed...
		var newContents = await autocompleteStrategy.GetSuggestionsAsync(IngredientEntry.Text);

		// TODO: this needs to be returning type Ingredient, not string
		IngredientPicker.ItemsSource = new ObservableCollection<Ingredient>(newContents);
    }

	/// <summary>
	/// sets the SelectedIndex of the IngredientPicker back to -1
	/// </summary>
	public void ClearSelectedIngredient()
	{
		IngredientPicker.SelectedIndex = -1;
	}

    /// <summary>
    /// Capture the selection the user makes, so we can send it as part of the event that 
    /// AddRecipeIngredientsPage is listening to
    /// </summary>
    /// <param name="ingredient"></param>
    public void OnIngredientSelected(Ingredient ingredient)
    {
        // fire the event handler, if it's not null, so that the subscribed page hears it
        IngredientSelected?.Invoke(this,
            new IngredientSelectedEventArgs(ingredient));
    }
}