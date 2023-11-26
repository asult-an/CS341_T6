using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CookNook.Model.Interfaces;
using CookNook.Services;

namespace CookNook;

public partial class Cookbook : ContentPage
{
    private User user;
    private IRecipeLogic recipeLogic;
    private ICookbookPageLogic cookbookPageLogic;

    public User AppUser { get { return user; } set { user = value; } }
    public string PageTitle { get { return user.Username + "'s Cookbook"; } }
    
    
    public Cookbook()
    {
        InitializeComponent();
        recipeLogic = new RecipeLogic(new RecipeDatabase());
        user = UserViewModel.Instance.AppUser;
        cookbookPageLogic = new CookbookPageLogic(new CookbookPageDatabase(user.Id), recipeLogic);

        this.BindingContext = this;
        // TODO: see if we still need LoadRecipes, or if we're calling it twice
        LoadRecipes(user.Id);
        
    }

    // Constructor with dependency injection (UNUSED)
    public Cookbook(IRecipeLogic recipeLogic, ICookbookPageLogic cookbookPageLogic)
    {
        InitializeComponent();
        this.recipeLogic = recipeLogic;
        this.cookbookPageLogic = cookbookPageLogic;
        LoadRecipes(user.Id);
    }

    private void LoadRecipes(long userID)
    {
        recipesCollectionView.ItemsSource = recipeLogic.CookBookRecipes(userID);

    }

    // Other methods
    // TODO: CookbookPagePickerSelection_Changed...
}
