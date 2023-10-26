using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    internal class Recipe
    {
        private int id;
        private string name;
        private string description;
        private int author;
        private ObservableCollection<string> ingredients;
        private ObservableCollection<string> ingredientsQty;
        private int cookTime;
        private string course;
        private int rating;
        private int servings;
        private string image;
        private ObservableCollection<string> tags;
        private ObservableCollection<string> followers;
        public Recipe() { }

        public Recipe(int inId, string inName, string inDescription, int inAuthor, 
            ObservableCollection<string> inIngredients, ObservableCollection<string> inIngredientsQty, 
            int inCooktime, string inCourse, int inRating, int inServings, string inImage, 
            ObservableCollection<string> inTags, ObservableCollection<string> inFollowers) 
        { 
            id = inId;
            name = inName;
            description = inDescription;
            author = inAuthor;
            ingredients = inIngredients;
            ingredientsQty = inIngredientsQty;
            cookTime = inCooktime;
            course = inCourse;
            rating = inRating;
            servings = inServings;
            image = inImage;
            tags = inTags;
            followers = inFollowers;
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
        public int Author { get { return author; } set { author = value; } }
        public ObservableCollection<string> Ingredients { get { return ingredients; } set { ingredients = value; } }
        public ObservableCollection<string> IngredientsQty { get { return ingredientsQty; } set { ingredientsQty = value; } }
        public int CookTime { get { return cookTime; } set { cookTime = value; } }
        public string Course { get { return course; } set { course = value; } }
        public int Rating { get { return rating; } set { rating = value; } }
        public int Servings { get { return servings; } set { servings = value; } }
        public string Image { get { return image; } set { image = value; } }
        public ObservableCollection<string> Tags { get { return tags; } set { tags = value; } }
        public ObservableCollection <string> Followers {  get { return followers; } set {  followers = value; } }

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
