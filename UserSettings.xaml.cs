using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.XMLHelpers;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace CookNook;

public partial class UserSettings : ContentPage, INotifyPropertyChanged
{
    private IUserLogic userLogic;
    private User user;
    public event PropertyChangedEventHandler PropertyChanged;
    private ImageSource userImage;
    private byte[] imageBytes;
    public ImageSource UserImage
    {
        get => userImage;
        set
        {
            if (userImage != value)
            {
                userImage = value;
                OnPropertyChanged(nameof(UserImage));
            }
        }
    }
    public UserSettings()
	{
        user = UserViewModel.Instance.AppUser;
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        InitializeComponent();
        BindingContext = this;
        LoadProfilePic(user);
    }

    public UserSettings(User inUser)
    {
        userLogic = MauiProgram.ServiceProvider.GetService<IUserLogic>();
        InitializeComponent();
        user = inUser;
        BindingContext = this;
        LoadProfilePic(user);
    }

    public async void UserAccountSettingsClicked(object sender, EventArgs e)
	{
		AccountSettings accountSettings = new AccountSettings();
		await Navigation.PushAsync(accountSettings);
	}

    public async void DietaryRestrictionsClicked(object sender, EventArgs e)
    {
        DietaryRestrictionsPage dietaryRestrictionsPage = new DietaryRestrictionsPage(user);
        await Navigation.PushAsync(dietaryRestrictionsPage);
    }

    public async void LogOutClicked(object sender, EventArgs e)
    {
        //TODO: Debug this
        Navigation.PopToRootAsync();
        await Navigation.PopAsync();
    }

    private void LoadProfilePic(User user)
    {
        byte[] userPic = userLogic.GetProfilePic(user);
        if (userPic != null)
        {
            var imageConverter = new ByteToImageConverter();
            UserImage = (ImageSource)imageConverter.Convert(userPic, null, null, null);
        }
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnChangePictureClicked(object sender, EventArgs e)
    {
        pictureOptions.IsVisible = !pictureOptions.IsVisible;
    }

    private async void OnTakePictureClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                UserImage = ImageSource.FromStream(() => stream);
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            Debug.WriteLine("Feature Not supported");
        }
        catch (PermissionException pEx)
        {
            Debug.WriteLine("Permission not granted");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error Occurred");
        }
    }

    private async void OnSelectFromStorageClicked(object sender, EventArgs e)
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
                UserImage = imageSource;
                userLogic.SetProfilePic(user, imageBytes);
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