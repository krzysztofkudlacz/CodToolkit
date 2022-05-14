using System;
using System.Collections.Generic;
using System.Linq;

namespace CodToolkit.Extensions
{
    public static class CollectionExtensions
    {
        public static IReadOnlyCollection<TResult> CartesianProduct<T1, T2, TResult>(
            this IReadOnlyCollection<T1> collection1,
            IReadOnlyCollection<T2> collection2,
            Func<T1, T2, TResult> creator)
        {
            var cartesianProduct = new List<TResult>(
                collection1.Count * collection2.Count);

            cartesianProduct.AddRange(
                collection1
                    .SelectMany(
                        e1 => collection2
                            .Select(e2 => creator(e1, e2))));

            return cartesianProduct;
        }
    }
}
