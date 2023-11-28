using CookNook.Model;
using CookNook.Model.Interfaces;
using CookNook.Model.Services;
using CookNook.Services;
using Microsoft.Extensions.Logging;
namespace CookNook;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterServices();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	/// <summary>
	/// Helper method for the app to configure dependency injection.
	/// Registers any BL classes as transient services since it can
	/// get expensive to call the databases, so we avoid adding it 
	/// as a singleton
	/// </summary>
	/// <param name="builder">the instance of the builder hosting the App</param>
	/// <returns>the builder, once it's had the services added</returns>
	public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
	{ 
		builder.Services.AddScoped<IRecipeDatabase, RecipeDatabase>();
		builder.Services.AddScoped<IRecipeLogic, RecipeLogic>();
		builder.Services.AddScoped<IIngredientDatabase, IngredientDatabase>();
		builder.Services.AddScoped<IIngredientLogic, IngredientLogic>();
		builder.Services.AddScoped<IUserDatabase, UserDatabase>();
		builder.Services.AddScoped<IUserLogic, UserLogic>();

		//builder.Services.
		// Hacky, but this gets it to compile for now.
		// TODO: troubleshoot later, true DI needed
		//builder.Services.AddSingleton<UserLogic>();
		return builder;
	}
}

