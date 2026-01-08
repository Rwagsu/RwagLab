using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RwagLab.Extensions;

public static class IEnumerableExtension {
    public static int GetIndexOrDefault<T>(this IEnumerable<T> source, Func<T, bool> func) {
        var item = source.FirstOrDefault(func);
        return source.IndexOf(item);
    }

    public static void AddIfNotNull<T>(this ICollection<T> source, T? item) where T : class {
        if (item != null) {
            source.Add(item);
        }
    }
}
