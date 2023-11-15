namespace CookNook;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

}