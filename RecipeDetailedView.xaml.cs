using CookNook.Model.Interfaces;
using CookNook.XMLHelpers;

namespace CookNook;
using CookNook.Model;
using CookNook.Services;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage
{
    private Recipe recipe;
    private User user;
    private IRecipeLogic recipeLogic;
    public User User
    {
        get { return user; }
        set { user = value; }
    }

    public Recipe Recipe
    {
        get { return recipe; }
        set { recipe = value; }
    }


    private IUserLogic userLogic;

    public RecipeDetailedView(Recipe inRecipe, User user)
	{
		InitializeComponent();
        try
        {
		    recipe = inRecipe;
            //BindingContext = this;
            BindingContext = recipe;

            Debug.WriteLine($"Viewing recipe {inRecipe.ID} by {recipe.AuthorID}");
            
            //userLogic = new UserLogic(new Model.Services.UserDatabase(), new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase())));
            userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
            User = userLogic.GetUserById(recipe.AuthorID);

            // since the IngredientSelector needs to be accessible for determining which DataTemplate we use for each Ingredient...
            var templateSelector = new IngredientTemplateSelector(Resources);

            // ...we define it here
            Resources["IngredientTemplateSelector"] = templateSelector;

            // bind the label for the username to the user
            AuthorName.BindingContext = User;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[RecipeDetailedView] (ERROR!) {ex.InnerException.Message}");
        }

        //try
        //{
        //    //Failing because FollowerIds is set to Null still?
        //    numFollowers = recipe.FollowerIds.Length;
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine("IN NUMFOLLOWERS");
        //    Debug.WriteLine(ex.Message);
        //}

    }
    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


    /// <summary>
    /// simple checker to see if the user should see the "Missing ingredients" message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnIngredientsLoaded(object sender, EventArgs e)
    {
        // check if the recipe's ingredient list is empty
        if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
            MissingIngredientsLabel.IsVisible = true;
        else 
            MissingIngredientsLabel.IsVisible = false;

    }

    private void ShowRatingOverlay()
    {
        RatingOverlay.IsVisible = true;
    }

    private void HideRatingOverlay()
    {
        RatingOverlay.IsVisible = false;
    }

    private void RateButton_Clicked(object sender, EventArgs e)
    {
        ShowRatingOverlay();
    }

    private void SubmitRating(object sender, EventArgs e)
    {
        var selectedRating = (int)Math.Round(RatingSlider.Value);
        recipeLogic = MauiProgram.ServiceProvider.GetService<IRecipeLogic>();
        recipeLogic.AddRating(selectedRating,recipe.ID);
        int rating = recipeLogic.GetRating(recipe.ID);
        recipe.Rating = rating;
        HideRatingOverlay();
    }

    private void CancelRating(object sender, EventArgs e)
    {
        HideRatingOverlay();
    }

    private void RatingSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        
        SliderValueLabel.Text = $"Rating: {Math.Round(e.NewValue)}";
    }

}