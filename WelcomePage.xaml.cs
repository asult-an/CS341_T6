﻿using CookNook.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CookNook;

public partial class WelcomePage : ContentPage
{
    //TEST DATABASE INSERTS
    

	public WelcomePage()
	{
		InitializeComponent();
        //TestDB.InsertRecipe(TestRecipe);
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
        try
        {
            await Navigation.PushAsync(new TabView());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        

    }

}
