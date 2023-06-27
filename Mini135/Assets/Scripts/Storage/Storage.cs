using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Storage {
  public abstract void Save();
  public abstract void Load();
  public abstract void Delete();
  public abstract bool Has(string key);
  // public abstract bool TryGet<T>(string key, out T value);
  // public abstract T Get<T>(string key);
  // public abstract void Set<T>(string id, T value);

  public abstract int GetInt(string key, int fallback = 0);
  public abstract bool GetBool(string key, bool fallback = false);
  public abstract float GetFloat(string key, float fallback = 0f);
  public abstract string GetString(string key, string fallback = "");
  public abstract Vector2 GetVector2(string key, Vector2 fallback = new());
  public abstract Vector3 GetVector3(string key, Vector3 fallback = new());

  public abstract void SetInt(string key, int value);
  public abstract void SetBool(string key, bool value);
  public abstract void SetFloat(string key, float value);
  public abstract void SetString(string key, string value);
  public abstract void SetVector2(string key, Vector2 value);
  public abstract void SetVector3(string key, Vector3 value);
}