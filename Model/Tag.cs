using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Empty constructor, allows for bracketed object construction
        /// </summary>
        public Tag() { }

        public Tag(int id, string name)
        {
            this.id = id;
            this.displayName = name;
        }

        /// <summary>
        /// Id-less constructor for a Tag: useful if the database can see the -1 as a trigger to 
        /// generate a new Id.  Though, if we just don't send one up at all, it will also come up
        /// with one.
        /// </summary>
        /// <param name="name"></param>
        public Tag(string name)
        {
            this.id = -1;
            this.displayName = name;
        }

        public static Tag Parse(string toParse)
        {
            Debug.Write("Here's what you need to parse: " + toParse);

            throw new FormatException("Invalid CourseType!");
        }
    }
}
