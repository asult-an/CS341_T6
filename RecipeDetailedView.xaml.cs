namespace CookNook;
using CookNook.Model;
using System.ComponentModel;
using System.Diagnostics;

public partial class RecipeDetailedView : ContentPage, INotifyPropertyChanged
{
	private Recipe recipe;
	private string recipeName;
	private UserLogic userLogic = new UserLogic();
	private string author;
	private int numFollowers = -1; //TODO: WRITE LOGIC TO EXTRACT THIS VALUE
	private Tag[] tags;
	private byte[] image;
	private int rating;
	private CourseType course;
	private int cooktime;
	private int servings;
	private string description;
	private Ingredient[] ingredients;

	public string RecipeName { get { return recipeName; } }

    public string Author { get { return author; } }

    public int NumFollowers { get { return numFollowers; } }
    public Tag[] Tags { get { return tags; } }
    public byte[] Image { get { return image; } }
    public int Rating { get { return rating; } }
    public CourseType Course { get { return course; } }
    public int Cooktime { get { return cooktime; } }
    public int Servings { get { return servings; } }
    public string Description { get { return description; } }
    public Ingredient[] Ingredients { get { return ingredients; } }
    public RecipeDetailedView(Recipe inRecipe)
	{
		InitializeComponent();
		recipe = inRecipe;
        recipeName = inRecipe.Name;
        BindingContext = recipe;
        
		
		try
		{
			recipeName = recipe.Name;
            author = userLogic.GetUserById(recipe.AuthorID).Username;
			tags = recipe.Tags;
            image = recipe.Image;
            rating = recipe.Rating;
            course = recipe.Course;
            cooktime = recipe.CookTime;
            servings = recipe.Servings;
            description = recipe.Description;
            ingredients = recipe.Ingredients;
            //Debug.WriteLine(author);

        }
		catch (Exception ex)
		{
            Debug.WriteLine("IN AUTHOR");
            Debug.WriteLine(ex.Message);
			author = "ERROR";
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

	
}