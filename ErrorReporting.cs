
namespace CookNook;

public enum RecipeAdditionError
{
    InvalidRating,
    InvalidCourse,
    InvalidImage,
    InvalidServings,
    InvalidName,
    InvalidDescription,
    InvalidCookTime,
    DuplicateId,
    DBAdditionError,
    NoError
}

public enum RecipeSelectionError
{
    InvalidRecipeId,
    RecipeAlreadyFollowed,
    NoError
}

public enum RecipeDeletionError
{
    RecipeNotFound,
    DBDeletionError,
    NoError
}

public enum RecipeEditError
{
    RecipeNotFound,
    InvalidFieldError,
    DBEditError,
    NoError
}

public enum UserAdditionError
{
    DuplicateUsername,
    InvalidUsername,
    InvalidPassword,
    InvalidEmail,
    DBAdditionError,
    NoError
}

public enum UserDeletionError
{
    UserNotFound,
    DBDeletionError,
    NoError
}

public enum UserEditError
{
    UserNotFound,
    InvalidFieldError,
    DBEditError,
    NoError
}

public enum UserSelectionError
{
    InvalidEmailAddress,
    InvalidPassword,
    InvalidUsername,
    NoUserWithId,
    RecipeAlreadyFollowed,
    UserAlreadyFollowed,
    UserAlreadyUnfollowed,
    NoError
}

public enum UserAuthenticationError
{
    InvalidUsername,
    InvalidPassword,
    NoError
}

public enum TagAdditionError
{
    TagAlreadyExists,
    DBAdditionError,
    NoError
}

public enum TagDeleteError
{
    TagNotFound,
    DBDeletionError,
    NoError
}

public enum TagUpdateError
{
    TagNotFound,
    DBUpdateError,
    NoError
}

public enum IngredientAdditionError
{
    IngredientAlreadyExists,
    DBAdditionError,
    BadParameters,
    NoError
}

public enum IngredientDeleteError
{
    IngredientNotFound,
    DBDeletionError,
    NoError
}

public enum IngredientUpdateError
{
    IngredientNotFound,
    DBUpdateError,
    BadParameters,
    NoError
}

public enum IngredientSelectionError
{
    IngredientNotFound,
    BadParameters,
    NoError
}


public enum CookbookPageAdditionError
{
    InvalidRecipeProvided,
    InvalidListProvided,
    ListNotFound,
    Unauthenticated,
    Unspecified,
    NoError
}
public enum CookbookPageDeletionError
{
    AdminOnlyOperation,
    InvalidRecipeProvided,
    InvalidListProvided,
    ListNotFound,
    Unauthenticated,
    Unspecified,
    NoError
}