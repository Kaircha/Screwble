using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

/// <summary>
/// Made possible by: https://dictionaryapi.dev/
/// </summary>
public static class Thesaurus {
  const string DICTIONARY_URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";
  //const string DATABASE_URL = "http://localhost:5048/";
  const string DATABASE_URL = "http://screwble.rifthoppers.com/";

  public static IEnumerator CheckDatabaseStatus(Action<bool> callback) {
    using UnityWebRequest webRequest = UnityWebRequest.Get($"{DATABASE_URL}?word=owlbear");
    webRequest.certificateHandler = new BypassCertificate();
    yield return webRequest.SendWebRequest();

    callback(webRequest.result switch {
      UnityWebRequest.Result.Success => true,
      _ => false,
    });
  }

  public static IEnumerator RequestDictionaryWordRoutine(string word, Action<ThesaurusEntry?> callback) {
    using UnityWebRequest webRequest = UnityWebRequest.Get($"{DICTIONARY_URL}{word.ToLower()}");
    webRequest.certificateHandler = new BypassCertificate();
    yield return webRequest.SendWebRequest();

    callback(webRequest.result switch {
      UnityWebRequest.Result.Success => JsonUtility.FromJson<ThesaurusEntry>("{\"words\":" + webRequest.downloadHandler.text + "}"),
      _ => null,
    });
  }

  public static IEnumerator RequestDatabaseWordRoutine(string word, Action<DatabaseEntry?> callback) {
    using UnityWebRequest webRequest = UnityWebRequest.Get($"{DATABASE_URL}?word={word.ToLower()}");
    webRequest.certificateHandler = new BypassCertificate();
    yield return webRequest.SendWebRequest();

    callback(webRequest.result switch {
      UnityWebRequest.Result.Success => JsonUtility.FromJson<DatabaseEntry>(webRequest.downloadHandler.text),
      _ => null,
    });
  }

  public static IEnumerator PostDatabaseWordRoutine(DatabaseEntry entry) {
    string json = JsonUtility.ToJson(entry);
    byte[] bytes = Encoding.UTF8.GetBytes(json);
    using UnityWebRequest webRequest = UnityWebRequest.Put(DATABASE_URL, bytes);
    webRequest.certificateHandler = new BypassCertificate();
    webRequest.method = "POST";
    webRequest.SetRequestHeader("X-HTTP-Method-Override", "POST");
    webRequest.SetRequestHeader("Content-Type", "application/json");
    yield return webRequest.SendWebRequest();
  }
}

public class BypassCertificate : CertificateHandler {
  protected override bool ValidateCertificate(byte[] certificateData) {
    //Simply return true no matter what
    return true;
  }
}

[Serializable]
public struct DatabaseEntry {
  public string playerName;
  public string word;
  public string definition;
}

[Serializable]
public struct ThesaurusEntry {
  public List<Word> words;
}

[Serializable]
public struct Word {  
  public string word;
  public string phonetic;
  public List<Phonetics> phonetics;
  public string origin;
  public List<Meaning> meanings;
  public List<License> license;
  public List<string> sourceUrls;
}

[Serializable]
public struct Phonetics {
  public string text;
  public string audio;
  public string sourceUrl;
  public License license;
}

[Serializable]
public struct Meaning {
  public string partOfSpeech;
  public List<Definition> definitions;
  public List<string> synonyms;
  public List<string> antonyms;
}

[Serializable]
public struct Definition {
  public string definition;
  public List<string> synonyms;
  public List<string> antonyms;
  public string example;
}

[Serializable]
public struct License {
  public string name;
  public string url;
}