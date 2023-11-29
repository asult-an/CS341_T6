using CookNook.Model;
using CookNook.Services;
using System.Diagnostics;

namespace CookNook;

public partial class Profile : ContentPage
{
    private IRecipeLogic recipeLogic;
    private User user;
    public User AppUser { get { return user; } set { user = value; } }
    
	public Profile()
	{
        recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));
        user = UserViewModel.Instance.AppUser;

        InitializeComponent();
        BindingContext = this;

        LoadRecipes(user.Id);
    }
    public Profile(User inUser)
    {
        recipeLogic = new RecipeLogic(new RecipeDatabase(), new IngredientLogic(new IngredientDatabase()));

        InitializeComponent();
        user = inUser;
        BindingContext = this;

        LoadRecipes(user.Id);
    }
    public async void SettingsClicked(object sender, EventArgs e)
	{
        Debug.WriteLine(user.Username);
        UserSettings userSettingsPage = new UserSettings();
        await Navigation.PushAsync(userSettingsPage);

    }
    public async void SearchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }
    public async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Recipe recipe)
        {
            // Navigate to the RecipePopUpPage with the selected recipe
            var popup = new RecipePopUpView(recipe, user);
            await Navigation.PushModalAsync(popup);
        }
    }

    private void LoadRecipes(long userID)
    {
        userRecipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes(userID);
    }
}