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

        private String imagePath;
        public AddRecipePage()
        {
            InitializeComponent();
        }

        private async void NextClicked(object sender, EventArgs e)
        {
            if (SelectedImage.Source is FileImageSource fileImageSource)
            {
                imagePath = fileImageSource.File;
            }

            // Create a new Recipe and fill in first page fields
            var newRecipe = new Recipe
            {

                Name = Name.Text,
                Description = Description.Text,
                CookTime = int.Parse(TimeToMake.Text),
                Servings = int.Parse(Servings.Text),
                Image = imagePath
            };

            // Navigate to the Addingredients page and pass the newRecipe object
            await Navigation.PushAsync(new AddRecipeIngredientsPage(newRecipe));
        }

        private async void PickImageClicked(object sender, EventArgs e)
        {
            // Configure the picker to look for images
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
    { //not sure if we only need to target android storage or others
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
        }

    }

}
