using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;


namespace CookNook;

public partial class AddRecipeIngredientsPage : ContentPage
{
    private Random random = new Random();
    public AddRecipePage PreviousPageData { get; set; }

    public string Ingredients { get; set; } 

    // TODO: fix the hacky depencdency injection workaround
    private RecipeLogic recipeLogic = new RecipeLogic(new RecipeDatabase());

    public AddRecipeIngredientsPage()
    {
        InitializeComponent();
    }

    public async void NextClicked(object sender, EventArgs e)
    {
        //Ingredients = (this.FindByName("Ingredients") as Entry).Text;
        
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
    }
}
