using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.Services;

namespace CookNook;

public partial class AddRecipeIngredientsPage : ContentPage, INotifyPropertyChanged
{
    // popup needs a "home"
    private IngredientPickerPopup currentIngredientPickerPopup;
    
    // public Recipe currentRecipe;
    private Recipe currentRecipe;
    private readonly IAutocompleteStrategy<Ingredient> ingredientStrategy;
    private readonly IRecipeLogic recipeLogic;
    private readonly IUserLogic userLogic;
    private readonly IIngredientLogic ingredientLogic;
    private IEnumerable<Ingredient> ingredientList; 
    private Ingredient selectedIngredient;
    private User user;

    /// <summary>
    /// The command that the button binds to for opening the IngredientPicker popup
    /// </summary>
    public ICommand OpenPickerCommand { get; private set; }


    /// <summary>
    /// public-facing accessor of the currentRecipe, so that the UI can bind to it
    /// </summary>
    public Recipe CurrentRecipe
    {
        get { return currentRecipe; }
        set { currentRecipe = value; }
    }

    /// <summary>
    /// The list of ingredients the user will chooser from, exposed as a property
    /// </summary>
    public IEnumerable<Ingredient> IngredientList
    {
        get => ingredientList;
        set
        {
            ingredientList = value;
            OnPropertyChanged(nameof(IngredientList));
        }
    }

    public string Ingredients { get; set; }

    public AddRecipeIngredientsPage(IUserLogic userLogic, IRecipeLogic recipeLogic, IIngredientLogic ingredientLogic, Recipe recipe, User inUser)
    {
        InitializeComponent();
        
        // define navigable route to the IngredientPicker popup
        OpenPickerCommand = new Command(OpenPickerPopup);

        if (recipe == null)
        {
            Console.WriteLine("AddRecipeIngredientsPage: recipe is null!");
            CurrentRecipe = new Recipe();
        }
        else
        {
            CurrentRecipe = recipe;
        }
        CurrentRecipe.Ingredients = new ObservableCollection<Ingredient>();
        user = inUser;
        this.ingredientLogic = ingredientLogic;
        this.recipeLogic = recipeLogic;
        this.userLogic = userLogic;

        // set the IngredientPicker's bindables
        
        IngredientList = ingredientLogic.GetAllIngredients();
        // autocompletion - first we need to prime the strategy with a collection of Ingredients*
        this.ingredientStrategy = new IngredientAutocompleteStrategy(IngredientList);
        
        // now we can hook it into the 
        // IngredientPicker.AutocompleteStrategy = ingredientStrategy;

        this.BindingContext = this;
    }

    /// <summary>
    /// Injects recipeLogic, then populates the list with ingredients to add to their recipe
    /// </summary>
    /// <param name="recipeLogic"></param>
    /// <param name="ingredientLogic"></param>
    public AddRecipeIngredientsPage(IRecipeLogic recipeLogic, IIngredientLogic ingredientLogic)
    {
        InitializeComponent();
         
        //this.recipeLogic = recipeLogic
        // use the recipeLogic scoped from Dependency Injection
        this.recipeLogic = recipeLogic;
        this.ingredientLogic = ingredientLogic;

        IngredientList = ingredientLogic.GetAllIngredients();
        this.BindingContext = this;
    }

    // /** ==============================  [ START POPUP FUNCTIONS ] ============================== 

    /// <summary>
    /// Activated by clicking on the button, the command calls this method 
    /// to instantiate a new IngredientPickerPopup and sends the user unto its focus
    /// </summary>
    private async void OpenPickerPopup()
    {
        // convert IngredientList into an IEnumerable
        // var allIngredients = await ingredientLogic.GetAllIngredients();
        var allIngredients = ingredientLogic.GetAllIngredients();

        // pass the ingredients into the custom picker
        currentIngredientPickerPopup = new IngredientPickerPopup(allIngredients);

        //await Application.Current.MainPage.Navigation.PushModalAsync(ingredientPickerPopup);

        // show the popup
        Debug.WriteLine($"[AddRecipeIngredientPage] Opening IngredientPickerPopup ({currentIngredientPickerPopup})");
        Debug.WriteLine($"[AddRecipeIngredientPage] Verifying Strategy... {currentIngredientPickerPopup.AutocompletePickerControl.AutocompleteStrategy})");
        Debug.WriteLine($"[AddRecipeIngredientPage] Verifying ItemsSource... {currentIngredientPickerPopup.AutocompletePickerControl.ItemsSource})");

        await this.ShowPopupAsync(currentIngredientPickerPopup);
        // this.ShowPopup(ingredientPickerPopup);
    }

    /// <summary>
    /// Wrapper method to close the popup when the user is done selecting an ingredient
    /// </summary>
    /// <param name="popup"></param>
    private async void ClosePickerPopup(Popup popup)
    {
        await popup.CloseAsync();
        popup = null;
    }

    /// <summary>
    /// Handles the IngredientSelectedEvent, by taking the incoming ingredient
    /// and displaying it on the page as the selected ingredient
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnIngredientSelected(object sender, IngredientSelectedEventArgs e)
    {
        // store the chosen ingredient so it can be added after the user is happy with other fields
        this.selectedIngredient = e.SelectedIngredient;

        // set the currently displayed item of the recipe ingredients page
        if (selectedIngredient != null)
            btnIngredientPicker.Text = selectedIngredient.Name;
        else 
            btnIngredientPicker.Text = "Select Ingredient";

        // Unsubscribe from the event before closing
        currentIngredientPickerPopup.IngredientSelectedEvent -= OnIngredientSelected;


        // send the popup back here to get closed and deconstructed 
        // if (sender is Popup popup)
        if (sender is IngredientPickerPopup popup)
        {
            ClosePickerPopup(popup);
        }
    }
     // ==============================  [ END POPUP FUNCTIONS ] ============================== */


    private void AddIngredientClicked(object sender, EventArgs e)
    {
        try
        {
            // check our SelectedIngredient property...

            // Ingredient selectedIngredient = IngredientPicker.SelectedItem as Ingredient;
            // Ingredient selectedIngredient = IngredientPicker.SelectedIngredient;
            
            string ingredientName = selectedIngredient.Name;
            long ingredientId = selectedIngredient.IngredientId;

            int quantity = int.Parse(QuantityEntry.Text);

            // if null, no unit, otherwise grab the selected unit
            string unit = (UnitPicker.SelectedItem == null) ? "" : selectedIngredient.Unit;

            // concatenation
            string displayQuantity = $"{quantity} {unit}";
            
            // if the data was able to be parse, structure into a new Ingredient obj according to which values are null
            if(string.IsNullOrEmpty(unit))
            {
                // grab the ingredients ALREADY on the currentRecipe, after invoking an Add on the latest ingredient
                CurrentRecipe.Ingredients.Add(new Ingredient(ingredientId, ingredientName, quantity.ToString()));

                //ingredients = currentRecipe.Ingredients.Append(new Ingredient(ingredientName, QuantityEntry.Text)).ToList();
            }
            else // if we have a valid unit:
            {
                // grab the ingredients ALREADY on the currentRecipe, after invoking an Add on the latest ingredient
                CurrentRecipe.Ingredients.Add(new Ingredient(ingredientId, ingredientName, quantity.ToString(), unit));
                //ingredients = currentRecipe.Ingredients.Append(new Ingredient(ingredientName, QuantityEntry.Text, unit)).ToList();
            }

        }
        catch (Exception ex)
        {
            Debug.Write("AddIngredientsPage: " + ex.Message);
            DisplayAlert("Error", "Ingredient Add Failed", "Okay");
        }
        finally
        {
            //clear entries and picker
            // IngredientPicker.ClearSelectedIngredient();
            // IngredientEntry.Text = string.Empty;

            QuantityEntry.Text = string.Empty;
            UnitPicker.SelectedIndex = -1;
        }
    }

    
    public async void NextClicked(object sender, EventArgs e)
    {
        //Ingredients = (this.FindByName("Ingredients") as Entry).Text;
        // TODO: finish TAGS
        Tag[] tags = { new Tag { DisplayName = "test" } };
        
        // assemble the recipe now that we have all the information we need to fully construct it
        var newRecipe = new Recipe(
            CurrentRecipe.Name,                   // name
            CurrentRecipe.Description,            // description
            CurrentRecipe.CookTime,               // cooktime 
            CurrentRecipe.Ingredients,                      //recipeLogic.GetIngredientsByRecipe(1),
            // CourseType.Parse("Dinner"),
            CourseType.Parse(CourseEntry.Text),
            CurrentRecipe.AuthorID,
            4,                             // TODO: rating
            1,                            // TODO: servings
            tags,                                 // TODO: recipeLogic.GetTagsForRecipe
            new long[] { }             // followerIds
            //recipeLogic.GetFollowerIds()

        ); 

        // TODO: map ingredients and their quantities...?

        // "compound assignment" operator: only triggers if left side of operator is null, assigns right side's value
        // recipeLogic ??= new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        // recipeLogic ??= MauiProgram.ServiceProvider.GetService<RecipeLogic>();

        var result = recipeLogic.AddRecipe(newRecipe);

        // Check if the recipe was added successfully and navigate accordingly
        switch (result)
        {
            case RecipeAdditionError.NoError:
                await Navigation.PushAsync(new DietaryRestrictionsPage(user));
                await DisplayAlert("Success", "Recipe added successfully!", "OK");
                break;
            case RecipeAdditionError.DBAdditionError:
                await DisplayAlert("Error", "LogicError", "OK");
                break;
            default:
                await DisplayAlert("Error", "Failed to add recipe", "OK");
                break;
        }
    }


    //REVISE, some properties may be incorrect
    /// <summary>
    /// Sends the recipe back to the previous page so the entered 
    /// data is not lost during navigation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();

        var newRecipe = new Recipe(
            0,
            currentRecipe.Name,
            currentRecipe.Description,
            currentRecipe.CookTime,
            currentRecipe.Ingredients,
            CourseType.Parse(CourseEntry.Text),
            0,     //TODO: get the author-id from the user
            0,
            0,
            new Tag[] { },
            new long[] { },
            //Encoding.ASCII.GetBytes(PreviousPageData.ImagePath)
            Encoding.ASCII.GetBytes("NO_IMAGE")
        );

    }

    /// <summary>
    /// Broadcasts an event whenever the IngredientList property is changed
    /// </summary>
    public new event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Invokes the PropertyChanged event
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // update the Ingredient box on the page

    }
}