using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{

    /// <summary>
    /// Class to represent a tag.  
    /// Tags are used to identify recipes with various definitions.  For the initial phase
    /// of development, we'll unify all tags in one table that all users can freely contribute 
    /// to. 
    /// A tag has two attributes: its id, and its DisplayName.  The Id is seldom used outside of 
    /// tag-based operations (e.g modifying an existing tag, adding a new tag)
    /// </summary>
    public class Tag
    {
        private int id;
        private string displayName;


        /// <summary>
        /// The string of text that the tag will show up as
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// Unique identifier for the tag
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }


    }
}
