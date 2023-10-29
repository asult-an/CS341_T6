using CookNook.Model;
using System.Collections.ObjectModel;

namespace CookNook;

public partial class WelcomePage : ContentPage
{
    //TEST DATABASE INSERTS
    /*
    Database TestDB = new Database();
    static ObservableCollection<String> ingredients = new ObservableCollection<String>();
    
    static ObservableCollection<String> ingredientsQty = new ObservableCollection<String>();
    static ObservableCollection<String> tags = new ObservableCollection<String>();
    static ObservableCollection<String> followers = new ObservableCollection<String>();
    Recipe TestRecipe = new Recipe(55, "The First Recipe!", "This is the first recipe inserted into the CookNook database!", 1, ingredients, ingredientsQty, 60, "Dinner", 50, 6, "image_ref", tags, followers);
    */
	public WelcomePage()
	{
		InitializeComponent();
	}

	public async void LoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    public async void SignUpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    public async void SkipClicked(object sender, EventArgs e)
    {
        //TestDB.InsertRecipe(TestRecipe);// TEST DATABASE INSERTS
        await Navigation.PushAsync(new TabView());

    }

}
