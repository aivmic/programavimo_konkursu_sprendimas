namespace Solutions;

public static class StringExtensions
{
    /// <summary>
    /// Repeats a string a number of times.
    /// </summary>
    /// <param name="text">text to repeat</param>
    /// <param name="count">times to repeat text for</param>
    /// <returns>repeated text</returns>
    public static string Repeat(this string text, int count)
    {
        return string.Join(
            "",
            Enumerable.Repeat(
                text,
                count
            )
        );
    }

    /// <summary>
    /// Converts a string to a dictionary of character counts.
    /// </summary>
    /// <param name="text">Text whose characters to count</param>
    /// <returns>Dictionary of characters and their counts</returns>
    public static Dictionary<char, int> ToCharCounts(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new Dictionary<char, int>();
        }

        return text
            .GroupBy(c => c)
            .ToDictionary(
                g => g.Key,
                g => g.Count()
            );
    }
}

public static class ObjectExtensions
{
    /// <summary>
    /// Maps an object to another object using a function.
    /// </summary>
    /// <typeparam name="T">Initial object type</typeparam>
    /// <param name="obj">Object of type T</param>
    /// <param name="map">Mapping function to map object T to object of type R</param>
    /// <returns>Object of type R</returns>
    public static R To<T, R>(this T obj, Func<T, R> map)
    {
        if (
            obj == null ||
            EqualityComparer<T>.Default.Equals(obj, default) ||
            map == null
        )
        {
            return default;
        }

        return map(obj);
    }
}