using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMobileDevice.Generator
{
    internal static class DictionaryExtensions
    {
        internal static void AddToInnerList<TKey, TValue>(this IDictionary<TKey, ImmutableList<TValue>> dictionary, TKey key, TValue item)
        {
            ImmutableList<TValue> items;

            if (dictionary.TryGetValue(key, out items))
            {
                dictionary[key] = items.Add(item);
            }
            else
            {
                dictionary.Add(key, ImmutableList.Create(item));
            }
        }
    }
}