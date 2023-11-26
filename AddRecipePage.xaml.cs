using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.Services;
using Microsoft.Maui.Storage;

namespace CookNook
{
    public partial class AddRecipePage : ContentPage
    {
        private string recipeName;
        private string recipeCooktime;
        private string recipeInstructions;
<<<<<<< HEAD
<<<<<<< HEAD

        //private string Description
        private int Servings;
=======
        // private string description
        // private int servings;
>>>>>>> 1bcdde6282e19ce11b60af23d0378a028a943f87
=======
        //private string Description
        private int Servings;
>>>>>>> parent of 1bcdde6 (fix: removed duplicate Description property)
        private IRecipeLogic recipeLogic;
        private IIngredientLogic ingredientLogic;

        // TODO: PreviousPageData uses these three.  Are we grabbing these?
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        public string RecipeInstructions { get { return recipeInstructions; } set { recipeInstructions = value; } }
        private string imagePath;
        private byte[] imageBytes;
        private User user;

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
            user = UserViewModel.Instance.AppUser;
        }


        private async void NextClicked(object sender, EventArgs e)
        {
            // warn the user if they try to enter a recipe with missing datum
<<<<<<< HEAD
<<<<<<< HEAD
            if (Name.Text == null || TimeToMake.Text == null || RecipeDescription.Text == null)
=======
            // TODO: write better sanity checks
            //if (Name.Text == null || TimeToMake.Text == null || recipeInstructions == null)
            if (Name.Text == null || TimeToMake.Text == null || Description.Text == null)
>>>>>>> 1bcdde6282e19ce11b60af23d0378a028a943f87
=======
            if (Name.Text == null || TimeToMake.Text == null || recipeInstructions == null)
>>>>>>> parent of 1bcdde6 (fix: removed duplicate Description property)
            {
                await DisplayAlert("Error", "Please fill out all fields", "Okay");
                return;
            }

            try
            {
                var newRecipe = new Recipe
                {
                    Name = Name.Text,
                    CookTime = int.Parse(TimeToMake.Text),
                    Description = RecipeDescription.Text,
                    Image = imageBytes,                 // Use the byte array of the selected image
                    AuthorID = user.Id
                };

                // Navigate to the Addingredients page and pass the newRecipe object
                await Navigation.PushAsync(new AddRecipeIngredientsPage(this.recipeLogic, this.ingredientLogic, newRecipe, user));
            }
            catch (Exception ex)
            {
                Debug.Write("AddRecipePage: " + ex.Message);
                DisplayAlert("Error", "Recipe Add Failed", "Okay");
            }
            
        }

        private async void PickImageClicked(object sender, EventArgs e)
        {
            var pickOptions = new PickOptions
            {
                PickerTitle = "Please select an image",
                // Use predefined file types
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.PickAsync(pickOptions);

            try
            {
                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            imageBytes = memoryStream.ToArray(); // Store the image data as a byte array
                        }
                    }

                    ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    SelectedImage.Source = imageSource;
                }
                else
                    Debug.WriteLine("No file picked");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


    }

}
