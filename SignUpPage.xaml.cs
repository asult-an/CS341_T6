namespace CookNook;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
