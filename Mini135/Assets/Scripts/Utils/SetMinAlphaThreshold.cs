using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMinAlphaThreshold : MonoBehaviour {
  public void Start() {
    if (TryGetComponent(out Image image)) image.alphaHitTestMinimumThreshold = 0.5f;
  }
}