using System.Collections.ObjectModel;
using System.ComponentModel;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook;

/// <summary>
/// Defines a custom control that allows an 
/// </summary>
public partial class AutocompletePicker : ContentView, INotifyPropertyChanged
{
	private IAutocompleteStrategy<Ingredient> strategy;

	private Ingredient selectedIngredient;
	private readonly List<Ingredient> ingredients;

	public Ingredient SelectedIngredient
	{
		get { return selectedIngredient; }
		set { selectedIngredient = value; }
	}


	public AutocompletePicker(IAutocompleteStrategy<Ingredient> strategy, List<Ingredient> ingredientList)
	{
		InitializeComponent();
		this.strategy = strategy;
	}


	/// <summary>
	/// Fires when the user types text, provided the system isn't under too much load
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void IngredientEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		// TODO: To avoid overwhelming the system, we need some sort of cooldown or debouncing

		// on text changed...
		var newContents = await strategy.GetSuggestionsAsync(IngredientEntry.Text);

		// TODO: this needs to be returning type Ingredient, not string
		IngredientPicker.ItemsSource = new ObservableCollection<Ingredient>(newContents);
    }
}