using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
  private Letter Letter;

  public bool IsFilled => Letter != null;

  public void Fill(Letter letter) {
    Letter = letter;
    letter.transform.localPosition = Vector3.zero;
  }
}