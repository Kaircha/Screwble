using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
  [Header("UI")]
  public TextMeshProUGUI LetterUI;
  public TextMeshProUGUI PointsUI;
  public Image Target;
  public Image Image;
  public Image Outline;
  public Button Button;

  [Header("SFX")]
  public AudioSource Source;
  public AudioClip Hover;

  [Header("Value")]
  public char Value;
  [Min(0)] public int Points;
  public Vector2Int Position;

  [ContextMenu("Get Position")]
  public void GetPosition() {
    int index = transform.GetSiblingIndex();
    // 00 to 99
    Position.y = Mathf.FloorToInt(index / 10);
    Position.x = index - 10 * Position.y;
  }

  public void UpdateUI() {
    LetterUI.text = Value.ToString().ToUpper();
    PointsUI.text = Points.ToString();
  }

  public void Disable() {
    Target.raycastTarget = false;
    Button.interactable = false;
  }

  public void Enable() {
    Target.raycastTarget = true;
    Button.interactable = true;
  }

  public void Set(LetterPointsPair lpp) => Set(lpp.value, lpp.points);
  public void Set(char c, int p) {
    Value = c;
    Points = p;
    UpdateUI();
  }


  public bool IsLegalChoice() {
    if (GameManager.Instance.Letters.Count == 0) return true;

    Vector2Int last = GameManager.Instance.Letters[^1].Position;
    Vector2Int diff = Position - last;

    if (diff.x > 1 || diff.x < -1 || diff.y > 1 || diff.y < -1) return false;
    else return true;
  }


  private bool IsClicked;
  public void OnPointerDown(PointerEventData eventData) {
    if (!Target.raycastTarget) return;
    if (IsClicked) return;
    IsClicked = true;
    if (IsLegalChoice()) GameManager.Instance.WordMakeState.OnLetterHovered(this);
  }

  public void OnPointerUp(PointerEventData eventData) => IsClicked = false;
  public void OnPointerExit(PointerEventData eventData) => IsClicked = false;
  public void OnPointerEnter(PointerEventData eventData) {
    if (!Target.raycastTarget) return;
    Source.PlayOneShot(Hover);
    if (IsLegalChoice() && Input.GetMouseButton(0)) GameManager.Instance.WordMakeState.OnLetterHovered(this);
  }
}