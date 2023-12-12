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
        private Int64 id;
        private string username;
        private string email;
        private string password;
        private List<string> appPreferences;
        private List<DietPreference> dietaryPreferences;
        private string profilePicture;
        private List<Int64> authorList;
        private List<Int64> followers;
        private List<Int64> following;

        private List<Recipe> cookBook;
        public User() { }

        public User(Int64 id, string inUsername,  string inEmail, string inPassword)
        {
            username = inUsername;
            email = inEmail;
            password = inPassword;
        }

        public Int64 Id { get { return id; } set { id = value; } }
        public string Username { get { return username; } set {  username = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public List<string> AppPreferences { get {  return appPreferences; } set {  appPreferences = value; } }
        public List<DietPreference> DietaryPreferences { get { return dietaryPreferences; } set {  dietaryPreferences = value; } }
        public string ProfilePicture { get {  return profilePicture; } set {  profilePicture = value; } }
        public List<Int64> AuthorList { get { return authorList; } set { authorList = value; } }
        public List<Int64> Followers { get { return followers; } set {  followers = value; } }
        public List<Int64> Following { get { return following; } set { following = value; } }
        public List<Recipe> CookBook { get {  return cookBook; } set {  cookBook = value; } }

    }
}
