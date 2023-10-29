using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class User
    {
        //private fields for the User class:
        //username, email, and password fields: username will be the unique identifier for users
        private string username;
        private string email;
        private string password;
        //string ObservableCollections of app preferences and dietary preferences
        private ObservableCollection<string> appPreferences;
        private ObservableCollection<string> dietaryPreferences;
        //reference string to the user's profile picture
        private string profilePicture;
        //Recipe ObservableCollection of the recipes a user has created
        private ObservableCollection<Recipe> authorList;
        //a list of users the user follows
        private List<string> followers;
        //a list of users that follows the user
        private List<string> following;
        //a list of recipes the user has saved
        private ObservableCollection<Recipe> cookBook;

        public string Username { get { return username; } set {  username = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public ObservableCollection<string> AppPreferences { get {  return appPreferences; } set {  appPreferences = value; } }
        public ObservableCollection <string> DietaryPreferences { get { return dietaryPreferences; } set {  dietaryPreferences = value; } }
        public string ProfilePicture { get {  return profilePicture; } set {  profilePicture = value; } }
        public ObservableCollection<Recipe > AuthorList { get { return authorList; } set { authorList = value; } }
        public List<string> Followers { get { return followers; } set {  followers = value; } }
        public List<string> Following { get { return following; } set { following = value; } }
        public ObservableCollection<Recipe> CookBook { get {  return cookBook; } set {  cookBook = value; } }

    }
}
