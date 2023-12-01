using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook.Controls;

/// <summary>
/// Defines a custom control that allows an 
/// </summary>
public partial class AutocompletePicker : ContentView, INotifyPropertyChanged
{
	/// <summary>
	/// We want 'ItemsSource' to appear on the XAML like it would a regular CollectionView
	/// </summary>
	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
		propertyName: "ItemsSource",
		returnType: typeof(IEnumerable<Ingredient>),
		declaringType: typeof(AutocompletePicker),
		defaultValue: default(IEnumerable<Ingredient>));
		// defaultBindingMode: BindingMode.TwoWay,
		// propertyChanged: OnItemsSourceChanged);

    public IEnumerable<Ingredient> ItemsSource
    {
        get { return (IEnumerable<Ingredient>)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    private IAutocompleteStrategy<Ingredient> autocompleteStrategy;
	private readonly List<Ingredient> ingredients;
    
	/// <summary>
	/// Placeholder for the chosen ingredient to be stored in before being sent out through events
	/// </summary>
	private Ingredient selectedIngredient;
	
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

	// public AutocompletePicker(IAutocompleteStrategy<Ingredient> autocompleteStrategy, List<Ingredient> ingredientList)
	public AutocompletePicker(IAutocompleteStrategy<Ingredient> autocompleteStrategy)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			InitializeComponent();
			this.autocompleteStrategy = autocompleteStrategy;
			Debug.WriteLine($"[AutocompletePicker] Verifying Strategy... {this.autocompleteStrategy})");
			Debug.WriteLineIf((this.autocompleteStrategy == null), "[AutocompletePicker] (ERROR): null strategy used!");

			this.BindingContext = this;
			Debug.WriteLine($"[AutocompletePicker] BindingContext updated: {this.BindingContext}");
		});

    }

    public AutocompletePicker() {
		MainThread.BeginInvokeOnMainThread(() =>
		{
			// didn't realize I needed this: ChatGPT pointed out the other wars the empty CTOR is used :O
			Debug.WriteLine("WARNING: Empty constructor used for AutocompletePicker!");
			this.BindingContext = this;
			Debug.WriteLine($"[AutocompletePicker] BindingContext updated: {this.BindingContext}");
		});
	}

	/// <summary>
	/// When the user types in the Entry, the string is captured as a query and sent to 
	/// AutocompleteStrategy's implementation to return a populated list of the results
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void IngredientEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		
		// TODO: To avoid overwhelming the system, we need some sort of cooldown or debouncing
		// if time since last update exceeds some threshold...


		string queryString = IngredientEntry.Text;

		if (autocompleteStrategy != null)
		{
			Debug.WriteLine($"[AutocompletePicker] Fetching results for {queryString}...");
			// on text changed...
			var newContents = await autocompleteStrategy.GetSuggestionsAsync(IngredientEntry.Text);
			Debug.WriteLine(newContents);

			Debug.WriteLine("[AutocompletePicker] Setting ItemsSource... ");
            // TODO: this needs to be returning type Ingredient, not string
            // IngredientPicker.ItemsSource = new ObservableCollection<Ingredient>(newContents);
            IngredientsCollectionView.ItemsSource = new ObservableCollection<Ingredient>(newContents);

        }
        else
			Debug.WriteLine("[AutocompletePicker] (ERROR) Stategy was null!");
    }

    /// <summary>
    /// sets the SelectedIndex of the IngredientsCollectionView back to -1
    /// </summary>
    public void ClearSelectedIngredient()
	{
        IngredientsCollectionView.SelectedItems = null;
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