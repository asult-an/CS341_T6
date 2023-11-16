using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    /// <summary>
    /// Class to represent a Recipe. 
    /// TODO: forego custom_(...) tables for a isPublic column in the Database
    /// Though, this creates a new design problem: 
    /// what happens when a user creates a new tag?  Other users probably don't want to see them, so a custom_tags table still might be justifiable.
    /// Since tags are otherwise public, any non-public tags should be shown to the user in an aggregate list during recipe creation
    /// 
    /// Only stores Ids of users following the recipe, not the users themselves.  Could either use UserDatabase or design a FollowerDatabase service, 
    /// but using UserDatabase requires less refactoring
    /// </summary>
    public class Recipe
    {
        private int id;
        private string name;
        private string description;

        private int authorId;
        private Ingredient[] ingredients;
        private string ingredientsQty;

        private int cookTime;
        private CourseType course;
        private int rating;
        private int servings;
        private byte[] image;

        private Tag[] tags;// todo
        private int[] followerIds;//todo

        /// <summary>
        /// Empty constructor for a Recipe object, used for testing purposes
        /// </summary>
        public Recipe() { }


        // TODO: use a default image as the default parameter instead of none
        /// <summary>
        /// Creates a new Recipe object, missing several parameters including author_id, tags, and followers
        /// </summary>
        /// <param name="name">Recipe's name to be shown</param>
        /// <param name="description">More information about the recipe, should describe its taste</param>
        /// <param name="cookTime">Number of minutes to cook the meal, possibly counting cooling if specified in description</param>
        /// <param name="rating">the rating of the recipe: defaults to -1, especially if user chooses not to upload to public repository</param>
        /// <param name="servings">number of mouths the dish feeds</param>
        /// <param name="course">one of the predefined CourseTypes</param>
        /// <param name="ingredients">Array of Ingredients, resolved by RecipeLogic</param>
        /// <param name="tags">Array of tagIds that this recipe is tagged as</param>
        /// <param name="followerIds">Array of userIds of the users following this recipe</param>
        /// <param name="authorId">The Id of the user who created this recipe, -1 </param>
        public Recipe(
            string name,
            string description,
            int cookTime,
            Ingredient[] ingredients,
            CourseType course,
            int authorId,
            int rating = 3,
            int servings = 1,
            Tag[] tags = null,
            int[] followerIds = null,
            byte[] imageArr = null
        )
        {
            this.name = name;
            this.description = description;
            this.cookTime = cookTime;
            this.course = course;
            this.tags = tags;
            this.authorId = authorId;
            this.rating = rating;
            this.servings = servings;
            this.followerIds = followerIds;
            this.ingredients = ingredients;
            this.image = imageArr;

        }
        /// <summary>
        /// Creates a new Recipe object, missing several parameters including author_id, tags, and followers
        /// </summary>
        /// <param name="recipeID">Recipe's ID, used for updates</param>
        /// <param name="name">Recipe's name to be shown</param>
        /// <param name="description">More information about the recipe, should describe its taste</param>
        /// <param name="cookTime">Number of minutes to cook the meal, possibly counting cooling if specified in description</param>
        /// <param name="rating">the rating of the recipe: defaults to -1, especially if user chooses not to upload to public repository</param>
        /// <param name="servings">number of mouths the dish feeds</param>
        /// <param name="course">one of the predefined CourseTypes</param>
        /// <param name="ingredients">Array of Ingredients, resolved by RecipeLogic</param>
        /// <param name="tags">Array of tagIds that this recipe is tagged as</param>
        /// <param name="followerIds">Array of userIds of the users following this recipe</param>
        /// <param name="imageArr">The i</param>
        public Recipe(
            int recipeID,
            string name,
            string description,
            int cookTime,
            Ingredient[] ingredients,
            CourseType course,
            int authorId,
            int rating = 3,
            int servings = 1,
            Tag[] tags = null,
            int[] followerIds = null,
            byte[] imageArr = null
        )
        {
            this.name = name;
            this.description = description;
            this.cookTime = cookTime;
            this.course = course;
            this.tags = tags;
            this.rating = rating;
            this.servings = servings;
            this.followerIds = followerIds;
            this.ingredients = ingredients;
            this.image = imageArr;

        }
        public int ID { get { return id; } set { id = value; } }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int AuthorID { get { return authorId; } set { authorId = value; } }
        public Ingredient[] Ingredients { get { return ingredients; } set { ingredients = value; } }
        public String IngredientsQty { get { return ingredientsQty; } set { ingredientsQty = value; } }

        public int CookTime { get { return cookTime; } set { cookTime = value; } }
        public CourseType Course { get { return course; } set { course = value; } }
        public int Rating { get { return rating; } set { rating = value; } }
        public int Servings { get { return servings; } set { servings = value; } }
        public byte[] Image { get { return image; } set { image = value; } }
        public Tag[] Tags { get { return tags; } set { tags = value; } }
        
        public int[] FollowerIds { get { return followerIds; } set { followerIds = value; } }


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
            return FollowerIds.ToString();
        }
    }
}
