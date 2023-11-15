using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook
{
    public partial class AddRecipePage : ContentPage
    {
        private string recipeName;
        private string recipeCooktime;
        private string recipeInstructions;
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        public string RecipeInstructions { get { return recipeInstructions; } set { recipeInstructions = value; } }

        public AddRecipePage()
        {
            InitializeComponent();
        }

        public async void NextClicked(object sender, EventArgs e)
        {
            
            // Capture user input
            RecipeName = (this.FindByName("Name") as Entry).Text;
            RecipeCooktime = (this.FindByName("Cooktime") as Entry).Text;
            
            //RecipeInstructions = (this.FindByName("Instructions") as Entry).Text;
            //DisplayAlert("Error", "Test", "Okay");
            var nextPage = new AddRecipeIngredientsPage { PreviousPageData = this };
            await Navigation.PushAsync(nextPage);
        }
    }

}
