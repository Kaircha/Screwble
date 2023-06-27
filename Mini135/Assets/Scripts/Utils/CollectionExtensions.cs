using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtensions {
  public static T GetRandom<T>(this List<T> collection, T fallback, T exclude) {
    if (collection == null || collection.Count == 0) return fallback;
    return GetRandom(collection, exclude);
  }
  public static T GetRandom<T>(this List<T> collection, T exclude) {
    if (exclude == null || !collection.Contains(exclude)) return GetRandom(collection);
    List<T> options = new(collection);
    options.Remove(exclude);
    if (options.Count == 0) {
      Debug.LogWarning($"Excluding {exclude} from {collection} results in an empty collection!");
      return GetRandom(collection);
    } 
    return GetRandom(options);
  }
  public static T GetRandom<T>(this List<T> collection) {
    if (collection == null || collection.Count == 0) return default;
    return collection[Random.Range(0, collection.Count())];
  }
}