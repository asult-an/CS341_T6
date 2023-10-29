﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookNook.Model
{
    public class Recipe
    {
        //create private fields to store recipe data
        //unique recipe ID/PK in database
        private int id;
        //recipe name, description, author
        private string name;
        private string description;
        private string author;
        //observable collections of ingredients and quanitities
        private ObservableCollection<string> ingredients;
        private ObservableCollection<string> ingredientsQty;
        //recipe cook time, course, rating, and image reference
        private int cookTime;
        private string course;
        private int rating;
        private int servings;
        private string image;
        //observable collections of tags and users following the recipe
        private ObservableCollection<string> tags;
        private ObservableCollection<string> followers;
        public Recipe() { }

        //constructor to initialize recipe with all fields
        public Recipe(int inId, string inName, string inDescription, string inAuthor,
            ObservableCollection<String> inIngredients, ObservableCollection<String> inIngredientsQty, 
            int inCooktime, string inCourse, int inRating, int inServings, string inImage,
            ObservableCollection<String> inTags, ObservableCollection<string> inFollowers) 
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
        //public properties to access recipe fields
        public int ID { get { return id; } set { id = value; } }
        public string Name { 
            get { return name; } 
            set { name = value; } }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Author { get { return author; } set { author = value; } }
        public ObservableCollection<String> Ingredients { get { return ingredients; } set { ingredients = value; } }
        public ObservableCollection<String> IngredientsQty { get { return ingredientsQty; } set { ingredientsQty = value; } }
        public int CookTime { get { return cookTime; } set { cookTime = value; } }
        public string Course { get { return course; } set { course = value; } }
        public int Rating { get { return rating; } set { rating = value; } }
        public int Servings { get { return servings; } set { servings = value; } }
        public string Image { get { return image; } set { image = value; } }
        public ObservableCollection<String> Tags { get { return tags; } set { tags = value; } }
        public ObservableCollection<string> Followers {  get { return followers; } set {  followers = value; } }
        //to string methods for recipe fields
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
