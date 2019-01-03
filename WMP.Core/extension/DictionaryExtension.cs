using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMP.Core
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this string json)
        {
            var empty = new Dictionary<TKey, TValue>();
            if (string.IsNullOrEmpty(json)) return empty;

            var p = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);
            return p;
        }

        public static IDictionary<string, object> ToObjectDictionary(this string json)
        {
            return ToDictionary<string, object>(json);
        }

        public static T DeserializeObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static List<Dictionary<string, object>> ToListObjectDictionary(this string json)
        {
            var empty = new List<Dictionary<string, object>>();
            if (string.IsNullOrEmpty(json)) return empty;

            var table = DeserializeObject<List<Dictionary<string, object>>>(json);
            return table;
        }

        /// <summary>
        /// json 문자열 -> DataSet
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataSet ToDataSet(this string json)
        {
            var empty = new DataSet();
            if (string.IsNullOrEmpty(json)) return empty;

            var ds = JsonConvert.DeserializeObject<DataSet>(json);
            return ds;
        }

        public static Dictionary<string, string> ToStringDictionary(this IDictionary<string, object> dict)
        {
            if (null == dict) return new Dictionary<string, string>();

            return dict.ToDictionary(k => k.Key, k => k.Value == null ? string.Empty : k.Value.ToString(), StringComparer.InvariantCultureIgnoreCase);
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this Hashtable ht)
        {
            return ht.Cast<DictionaryEntry>().ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);
        }

        public static Hashtable ToHashtable(this Dictionary<string, string> dic)
        {
            var ht = new Hashtable(dic);
            return ht;
        }

        public static List<Dictionary<string, object>> ToListDictionaryObject(this DataTable table)
        {
            var list = table.AsEnumerable().Select(
                row => table.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName.Replace("\0", string.Empty),
                    column => row[column])
                    ).ToList();

            return list;
        }

        public static Dictionary<string, TValue> ToDictionary<TValue>(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);

            return dictionary;
        }

        public static DataTable CreateTable(this IEnumerable<IDictionary<string, object>> parents)
        {
            var table = new DataTable();

            foreach (var parent in parents)
            {
                var children = parent.Values
                                     .OfType<IEnumerable<IDictionary<string, object>>>()
                                     .ToArray();

                var length = children.Any() ? children.Length : 1;

                var parentEntries = parent.Where(x => x.Value is string)
                                          .Repeat(length)
                                          .ToLookup(x => x.Key, x => x.Value);
                var childEntries = children.SelectMany(x => x.First())
                                           .ToLookup(x => x.Key, x => x.Value);

                var allEntries = parentEntries.Concat(childEntries)
                                              .ToDictionary(x => x.Key, x => x.ToArray());

                var headers = allEntries.Select(x => x.Key)
                                        .Except(table.Columns
                                                     .Cast<DataColumn>()
                                                     .Select(x => x.ColumnName))
                                        .Select(x => new DataColumn(x))
                                        .ToArray();
                table.Columns.AddRange(headers);

                var addedRows = new int[length];
                for (int i = 0; i < length; i++)
                    addedRows[i] = table.Rows.IndexOf(table.Rows.Add());

                foreach (DataColumn col in table.Columns)
                {
                    object[] columnRows;
                    if (!allEntries.TryGetValue(col.ColumnName, out columnRows))
                        continue;

                    for (int i = 0; i < addedRows.Length; i++)
                        table.Rows[addedRows[i]][col] = columnRows[i];
                }
            }

            return table;
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times)
        {
            source = source.ToArray();
            return Enumerable.Range(0, times).SelectMany(_ => source);
        }
    }
}
