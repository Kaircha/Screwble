using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {
  public AudioSource Source;
  public AudioClip ClickSFX;
  public AudioClip HoverSFX;

  public void OnPointerClick(PointerEventData eventData) => Source.PlayOneShot(ClickSFX);
  public void OnPointerEnter(PointerEventData eventData) => Source.PlayOneShot(HoverSFX);
}