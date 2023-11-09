using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class Recipe
    {
        private int id;
        private string name;
        private string description;
        private int authorId;
        private string ingredients;
        private string ingredientsQty;
        private int cookTime;
        private CourseType course;
        private int rating;
        private int servings;
        private string image;
        string tags;
        int[] tagIds;// todo
        int[] followerIds;//todo
        string followers;
        
        /// <summary>
        /// Empty constructor for a Recipe object, used for testing purposes
        /// </summary>
        public Recipe() { }


        // TODO: use a default image as the default parameter instead of none
        /// <summary>
        ///         /// Creates a new Recipe object, missing several parameters including author_id, tags, and followers
        /// </summary&gt;
        /// <param name="name">Recipe's name to be shown&lt;/param&gt;
        /// <param name="description">More information about the recipe, should describe its taste&lt;/param&gt;
        /// <param name="cookTime">Number of minutes to cook the meal, possibly counting cooling if specified in description&lt;/param&gt;
        /// <param name="rating">the rating of the recipe: defaults to -1, especially if user chooses not to upload to public repository&lt;/param&gt;
        /// <param name="servings">number of mouths the dish feeds&lt;/param&gt;
        /// <param name="course">one of the predefined CourseTypes</param>
        /// <param name="tagIds"></param>
        /// <param name="rating"></param>
        /// <param name="servings"></param>
        public Recipe(string name, string description, int cookTime, CourseType course, int[] tagIds = null, int rating = 3, int servings = 1) { 
             this.name = name;
            this.description = description;
            this.cookTime = cookTime;
            this.course = course;
            this.tagIds = tagIds;
            this.rating = rating;
            this.servings = servings;
            followerIds = Array.Empty<int>();
        }

        /// <summary>
        /// Old constructor for the recipe, will be deleted when merging the final branch
        /// </summary>
        [Obsolete]
        public Recipe(int inId, string inName, string inDescription, int inAuthorID,
            string inIngredients, string inIngredientsQty, 
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            string inTags, string inFollowers) 
        { 
            //id = inId;
            //name = inName;
            //description = inDescription;
            //authorId = inAuthorID;
            //ingredients = inIngredients;
            //ingredientsQty = inIngredientsQty;
            //cookTime = inCooktime;
            //course = inCourse;
            //rating = inRating;
            //servings = inServings;
            //image = inImage;
            //tags = inTags;
            //followers = inFollowers;
        }

        public int ID { get { return id; } set { id = value; } }
        public string Name { 
            get { return name; } 
            set { name = value; } }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int AuthorID { get { return authorId; } set { authorId = value; } }
        public String Ingredients { get { return ingredients; } set { ingredients = value; } }
        public String IngredientsQty { get { return ingredientsQty; } set { ingredientsQty = value; } }
        public int CookTime { get { return cookTime; } set { cookTime = value; } }
        public CourseType Course { get { return course; } set { course = value; } }
        public int Rating { get { return rating; } set { rating = value; } }
        public int Servings { get { return servings; } set { servings = value; } }
        public string Image { get { return image; } set { image = value; } }
        public String Tags { get { return tags; } set { tags = value; } }
        public string Followers {  get { return followers; } set {  followers = value; } }

        public string IngredientsToString()
        {
            return Ingredients.ToString();
        }
        public string IngredientsQtyToString()
        {
            return Ingredients.ToString();
        }
        public string TagsToString()
        {
            return Ingredients.ToString();
        }
        public string FollowersToString()
        {
            return Followers.ToString();
        }
    }
}
