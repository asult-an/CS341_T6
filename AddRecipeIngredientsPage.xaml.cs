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

    private void CompleteRecipeClicked(object sender, EventArgs e)
    {
        
        

        
    }
}

