using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private User user;
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        public string RecipeInstructions { get { return recipeInstructions; } set { recipeInstructions = value; } }
        private string imagePath;
        private byte[] imageBytes;
        public User PageUser { get { return user; } set { user = value; } }

        // constructor with dependency injection
        public AddRecipePage(IRecipeLogic recipeLogic, User inUser)
        {
            InitializeComponent();
            this.recipeLogic = recipeLogic;
        }

        public AddRecipePage(User inUser)
        {
            InitializeComponent();
            this.recipeLogic = new RecipeLogic(new RecipeDatabase());
            user = inUser;
        }
        public AddRecipePage()
        {
            InitializeComponent();
            this.recipeLogic = new RecipeLogic(new RecipeDatabase());
        }


        private async void NextClicked(object sender, EventArgs e)
        {

            var newRecipe = new Recipe
            {
                Name = Name.Text,
                CookTime = int.Parse(TimeToMake.Text),
                Description = recipeInstructions,
                Image = imageBytes // Use the byte array of the selected image
            };

            // Navigate to the Addingredients page and pass the newRecipe object
            await Navigation.PushAsync(new AddRecipeIngredientsPage(this.recipeLogic, newRecipe, user));
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
