using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        cookbookPageLogic = new CookbookPageLogic(new CookbookPageDatabase());
        user = UserViewModel.Instance.AppUser;
        BindingContext = this;
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
}
