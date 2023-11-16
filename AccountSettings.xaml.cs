using CookNook.Model;

namespace CookNook;

public partial class AccountSettings : ContentPage
{
	UserLogic userLogic = new UserLogic();
	public AccountSettings()
	{
		InitializeComponent();
	}

	public async void UpdateClicked(object sender, EventArgs e)
	{
		//Make sure new passwords match
		if (NewPassword.Text != NewPasswordConfirm.Text)
		{
			DisplayAlert("Error", "New passwords do not match", "Okay");
		}
		//Confirm old password
		//if (!userLogic.CheckPassword())
		//{
  //          DisplayAlert("Error", "Old password is incorrect", "Okay");
  //      }
		else
		{
            //Set new password
            DisplayAlert("Success", "Password changed", "Okay");
        }


    }
	
}