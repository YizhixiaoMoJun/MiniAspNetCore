using System;
using System.Collections.Generic;

namespace MiniAspNetCore
{
    public interface IFeatureCollection : IDictionary<Type,object>
    {
         
    }

    public static class FeatureCollectionExtenstion
    {
         public static T GetService<T>(this IFeatureCollection features) 
        {
            return features.TryGetValue(typeof(T), out var value)
                ? (T)value
                : default;
        }

        public static void SetService<T>(this IFeatureCollection features, T feature)
        {
            features[typeof(T)] = feature;
        }
    }
}