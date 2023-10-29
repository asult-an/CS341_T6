using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook
{
    public partial class AddRecipePage : ContentPage
    {
        public string RecipeName { get; set; }
        public string RecipeTimeToMake { get; set; }

        public AddRecipePage()
        {
            InitializeComponent();
        }

        public async void NextClicked(object sender, EventArgs e)
        {
            // Capture user input
            RecipeName = (this.FindByName("Name") as Entry).Text;
            RecipeTimeToMake = (this.FindByName("TimeToMake") as Entry).Text;

            var nextPage = new AddRecipeIngredientsPage { PreviousPageData = this };
            await Navigation.PushAsync(nextPage);
        }
    }

}
