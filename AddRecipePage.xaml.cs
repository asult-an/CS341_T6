using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using Microsoft.Maui.Storage;

namespace CookNook
{
    public partial class AddRecipePage : ContentPage
    {

        private string recipeName;
        private string recipeCooktime;
        private string recipeInstructions;
        private string Description;
        private int Servings;
        private IRecipeLogic recipeLogic;
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        public string RecipeInstructions { get { return recipeInstructions; } set { recipeInstructions = value; } }
        private String imagePath;

        // constructor with dependency injection
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
      

        private async void NextClicked(object sender, EventArgs e)
        {

            if (SelectedImage.Source is FileImageSource fileImageSource)
            {
                imagePath = fileImageSource.File;
            }
            else 
                imagePath = "NO_IMAGE";

            // Create a new Recipe and fill in first page fields
            var newRecipe = new Recipe
            {

                Name = Name.Text,
                //Description = Description.Text,
                CookTime = int.Parse(TimeToMake.Text),
                //Servings = int.Parse(Servings.Text),
                Image = Encoding.ASCII.GetBytes(imagePath)
            };

            // Navigate to the Addingredients page and pass the newRecipe object
            await Navigation.PushAsync(new AddRecipeIngredientsPage(this.recipeLogic, newRecipe));
        }

        private async void PickImageClicked(object sender, EventArgs e)
        {
            // Configure the picker to look for images
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                                                { 
                                                    //not sure if we only need to target android storage or others
                                                    { DevicePlatform.iOS, new[] { "public.image" } }, 
                                                    { DevicePlatform.Android, new[] { "image/*" } },
                                                    { DevicePlatform.WinUI, new[] { ".jpg", ".png" } },
                                                    { DevicePlatform.Tizen, new[] { "*/*" } },
                                                    { DevicePlatform.macOS, new[] { "public.image" } }, 
                                                });

            // Open the picker
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select an image",
                FileTypes = customFileType,
            });

            if (result != null)
            {
                // Load the picked image into the window
                var stream = await result.OpenReadAsync();
                ImageSource imageSource = ImageSource.FromStream(() => stream);
                SelectedImage.Source = imageSource;
            }

           
            // Capture user input
            RecipeName = (this.FindByName("Name") as Entry).Text;
            RecipeCooktime = (this.FindByName("Cooktime") as Entry).Text;
            RecipeInstructions = (this.FindByName("Instructions") as Editor).Text;
            var newRecipe = new Recipe
            {

                Name = recipeName,
                Description = recipeInstructions,
                //CookTime = int.Parse(TimeToMake.Text),
                CookTime = int.Parse(recipeCooktime),
                //Servings = int.Parse(Servings.),
                Image = Encoding.ASCII.GetBytes(imagePath)
            };
            //DisplayAlert("Error", "Test", "Okay");
            var nextPage = new AddRecipeIngredientsPage(recipeLogic, newRecipe);
            await Navigation.PushAsync(nextPage);

            //RecipeInstructions = (this.FindByName("Instructions") as Entry).Text;
            //DisplayAlert("Error", "Test", "Okay");
            //var nextPage = new AddRecipeIngredientsPage { PreviousPageData = this };
           // await Navigation.PushAsync(nextPage);

        }

    }

}
