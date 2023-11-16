
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;


namespace CookNook;

public partial class AddRecipeIngredientsPage : ContentPage
{

    private Recipe currentRecipe;


    private Random random = new Random();

    /// <summary>
    /// The list of ingredients the user will chooser from, exposed as a property
    /// </summary>
    public static IEnumerable<Ingredient> IngredientList { get; set; }  //  => IngredientList;


    public AddRecipePage PreviousPageData { get; set; }

    public string Ingredients { get; set; }

    // TODO: consider using Ingredient model instead of String
    public string Ingredients => Ingredients;

    private IRecipeLogic recipeLogic;

    
    public AddRecipeIngredientsPage(Recipe recipe)
    {
        InitializeComponent();
        currentRecipe = recipe;
        currentRecipe.Ingredients = new ObservableCollection<string>();
        currentRecipe.IngredientsQty = new ObservableCollection<string>();
    }

    /// <summary>
    /// Injects recipeLogic, then populates the list with ingredients to add to their recipe
    /// </summary>
    /// <param name="recipeLogic"></param>
    public AddRecipeIngredientsPage(IRecipeLogic recipeLogic)
    {
        InitializeComponent();
        this.recipeLogic = recipeLogic;
        IngredientList = recipeLogic.GetAllIngredients();
    }

    // W
    private void AddIngredientClicked(object sender, EventArgs e)
    {

        string ingredient = IngredientEntry.Text;
        int quantity = int.Parse(QuantityEntry.Text);
        string unit = UnitPicker.SelectedItem.ToString();


        string displayQuantity = $"{quantity} {unit}";

        // Add to the ObservableCollection
        currentRecipe.Ingredients.Add(ingredient);
        currentRecipe.IngredientsQty.Add(displayQuantity);

        //clear entries and picker
        IngredientEntry.Text = string.Empty;
        QuantityEntry.Text = string.Empty;
        UnitPicker.SelectedIndex = -1;
    }

    




    public async void NextClicked(object sender, EventArgs e)
    {
        //Ingredients = (this.FindByName("Ingredients") as Entry).Text;

        //Ingredient testIngredients = new Ingredient("test", 1, "test");
        Ingredient[] testIngredients = new Ingredient[]
        {
                // one 'unitless' ingredient
                new Ingredient("Industrial Runoff", "1"),
                
                // and a regular one
                new Ingredient("Artichoke Hearts", "2", "oz")
        };
        Tag[] tags = { new Tag { DisplayName = "test" } };

        var newRecipe = new Recipe(
            PreviousPageData.RecipeName,                              // name
            PreviousPageData.RecipeInstructions,            // description
            int.Parse(PreviousPageData.RecipeCooktime),       // cooktime 
            testIngredients,          //recipeLogic.GetIngredientsByRecipe(1),
            CourseType.Parse("DINNER"),
            -1,
            4,
            1,
            tags,             // recipeLogic.GetTagsForRecipe
            new int[] {1}       // followerIds
           
        ); 
            //recipeLogic.GetFollowerIds()

        // TODO: map ingredients and their quantities...?

       // Add recipe to the database using RecipeLogic
       var result = recipeLogic.AddRecipe(newRecipe);

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

       
    }


    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
        
        var newRecipe = new Recipe(
            (int)random.NextInt64(5000),
            PreviousPageData.RecipeName,
            "Description",
            0,
            "",
            "",
            int.Parse(PreviousPageData.RecipeCooktime),
            "Course",
            0,
            0,
            "Image",
            "",
            ""
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
}

