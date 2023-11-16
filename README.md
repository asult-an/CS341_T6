# CS341_T6

# Changes (PROJECT MILESTONE 3)
Signup, Login, and Add Recipe functionalities have been implemented and tested 😃😃
The AccountSettings page has been implemented and allows the user to change their password 😃
Our rudimentary testbed has been upgraded to introduce (and implement) models for Tag & Ingredient
The ER Diagram was found to be misrepresentative: after revision, several major BL ambiguities surrounding
	the relation between tags, ingredients, and preferences were resolved 😃😃😃
The DB has been prepped for user preferences to be implemented, we will likely be foregoing JSON for *dietary* preferences 
The UI's backend has been revised to incorporate the new models and their respective interfaces 😃

# Changes (PROJECT MILESTONE 2)
UserLogic, RecipeLogic, and RecipeDatabase classes have been implemented 😃
UserDatabase have been partially implemented, and will need some work to bring them to expected functionality 
UserLogic, UserDatabase, RecipeLogic, and RecipeDatabase interfaces have been implemented 😃
Feed and AddRecipe pages have been filled with additional placeholder data and input elements 😃😃😃
Navigation to Login/Signup, Feed, Cookbook, and Add Recipe pages has been implemented 😃😃
Add recipe pages are able to select images stored on phone to store in database
Cookbook is partially implemented and is fetching data from the database

# Coding Standards

## C# Standards
### File
**Whitespace** - One statement per line (duh!)
**Comments everywhere!**
### Limits
- Columns: 115
- Class Size: 400 lines
- Method size: 80 lines
##### Class Skeleton:
  ```FooClass
	  |
	  + -- Constants
	  + ---- Properties 
	  + ------ Constructor
	  + -------- Methods
  ```

### Syntax
**PascalCase**:
- Directories (`CookNook/Models`)
- Classes (`BizzBuzz.cs`)
- Methods (`GetJarFile()`)
- Public Properties (`public string Username`)

**camelCase**:
- local variables (`int numClicks`)
- private variables (`private int fooId...`)

### XAML Standards
TODO!

### SQL Standards:
**Table names**: lowercase, snake_case (`airports_table`)
**Attribute names**: lowercase, snake_case (`plane_make`)
**Values**: lowercase if internal values 


