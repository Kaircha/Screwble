using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions {
  public static string Capitalize(this string value) {
    if (string.IsNullOrEmpty(value)) return value;

    if (value.Length == 1) return value.ToUpper();
    else return $"{value[0].ToString().ToUpper()}{value[1..]}";
  }
}