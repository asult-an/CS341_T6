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
    private Popup currentIngredientPickerPopup;
    
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

    // TODO: consider using Ingredient model instead of String
    //public string Ingredients => Ingredients;



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

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    InitializeCommands();
    //}

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
    /// Instantiates a new IngredientPickerPopup and sends the user unto its focus
    /// </summary>
    private async void OpenPickerPopup()
    {
        currentIngredientPickerPopup = new IngredientPickerPopup();
        // var ingredientPickerPopup = new IngredientPickerPopup();

        //await Application.Current.MainPage.Navigation.PushModalAsync(ingredientPickerPopup);

        // show the popup
        this.ShowPopup(currentIngredientPickerPopup);
        // this.ShowPopup(ingredientPickerPopup);
    }

    /// <summary>
    /// Wrapper method to close the popup when the user is done selecting an ingredient
    /// </summary>
    /// <param name="popup"></param>
    private async void ClosePickerPopup(Popup popup)
    {
        //if (this.CurrentPopup is IngredientPickerPopup currentPopup)
        //if (popup != null)
        //{
        //    // goodbye popup! (close the lil guy)
        //    currentIngredientPickerPopup.Close();
        //}
        CommunityToolkit.Maui.Core.PopupExtensions.ClosePopup(popup);
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

        // send the popup back here to get closed and deconstructed 
        if (sender is Popup popup)
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

        //Ingredient testIngredients = new Ingredient("test", 1, "test");
        //Ingredient[] testIngredients = new Ingredient[]
        //{
        //        // one 'unitless' ingredient
        //        new Ingredient("Pie Crust", "1"),
        //        //TODO new ingredients fail
        //        // and a regular one

        //        //new Ingredient("Artichoke Hearts", "2", "oz")
        //        new Ingredient("Apple (Red Delicious)", "2")
        //};

        Tag[] tags = { new Tag { DisplayName = "test" } };

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

        // Add recipe to the database using RecipeLogic
        /**
         * TEMPORARY
         * 
         * [0:] Error finding recipe: System.InvalidOperationException: No row is available
            at Npgsql.NpgsqlDataReader.GetFieldValue[Int32](Int32 ordinal)
            at Npgsql.NpgsqlDataReader.GetInt32(Int32 ordinal)
            at CookNook.Model.RecipeDatabase.SelectRecipe(Int32 inID) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeDatabase.cs:line 502
            at CookNook.Model.RecipeLogic.FindRecipe(Int32 id) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeLogic.cs:line 148
            [0:] System.InvalidOperationException: Parameter 'Description' must have either its NpgsqlDbType or its DataTypeName or its Value set
            at Npgsql.NpgsqlParameter.ResolveHandler(TypeMapper typeMapper)
            at Npgsql.NpgsqlParameter.Bind(TypeMapper typeMapper)
            at Npgsql.NpgsqlParameterCollection.ProcessParameters(TypeMapper typeMapper, Boolean validateValues, CommandType commandType)
            at Npgsql.NpgsqlCommand.ExecuteReader(CommandBehavior behavior, Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteReader(CommandBehavior behavior, Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteScalar(Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteScalar()
            at CookNook.Model.RecipeDatabase.InsertRecipe(Recipe inRecipe) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeDatabase.cs:line 350
            at CookNook.Model.RecipeLogic.AddRecipe(Recipe recipe) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeLogic.cs:line 87
         */
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
        /**
       // Add recipe to the database using RecipeLogic
       var result = recipeLogic.AddRecipe(newRecipe);
       //DisplayAlert("Debug", result.ToString(), "Okay");
       // Check if the recipe was added successfully and navigate accordingly
       if (result == RecipeAdditionError.NoError)
       {
           await Navigation.PushAsync(new DietaryRestrictionsPage());
           await DisplayAlert("Success", "Recipe added successfully!", "OK");
       }
       else if (result == RecipeAdditionError.DBAdditionError)
       {
           await DisplayAlert("Error", "LogicError", "OK");
       }
       else
       {
           await DisplayAlert("Error", "Failed to add recipe", "OK");
       }
       */

    }

    /// <summary>
    /// Broadcasts an event whenever the IngredientList property is changed
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Invokes the PropertyChanged event
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // update the Ingredient box on the page

    }

    private void btnIngredientPicker_Clicked(object sender, EventArgs e)
    {
        TODO_IMPLEMENT_ME();
    }
}

