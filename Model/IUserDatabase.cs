using CookNook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IUserDatabase
    {
        public ObservableCollection<User> Follow(string userID);
        public ObservableCollection<User> Unfollow(string userID);
        public ObservableCollection<User> LoadFollowers(List<string> followers);
        public User SelectUser(int userID);
        public UserAdditionError InsertUser(User inUser);
        public UserEditError EditUser(User inUser);
        public ObservableCollection<User> SelectAllUsers(List<string> users);
    }
}
