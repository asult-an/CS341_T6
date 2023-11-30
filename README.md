# CS341_T6
## Changes (PROJECT MILESTONE 4)
User Profile page layout ✅
Hashed User Authentication ✅
Functional implementation of cookbook page ✅
Create custom cookbooks/display on cookbook page - Saved for next sprint
Recipe filters in feed - latest recipe method in progress, other filters have not been started yet
Event handlers added to UserSettings page ✅
Create UserPreferences model - User Preferences will be implemented next sprint, but the database has had preparations made
Get toggle button working for dark mode - Not done: Not vital for this sprint.
Light mode pages - Not done: Not vital for this sprint
App logo - Not done: Not vital for this sprint
Tag management - Not done: Getting core functionality done first
Basic layout of profile page - ✅
Basic layout of settings page - ✅
Cookbook page V1 finalized - ✅ V2 will include custom recipe lists
Cookbage page V2’s code-behind is complete, the UI just needs to be reworked
Feed grabs random recipes from the database
Recipes can be displayed in a pop up view to see the basic details
Ingredient Population:✅ When adding a new recipe, ingredients are available from a convenient Picker element: the ingredients also appear on the display.  The remove button needs to be hooked up, however.
Dietary Restrictions: This task was delayed, more should have been done but it was decided to keep this for next sprint
User Authentication: Passwords stored during registration are now salted & hashed instead of being stored as plaintext
Misc: Dependency Injection was fully configured, a static ServiceProvider class was built into MauiProgram so that references that aren’t relevant to a context in a specific domain can be provided anytime, anywhere


# Changes (PROJECT MILESTONE 3)
Signup, Login, and Add Recipe functionalities have been implemented and tested 😃😃
The AccountSettings page has been implemented and allows the user to change their password 😃
Our rudimentary testbed has been upgraded to introduce (and implement) models for Tag & Ingredient
The ER Diagram was found to be misrepresentative: after revision, several major BL ambiguities surrounding
	the relation between tags, ingredients, and preferences were resolved 😃😃😃
The DB has been prepped for user preferences to be implemented, we will likely be foregoing JSON for *dietary* preferences 
The UI's backend has been revised to incorporate the new models and their respective interfaces 😃
Add recipe pages are able to select images stored on phone to store in database
Cookbook is partially implemented and is fetching data from the database
Color changes and user design have been updated

# Changes (PROJECT MILESTONE 2)
UserLogic, RecipeLogic, and RecipeDatabase classes have been implemented 😃
UserDatabase have been partially implemented, and will need some work to bring them to expected functionality 
UserLogic, UserDatabase, RecipeLogic, and RecipeDatabase interfaces have been implemented 😃
Feed and AddRecipe pages have been filled with additional placeholder data and input elements 😃😃😃
Navigation to Login/Signup, Feed, Cookbook, and Add Recipe pages has been implemented 😃😃


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


