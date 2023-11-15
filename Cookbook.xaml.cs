namespace CookNook;

public partial class Cookbook : ContentPage
{
    public Cookbook()
    {
        InitializeComponent();
    }

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Profile());
    }
}