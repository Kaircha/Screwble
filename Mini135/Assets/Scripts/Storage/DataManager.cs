using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager> {
  public Storage Storage;

  public override void Awake() {
    base.Awake();
    InitAll();
    LoadAll();
  }
  private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
  private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => SaveAll();
  private void OnApplicationQuit() => SaveAll();

  public void SaveAll() {
    Storage.Save();
  }
  public void InitAll() {
    Storage = new PrefsStorage();
  }
  public void LoadAll() {
    Storage.Load();
  }

  public int GetInt(string key, int fallback = 0) => Storage.GetInt(key, fallback);
  public bool GetBool(string key, bool fallback = false) => Storage.GetBool(key, fallback);
  public float GetFloat(string key, float fallback = 0f) => Storage.GetFloat(key, fallback);
  public string GetString(string key, string fallback = "") => Storage.GetString(key, fallback);
  public Vector2 GetVector2(string key, Vector2 fallback = new()) => Storage.GetVector2(key, fallback);
  public Vector3 GetVector3(string key, Vector3 fallback = new()) => Storage.GetVector3(key, fallback);

  public void SetInt(string key, int value) => Storage.SetInt(key, value);
  public void SetBool(string key, bool value) => Storage.SetBool(key, value);
  public void SetFloat(string key, float value) => Storage.SetFloat(key, value);
  public void SetString(string key, string value) => Storage.SetString(key, value);
  public void SetVector2(string key, Vector2 value) => Storage.SetVector2(key, value);
  public void SetVector3(string key, Vector3 value) => Storage.SetVector3(key, value);
}