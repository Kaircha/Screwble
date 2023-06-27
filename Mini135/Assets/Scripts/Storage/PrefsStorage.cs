using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PrefsStorage uses Unity's PlayerPrefs data storage system.
/// Due to limitations, this doesn't allow for the loading or deleting of individual slots;
/// Instead, slots are distinguished through prefixes in the accessor key.
/// </summary>
public class PrefsStorage : Storage {
  private string Prefix => "SaveData";

  public PrefsStorage() { }

  public override void Save() => PlayerPrefs.Save();
  public override void Load() { }
  public override void Delete() => PlayerPrefs.DeleteAll();

  public override bool Has(string key) => PlayerPrefs.HasKey(Prefix + key);
  public override int GetInt(string key, int fallback) => PlayerPrefs.GetInt(Prefix + key, fallback);
  public override bool GetBool(string key, bool fallback) => PlayerPrefs.GetInt(Prefix + key, fallback ? 1 : 0) == 1;
  public override float GetFloat(string key, float fallback) => PlayerPrefs.GetFloat(Prefix + key, fallback);
  public override string GetString(string key, string fallback) => PlayerPrefs.GetString(Prefix + key, fallback);
  public override Vector2 GetVector2(string key, Vector2 fallback) => 
    new(PlayerPrefs.GetFloat($"{Prefix + key}_x", fallback.x), 
        PlayerPrefs.GetFloat($"{Prefix + key}_y", fallback.y));
  public override Vector3 GetVector3(string key, Vector3 fallback) => 
    new(PlayerPrefs.GetFloat($"{Prefix + key}_x", fallback.x), 
        PlayerPrefs.GetFloat($"{Prefix + key}_y", fallback.y), 
        PlayerPrefs.GetFloat($"{Prefix + key}_z", fallback.z));

  public override void SetInt(string key, int value) => PlayerPrefs.SetInt(Prefix + key, value);
  public override void SetBool(string key, bool value) => PlayerPrefs.SetInt(Prefix + key, value ? 1 : 0);
  public override void SetFloat(string key, float value) => PlayerPrefs.SetFloat(Prefix + key, value);
  public override void SetString(string key, string value) => PlayerPrefs.SetString(Prefix + key, value);
  public override void SetVector2(string key, Vector2 value) {
    PlayerPrefs.SetFloat($"{Prefix + key}_x", value.x);
    PlayerPrefs.SetFloat($"{Prefix + key}_y", value.y);
  }
  public override void SetVector3(string key, Vector3 value) {
    PlayerPrefs.SetFloat($"{Prefix + key}_x", value.x);
    PlayerPrefs.SetFloat($"{Prefix + key}_y", value.y);
    PlayerPrefs.SetFloat($"{Prefix + key}_z", value.z);
  }
}