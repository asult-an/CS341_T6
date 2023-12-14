using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CookNook;

public partial class SearchPage : ContentPage
{
	private ObservableCollection<User> users;

	private UserLogic userLogic = new UserLogic();

    public ObservableCollection<User> Users
    {
        get { return users; }
        set { users = value; }
    }

    public SearchPage()
	{
		InitializeComponent();
	}

	public async void OnItemTapped(object sender, EventArgs e)
	{
        Debug.WriteLine("IN SEARCH USERS, USER TAPPED");
        throw new NotImplementedException();
	}
    public async void OnTextChanged(object sender, EventArgs e)
    {
        Debug.WriteLine("TEST1234");
        if(SearchInput.Text.Length > 2) 
        {
            //get list users/recipes whose names match the first n letters of the search query
                //write logic and db methods
            //split recipe and user searches??
            //update appropriate observable collection
        } 
    }

    public async void UserProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Profile());
    }

}
