using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model.Services
{
    internal class UserService
    {
        // place for the injected datbase instance to load into
        private readonly IUserDatabase userDatabase;

        // Unlike Lab3, actually implement Dependency Injection from DB to BL
        
        /// <summary>
        /// Creates a new UserService object with an instance of the database loaded
        /// via DI.  
        /// </summary>
        /// <param name="userDatabase"></param>
        public UserService(IUserDatabase userDatabase)
        {
            this.userDatabase = userDatabase;
        }

        

    }
}
