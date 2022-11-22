using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

public static class EnumerableExtension
{
    public static class Enum<T> where T : struct, IConvertible
    {
        public static string ToDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string c in Enum.GetNames(typeof(T)))
                {
                    var val = ("{0} - {1:D}", c, Enum.Parse(typeof(T), c));
                    sb.Append(val);
                }

                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return sb.ToString();
            }
        }
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).First();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
}