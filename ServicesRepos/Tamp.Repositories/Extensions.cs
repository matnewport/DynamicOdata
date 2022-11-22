using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Tamp.Repositories
{
    public static class TypeExentsions
    {
        /// <summary>
        /// Returns true if the Type is an Entity Framework Dynamic Proxy. This 
        /// method is currently used by SalesOrder and ShoppingCart.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDynamicProxy(this Type type)
        {
            return type.Namespace.Contains("DynamicProxies");
        }
    }
    public static class ObjectExtensions
    {
        private static readonly string _GENERIC_TICK = "`1";

        /// <summary>
        /// Gets the friendly type name. Cleans-up the name of EF dynamic proxies.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetTypeName(this Object obj)
        {
            // A fix to prevent a Null Reference Exception during event logging. It is not desireable to have an error in application that is due to event logging.
            if (obj.GetType().IsDynamicProxy())
            {
                // A fix to prevent an event source such as "AutoRefillEnrollment_4B0327672CAC7359E0DBE735FD96669FB7C2184CBCE12E4C202873DF25990D20" from appearing in the event log.
                // The happens because sometimes an instance of a model is actaully an Entity Framework dynamic proxy.
                return obj.GetType().BaseType.Name;
            }
            else if (obj.GetType().Name.EndsWith(_GENERIC_TICK))
            {
                // generics have ugly names
                return obj.GetType().Name.Replace(_GENERIC_TICK, string.Empty);
            }
            else
            {
                return obj.GetType().Name;
            }
        }
    }
    public static class EnumerableExtension
    {
        /// <summary>
        /// This extension converts an enumerable set to a Dapper TVP.
        /// </summary>
        /// <typeparam name="T">type of enumerbale</typeparam>
        /// <param name="enumerable">list of values</param>
        /// <param name="typeName">database type name</param>
        /// <param name="orderedColumnNames">if more than one column in a TVP, 
        /// columns order must mtach order of columns in TVP</param>
        /// <returns>a custom query parameter</returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>
            (this IEnumerable<T> enumerable,
            string typeName, IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();
            if (typeof(T).IsValueType || typeof(T).FullName.Equals("System.String"))
            {
                dataTable.Columns.Add(orderedColumnNames == null ?
                    "NONAME" : orderedColumnNames.First(), typeof(T));
                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(obj);
                }
            }
            else
            {
                PropertyInfo[] properties = typeof(T).GetProperties
                    (BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] readableProperties = properties.Where
                    (w => w.CanRead).ToArray();
                if (readableProperties.Length > 1 && orderedColumnNames == null)
                    throw new ArgumentException("Ordered list of column names must be provided when TVP contains more than one column");
                var columnNames = (orderedColumnNames ??
                    readableProperties.Select(s => s.Name)).ToArray();
                foreach (string name in columnNames)
                {
                    dataTable.Columns.Add(name, readableProperties.Single
                        (s => s.Name.Equals(name)).PropertyType);
                }

                foreach (T obj in enumerable)
                {
                    dataTable.Rows.Add(
                        columnNames.Select(s => readableProperties.Single
                            (s2 => s2.Name.Equals(s)).GetValue(obj))
                            .ToArray());
                }
            }
            return dataTable.AsTableValuedParameter(typeName);
        }
    }

}