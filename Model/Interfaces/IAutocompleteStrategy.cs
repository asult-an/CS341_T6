using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CookNook.Model.Interfaces;

/// <summary>
/// This is the IAutoCompleteStrategy interface.  
/// It's an exercise in the Strategy Pattern, wherein we can define multiple algorithms to be used interchangeably.
/// 
/// Since autocomplete will be handled slightly differently depending on the Collection, we'll use an interface
/// to abstract away implementation details.  The biggest benefit of this is so that we can switch to the right 
/// variant of (for example) sorting depending on the expected intensity of the operation. 
///
/// For instance: a short string used to match item names in a collection (e.g. "a") will be much less intensive
/// as we can switch to a search that only looks at the first letter of each item name.  However, a longer string
/// (e.g. "apple") will be more intensive, can requires a better search algorithm, such as a binary search.
/// </summary>
/// <typeparam name="T">The type of the collection to be searched</typeparam>
/// <remarks>
/// Further reading: https://www.topcoder.com/thrive/articles/The%20Strategy%20Pattern%20in%20C
/// </remarks>
public interface IAutocompleteStrategy<T>
{
    /// <summary>
    /// Provides recipes based on the input string, using the most suitable algorithm based on the 
    /// context of the requested operation.
    /// </summary>
    /// <remarks>
    /// Requires that the type T has a name-like property to search against
    /// </remarks>
    /// <param name="input">the string to search the collection with</param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetSuggestionsAsync(string input);
}