
namespace CookNook.Model;

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
    NoUserWithEmail,
    NoError
}
