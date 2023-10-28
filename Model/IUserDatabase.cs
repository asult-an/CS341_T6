using CookNook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public interface IUserDatabase
    {

        public User SelectUser();
        public UserAdditionError InsertUser(User inUser);
        public UserEditError EditUser(User inUser);
        public UserDeletionError DeleteUser(User inUser);
    }
}
