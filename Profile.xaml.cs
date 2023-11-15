namespace CookNook;

public partial class Profile : ContentPage
{
    public Profile()
    {
        InitializeComponent();
    }

    public async void SettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserSettings());
    }
}