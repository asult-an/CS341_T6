
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


    public string Ingredients { get; set; }

    // TODO: consider using Ingredient model instead of String
    //public string Ingredients => Ingredients;

    private IRecipeLogic recipeLogic;

    
    public AddRecipeIngredientsPage(IRecipeLogic recipeLogic, Recipe recipe)
    {
        InitializeComponent();
        currentRecipe = recipe;
        currentRecipe.Ingredients = new List<Ingredient>().ToArray();
        //currentRecipe.IngredientsQty = new List<string>();
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
        //currentRecipe.Ingredients.Add(ingredient);

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
                new Ingredient("Pie Crust", "1"),
                //TODO new ingredients fail
                // and a regular one
                //new Ingredient("Artichoke Hearts", "2", "oz")
                new Ingredient("Apple (Red Delicious)", "2")
        };
        Tag[] tags = { new Tag { DisplayName = "test" } };

        var newRecipe = new Recipe(
            currentRecipe.Name,                              // name
            currentRecipe.Description,            // description
            currentRecipe.CookTime,       // cooktime 
            testIngredients,          //recipeLogic.GetIngredientsByRecipe(1),
            CourseType.Parse("Dinner"),
            //CourseType.Parse(CourseEntry.Text),
            -1,
            4,
            1,
            tags,             // recipeLogic.GetTagsForRecipe
            new long[] {1}       // followerIds
           
        ); 
            //recipeLogic.GetFollowerIds()

        // TODO: map ingredients and their quantities...?
        if (recipeLogic == null)
        {
            recipeLogic = new RecipeLogic(new RecipeDatabase());
        }

        // Add recipe to the database using RecipeLogic
        /**
         * TEMPORARY
         * 
         * [0:] Error finding recipe: System.InvalidOperationException: No row is available
            at Npgsql.NpgsqlDataReader.GetFieldValue[Int32](Int32 ordinal)
            at Npgsql.NpgsqlDataReader.GetInt32(Int32 ordinal)
            at CookNook.Model.RecipeDatabase.SelectRecipe(Int32 inID) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeDatabase.cs:line 502
            at CookNook.Model.RecipeLogic.FindRecipe(Int32 id) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeLogic.cs:line 148
            [0:] System.InvalidOperationException: Parameter 'Description' must have either its NpgsqlDbType or its DataTypeName or its Value set
            at Npgsql.NpgsqlParameter.ResolveHandler(TypeMapper typeMapper)
            at Npgsql.NpgsqlParameter.Bind(TypeMapper typeMapper)
            at Npgsql.NpgsqlParameterCollection.ProcessParameters(TypeMapper typeMapper, Boolean validateValues, CommandType commandType)
            at Npgsql.NpgsqlCommand.ExecuteReader(CommandBehavior behavior, Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteReader(CommandBehavior behavior, Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteScalar(Boolean async, CancellationToken cancellationToken)
            at Npgsql.NpgsqlCommand.ExecuteScalar()
            at CookNook.Model.RecipeDatabase.InsertRecipe(Recipe inRecipe) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeDatabase.cs:line 350
            at CookNook.Model.RecipeLogic.AddRecipe(Recipe recipe) in C:\Users\staff.morriv92\source\repos\CS341\flavorflave\FlavorFlaveProto\ProtoFiles\Model\RecipeLogic.cs:line 87
         */
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

    /// <summary>
    /// Sends the recipe back to the previous page so the entered 
    /// data is not lost during navigation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();

        var newRecipe = new Recipe(
            (int)random.NextInt64(5000),
            currentRecipe.Name,
            "Description",
            currentRecipe.CookTime,
            new Ingredient[] { },
            CourseType.Parse(CourseEntry.Text),
            1,
            0,
            0,
            new Tag[] { },
            new long[] { },
            //Encoding.ASCII.GetBytes(PreviousPageData.ImagePath)
            Encoding.ASCII.GetBytes("NO_IMAGE")
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

