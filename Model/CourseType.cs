using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class CourseType
    {
        private string _Name;

        /// <summary>
        /// the private store of the course types
        /// </summary>
        private static List<CourseType> _CourseTypes = new List<CourseType>();

        /// <summary>
        /// public facing getter to retrieve ALL course types, useful from the UI to databind
        /// </summary>
        public static List<CourseType> CourseTypes { get { return _CourseTypes; } }

        public static readonly CourseType Unspecified = new CourseType("Unspecified");
        public static readonly CourseType Snack = new CourseType("Snack");
        public static readonly CourseType Breakfast = new CourseType("Breakfast");
        public static readonly CourseType Lunch = new CourseType("Lunch");
        public static readonly CourseType Dinner = new CourseType("Dinner");

        /// <summary>
        /// Returns the name of a coursetype, e.g. "Lunch"
        /// </summary>
        public string Name { get { return _Name; } private set { _Name = value; } }


        public CourseType(string name)
        {
            Name = name;
            _CourseTypes.Add(this);
        }

        public static CourseType Parse(string toParse)
        {
            foreach(CourseType courseType in _CourseTypes)
            {
                if (string.Equals(courseType.Name, toParse, StringComparison.OrdinalIgnoreCase))
                    return courseType;
            }
            throw new FormatException("Invalid CourseType!");
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
