
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

        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
        private string connString = GetConnectionString();
        static string dbPassword = "0eQSU1bp88pfd5hxYpfShw";
        static string dbUsername = "adeel";
        static int PORT_NUMBER = 26257;
        //create public property to access airport list
        public ObservableCollection<Recipe> Recipes { get { return recipes; } }
              

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

        public User SelectUser()
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
