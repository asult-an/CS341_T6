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

    public void LogOutClicked(object sender, EventArgs e)
    {
        UserViewModel.Instance.AppUser = null;
        Navigation.PushModalAsync(new WelcomePage());
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

    private async void OnChangePictureClicked(object sender, EventArgs e)
    {
        var pickOptions = new PickOptions
        {
            PickerTitle = "Please select an image",
            FileTypes = FilePickerFileType.Images // Use predefined file types
        };

        var result = await FilePicker.PickAsync(pickOptions);
        MemoryStream memoryStream = null; // Declare MemoryStream outside the try block

        try
        {
            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray(); // Store the image data as a byte array
                }

                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                UserImage = imageSource;
                userLogic.SetProfilePic(user, imageBytes);
            }
            else
            {
                Debug.WriteLine("No file picked");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error occurred: {ex}");
            memoryStream?.Dispose(); // Dispose of MemoryStream in case of an exception
            throw; // Re-throw the exception if you want to maintain the original exception handling behavior
        }
        finally
        {
            memoryStream?.Dispose(); // Ensure the MemoryStream is also disposed in the normal flow
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

    }




    public void ApplyDarkTheme(object sender, EventArgs e)
    {
        App.Current.UserAppTheme = AppTheme.Dark;
    }

    public void ApplyLightTheme(object sender, EventArgs e)
    {
        App.Current.UserAppTheme = AppTheme.Light;
    }


}