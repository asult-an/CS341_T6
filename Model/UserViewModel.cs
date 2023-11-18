using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class UserViewModel
    {
        private User user;
        public User AppUser { get { return user; } set { user = value; } }
        private static UserViewModel instance;
        public static UserViewModel Instance => instance ?? (instance = new UserViewModel());
    }
}
