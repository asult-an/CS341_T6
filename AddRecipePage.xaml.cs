using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;

namespace CookNook
{
    public partial class AddRecipePage : ContentPage
    {
        private string recipeName;
        private string recipeCooktime;
        private string recipeInstructions;
        private IRecipeLogic recipeLogic;
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        public string RecipeInstructions { get { return recipeInstructions; } set { recipeInstructions = value; } }

        public AddRecipePage(IRecipeLogic recipeLogic)
        {
            InitializeComponent();
            this.recipeLogic = recipeLogic; 
        }

        public AddRecipePage()
        {
            InitializeComponent();
            this.recipeLogic = new RecipeLogic(new RecipeDatabase());
        }

        public async void NextClicked(object sender, EventArgs e)
        {
            
            // Capture user input
            RecipeName = (this.FindByName("Name") as Entry).Text;
            RecipeCooktime = (this.FindByName("Cooktime") as Entry).Text;
            RecipeInstructions = (this.FindByName("Instructions") as Editor).Text;

            //DisplayAlert("Error", "Test", "Okay");
            var nextPage = new AddRecipeIngredientsPage(recipeLogic) { PreviousPageData = this };
            await Navigation.PushAsync(nextPage);
        }
    }

}
