using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.XMLHelpers;
using System.ComponentModel;

namespace CookNook;

public partial class UserSettings : ContentPage, INotifyPropertyChanged
{
    private IUserLogic userLogic;
    private User user;
    public event PropertyChangedEventHandler PropertyChanged;
    private ImageSource userImage;
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
            // Feature not supported on device
        }
        catch (PermissionException pEx)
        {
            // Permissions not granted
        }
        catch (Exception ex)
        {
            // Other error has occurred.
        }
    }

    private void OnSelectFromStorageClicked(object sender, EventArgs e)
    {
        
    }
}