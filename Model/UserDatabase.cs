
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal class UserDatabase : IUserDatabase
    {
        private List<string> followersList = new List<string>();
        private List<string> followingList = new List<string>();
        //private ObservableCollection<User> followers = new ObservableCollection<User>();
       // private ObservableCollection<User> following = new ObservableCollection<User>();
       // public ObservableCollection<User> Followers {  get { return followers; } }
       // public ObservableCollection<User> Following { get { return following; } }
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        private RecipeDatabase RecipeDB = new RecipeDatabase();
        //create public property to access airport list
      
              

        public static String GetConnectionString()
        {
            //initialize the string builder
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            //set the properties of the string builder
            connStringBuilder.Host = "third-sphinx-13032.5xj.cockroachlabs.cloud";
            connStringBuilder.Port = PORT_NUMBER;
            connStringBuilder.SslMode = SslMode.VerifyFull;
            connStringBuilder.Username = dbUsername;
            connStringBuilder.Password = dbPassword;
            connStringBuilder.Database = "defaultdb";
            connStringBuilder.ApplicationName = "";
            connStringBuilder.IncludeErrorDetail = true;
            //return the completed string
            return connStringBuilder.ConnectionString;
        }

        public ObservableCollection<User> Follow(string userID)
        {
            if (followingList.Contains(userID))
            {
                Console.WriteLine("Tried to follow someone who is already followed");
                return null;
            }
            followingList.Add(userID);
            return SelectAllUsers(followingList);
        }

        public ObservableCollection<User> Unfollow(string userID)
        {
            if (followingList.Contains(userID))
            {
                followingList.Add(userID);
                return SelectAllUsers(followingList);
                
            }
            Console.WriteLine("Tried to unfollow someone who isn't followed");
            return null;

        }

        public ObservableCollection<User> LoadFollowers(List<string> followers)
        {
            return SelectAllUsers(followers);
        }

        public User SelectUser(int userID)
        {
            throw new NotImplementedException();
        }

        public UserAdditionError InsertUser(User inUser)
        {
            throw new NotImplementedException();
        }

        public UserEditError EditUser(User inUser)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<User> SelectAllUsers(List<string> userIDs)
        {
            ObservableCollection<User> outUsers = new ObservableCollection<User>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            //initialize a new SQL command
            var cmd = new NpgsqlCommand();
            //connect the database to the command
            cmd.Connection = conn;
            NpgsqlDataReader reader;
            foreach (string userID in userIDs)
            {
                User user = new User();
                cmd.CommandText = "SELECT * FROM recipes WHERE recipe_id = @Recipe_ID";
                cmd.Parameters.AddWithValue("Recipe_ID", userID);
                reader = cmd.ExecuteReader();
                reader.Read();
                user.Username = reader.GetString(0);
                user.Email = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.AppPreferences = CreateStringCollection(reader.GetString(3));
                user.DietaryPreferences = CreateStringCollection(reader.GetString(4));
                user.ProfilePicture = reader.GetString(5);
                user.AuthorList = RecipeDB.SelectAllRecipes(CreateIntList(reader.GetString(6)));
                user.Followers = CreateStringList(reader.GetString(7));
                user.Following = CreateStringList(reader.GetString(8));
                outUsers.Add(user);
            }
            return outUsers;
        }
        private ObservableCollection<string> CreateStringCollection(string rawList)
        {
            List<string> list = rawList.Split(',').ToList<String>();
            return new ObservableCollection<string>(list);

        }
        private List<int> CreateIntList(string rawList) 
        {
            return rawList.Split(',').Select(int.Parse).ToList<int>();
        }
        private List<string> CreateStringList(string rawList)
        {
            return rawList.Split(',').ToList<string>();
        }
    }
}
