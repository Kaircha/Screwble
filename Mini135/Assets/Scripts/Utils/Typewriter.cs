using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : MonoBehaviour {
  private TextMeshProUGUI UI;

  public void Awake() => UI = GetComponent<TextMeshProUGUI>();

  public void Clear() {
    StopAllCoroutines();
    UI.text = "";
  }

  public void Write(string text) => UI.text = text;
  public void Write(string text, float duration) {
    if (duration <= 0 || text.Length <= 0) UI.text = text;
    else StartCoroutine(WriteRoutine(text, duration));
  }

  private IEnumerator WriteRoutine(string text, float duration) {
    UI.text = "";
    float interval = duration / text.Length;
    foreach (char c in text) {
      yield return new WaitForSeconds(interval);
      UI.text += c;
    }
  }
}