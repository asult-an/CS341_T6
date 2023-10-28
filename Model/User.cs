using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal class User
    {
        private string username;
        private string email;
        private string password;
        private ObservableCollection<string> appPreferences;
        private ObservableCollection<string> dietaryPreferences;
        private string profilePicture;
        private ObservableCollection<Recipe> authorList;
        private ObservableCollection<User> followList;
        private ObservableCollection<Recipe> cookBook;

        public string Username { get { return username; } set {  username = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public ObservableCollection<string> AppPreferences { get {  return appPreferences; } set {  appPreferences = value; } }
        public ObservableCollection <string> DietaryPreferences { get { return dietaryPreferences; } set {  dietaryPreferences = value; } }
        public string ProfilePicture { get {  return profilePicture; } set {  profilePicture = value; } }
        public ObservableCollection<Recipe > AuthorList { get { return authorList; } set { authorList = value; } }
        public ObservableCollection<User> FollowList { get { return followList; } set {  followList = value; } }
        public ObservableCollection<Recipe> CookBook { get {  return cookBook; } set {  cookBook = value; } }

    }
}
