using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
  public List<Letter> Letters;

  public void Awake() => Letters.ForEach(x => x.Set(global::Letters.GetRandom()));

  [ContextMenu("Reroll")]
  public void Reroll() {
    foreach (Letter letter in Letters) {
      letter.Set(global::Letters.GetRandom());
    }
  }

  [ContextMenu("Enable All")]
  public void EnableAll() => Letters.ForEach(x => x.Enable());
  
  [ContextMenu("Disable All")]
  public void DisableAll() => Letters.ForEach(x => x.Disable());
}