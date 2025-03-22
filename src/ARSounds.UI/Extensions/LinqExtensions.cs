using System.Collections.Generic;

namespace ARSounds.UI.Extensions;

public static class LinqExtensions
{
    public static void AddRange<T>(this ICollection<T> items, IEnumerable<T> items2)
    {
        foreach (var item in items2)
        {
            items.Add(item);
        }
    }
}
