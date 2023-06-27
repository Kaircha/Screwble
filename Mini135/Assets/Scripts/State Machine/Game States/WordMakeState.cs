using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;


public class WordMakeState : State {
  public List<Letter> Letters => GameManager.Instance.Letters;
  public AudioClip WordMakeMusic;
  [HideInInspector] public bool IsHeld;
  [HideInInspector] public bool IsFinished;
  public CinemachineVirtualCamera VCam;
  public TextMeshProUGUI TurnUI;
  public TextMeshProUGUI WordUI;
  public TextMeshProUGUI IsRealUI;
  public TextMeshProUGUI MeaningUI;
  public LineRenderer Line;
  public Board Board;

  public override void Enter() {
    IsFinished = false;

    AudioManager.Instance.PlayMusic(null, WordMakeMusic);
    IsRealUI.text = "";
    MeaningUI.text = "";
    foreach (Letter letter in Letters) {
      letter.Image.color = Color.white;
      letter.Set(global::Letters.GetRandom());
    }
    Letters.Clear();
    Line.positionCount = 0;
    Line.SetPositions(new Vector3[0]);
    Board.EnableAll();
  }

  public override IEnumerator Execute() {
    yield return SlideCamera();
    yield return MakeWord();
  }

  public override void Exit() {
    VCam.Priority = 0;
    Line.positionCount = 0;
    Line.SetPositions(new Vector3[0]);
    Board.DisableAll();
  }

  public IEnumerator SlideCamera() {
    VCam.Priority = 10;
    yield return new WaitForSeconds(1f);
  }

  public IEnumerator MakeWord() {
    TurnUI.text = $"{GameManager.Instance.WordPlayer.Name}'s Word";
    WordUI.text = "";
    Letters.Clear();

    while (true) {
      yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
      IsHeld = true;

      yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
      IsHeld = false;

      Letters.ForEach(x => x.Outline.gameObject.SetActive(false));
      if (Letters.Count > 1) break;
      else {
        Letters.Clear();
        WordUI.text = "";
        Line.positionCount = 0;
        Line.SetPositions(new Vector3[0]);
      }
    }

    IsFinished = true;
  }

  public void OnLetterHovered(Letter letter) {
    if (!Letters.Contains(letter)) {
      letter.Outline.gameObject.SetActive(true);
      Letters.Add(letter);
    } else if (Letters.Count > 1 && Letters[^2] == letter) {
      Letter toRemove = Letters[^1];
      toRemove.Outline.gameObject.SetActive(false);
      Letters.Remove(toRemove);
    }

    Line.positionCount = Letters.Count;
    Line.SetPositions(Letters.Select(x => x.transform.position).ToArray());
    WordUI.text = new string(Letters.Select(x => x.Value).ToArray());
  }
}