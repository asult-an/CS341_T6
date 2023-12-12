using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookNook.Model;
using CookNook.Model.Interfaces;

namespace CookNook;

public partial class DietaryRestrictionsPage : ContentPage
{
    private readonly IPreferenceProvider preferenceProvider;

    // reference to the current user
    private User user;


    /// <summary>
    /// Constructs page for viewing all dietary restrictions.  
    /// Requires a user, since only users can own a dietary preference
    /// </summary>
    /// <param name="inUser"></param>
    public DietaryRestrictionsPage(User inUser)
    {
        InitializeComponent();
        if(inUser == null)
        {
            // try getting user from UserViewModel/userLogic
            // var user = userLogic.CurrentUser;
            var user = UserViewModel.Instance.AppUser;
            if (user != null)
                inUser = user;
        }
        user = inUser;

        preferenceProvider = MauiProgram.ServiceProvider.GetService<IPreferenceProvider>();
        
        // get (and set) the user's preferences
        user.DietaryPreferences = preferenceProvider.GetLocalPreferencesAsync().Result;
    }

    public async void BackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    public async void FinishedAddClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TabView());
    }
}