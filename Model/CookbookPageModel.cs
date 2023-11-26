using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    /// <summary>
    /// Helps facilitate the loading of various user-defined cookbook pages by creating 
    /// locations for the columns from user_cookbook_pages to come back through
    /// </summary>
    public class CookbookPageModel
    {
        private string pageName;
        private long userId;
        private long listId;


        public string PageName { get { return pageName; } set { pageName = value; } }
        public long UserId { get { return userId; } set { userId = value; } }
        public long ListId { get { return listId; } set { listId = value; } }   
        
        /// <summary>
        /// Constructor for CookbookPageModel
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="userId"></param>
        /// <param name="listId"></param>
        public CookbookPageModel(string pageName, long userId, long listId)
        {
            this.pageName = pageName;
            this.userId = userId;
            this.listId = listId;
        }

    }
}
