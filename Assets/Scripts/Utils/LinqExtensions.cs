using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public static class LinqExtensions
    {
        private static Random _rnd = new Random();
        /// <summary>
        /// Select random element from collection
        /// </summary>
        /// <typeparam name="T">Collection objects type</typeparam>
        /// <param name="collection">Target collection</param>
        /// <returns>Random element</returns>
        public static T GetRandomElement<T>(this IEnumerable<T> collection)
        {
            if (collection.Count() == 0)
                throw new Exception("Collection is empty");

            return collection.ElementAt(_rnd.Next(0, collection.Count()));
        }
    }
}
