namespace CookNook;
using Model;

public partial class RecipeDetailedView : ContentPage
{
	private Recipe recipe;
	private string recipeName;
	public string RecipeName { get { return recipeName; } }
	public RecipeDetailedView(Recipe inRecipe)
	{
		InitializeComponent();
		recipe = inRecipe;
        recipeName = inRecipe.Name;
	}

	
}