
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

    public AddRecipeIngredientsPage(Recipe recipe)

    private Random random = new Random();
    public AddRecipePage PreviousPageData { get; set; }

    public string Ingredients { get; set; }

    private RecipeLogic recipeLogic = new RecipeLogic();

    

    {
        InitializeComponent();
        currentRecipe = recipe;
        currentRecipe.Ingredients = new ObservableCollection<string>();
        currentRecipe.IngredientsQty = new ObservableCollection<string>();
    }

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

    





        //Ingredients = (this.FindByName("Ingredients") as Entry).Text;
        
       var newRecipe = new Recipe(
           1,
           PreviousPageData.RecipeName,
           "Description",
           "SYSTEM",
           new System.Collections.ObjectModel.ObservableCollection<string>(),
           new System.Collections.ObjectModel.ObservableCollection<string>(),
           int.Parse(PreviousPageData.RecipeTimeToMake),
           "Course",
           0,
           0,
           "Image",
           new System.Collections.ObjectModel.ObservableCollection<string>(),
           new System.Collections.ObjectModel.ObservableCollection<string>()
       );

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

