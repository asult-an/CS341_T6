
using System;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CookNook.Model
{
    public class UserDatabase : IUserDatabase
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list

        public ObservableCollection<User> User{ get { return users; } }
              
        public UserDatabase() {
            connString = GetConnectionString();
        }


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


        public List<User> GetAllUsers()
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand("SELECT username, email, password, app_prefs, diet_prefs, profile_pic, author_list, follow_list FROM users");
            }
        }

       
        public UserSelectionError GetByEmail(string email)
        {
            // check if the email is present in the users

            // [T] 

            // [F] return UserSelectionError.NoUserWithEmail;
        }

        public UserSelectionError GetUserById(int userId)
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

        public UserDeletionError DeleteUser(User inUser)
        {
            throw new NotImplementedException();
        }

    }
}
