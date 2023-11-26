using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class AddRecipePage : ContentPage, INotifyPropertyChanged
    {
        private string recipeName;
        private string recipeCooktime;
        private string recipeInstructions;
        // private string description
        // private int servings;
        private IRecipeLogic recipeLogic;
        private IIngredientLogic ingredientLogic;
        private string imagePath;
        private byte[] imageBytes;
        private User user;

        public event PropertyChangedEventHandler PropertyChanged;

        // TODO: PreviousPageData uses these three.  Are we grabbing these?
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }
        public string RecipeCooktime { get { return recipeCooktime; } set { recipeCooktime = value; } }
        
        /// <summary>
        /// The instructions for the recipe, bound to the Description inpue in the XAML
        /// </summary>
        public string RecipeInstructions { 
            get { return recipeInstructions; } 
            set { 
                // if not null, set the value
                if(recipeInstructions != value)
                {
                    recipeInstructions = value; 
                    OnPropertyChanged(nameof(RecipeInstructions));
                }
            } 
        }


        // constructor with dependency injection
        public AddRecipePage(IRecipeLogic recipeLogic, IIngredientLogic ingredientLogic)
        {
            InitializeComponent();
            this.recipeLogic = recipeLogic;
            this.ingredientLogic = ingredientLogic;
            user = UserViewModel.Instance.AppUser;
        }

        public AddRecipePage()
        {
            InitializeComponent();
            this.recipeLogic = new RecipeLogic(new RecipeDatabase());
            this.ingredientLogic = new IngredientLogic(new IngredientDatabase());
            user = UserViewModel.Instance.AppUser;
        }


        private async void NextClicked(object sender, EventArgs e)
        {
            // warn the user if they try to enter a recipe with missing datum
            // TODO: write better sanity checks
            //if (Name.Text == null || TimeToMake.Text == null || recipeInstructions == null)
            if (Name.Text == null || TimeToMake.Text == null || Description.Text == null)
            {
                await DisplayAlert("Error", "Please fill out all fields", "Okay");
                return;
            }
            recipeInstructions = Description.Text;

            try
            {
                var newRecipe = new Recipe
                {
                    Name = Name.Text,
                    CookTime = int.Parse(TimeToMake.Text),
                    Description = recipeInstructions,
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
