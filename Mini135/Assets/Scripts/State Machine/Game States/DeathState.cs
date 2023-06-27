using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Linq;

public class DeathState : State {
  public CinemachineVirtualCamera VCam;
  public TextMeshProUGUI WhoDiedUI;
  public TextMeshProUGUI FakeWordUI;
  public TextMeshProUGUI P1Score;
  public TextMeshProUGUI P2Score;
  public Image MeaningInput;
  public Image SubmitButton;
  public Image DeathScreen;
  public Image ReplayBtn;
  public Image MenuButton;

  private string FakeWord;
  private string FakeDefinition;
  private bool IsSubmit;
  private (bool hasChosen, bool Choice) MainMenu;

  public override void Enter() {
    FakeWord = new string(GameManager.Instance.Letters?.Select(x => x.Value).ToArray());
    MeaningInput.raycastTarget = true;
    SubmitButton.raycastTarget = true;
    ReplayBtn.raycastTarget = true;
    MenuButton.raycastTarget = true;
  }

  public override IEnumerator Execute() {
    P1Score.text = "$0";
    P2Score.text = "$0";
    ShowWinner();
    ShowDeathScreen();
    VCam.Priority = 10;
    yield return new WaitForSeconds(5f);
    yield return SubmitWord();
    yield return WhatNext();
  }

  public override void Exit() {
    MeaningInput.raycastTarget = false;
    SubmitButton.raycastTarget = false;
    ReplayBtn.raycastTarget = false;
    MenuButton.raycastTarget = false;
    WhoDiedUI.gameObject.SetActive(false);
    FakeWordUI.gameObject.SetActive(false);
    MeaningInput.gameObject.SetActive(false);
    SubmitButton.gameObject.SetActive(false);
    ReplayBtn.gameObject.SetActive(false);
    MenuButton.gameObject.SetActive(false);
    Color color = DeathScreen.color;
    DeathScreen.color = new Color(color.r, color.g, color.b, 0f);
    VCam.Priority = 0;
  }

  public void ShowWinner() {
    WhoDiedUI.gameObject.SetActive(true);
    FakeWordUI.gameObject.SetActive(true);
    WhoDiedUI.text = $"{GameManager.Instance.WordPlayer.Name} got shot!";
    FakeWordUI.text = $"{FakeWord} was a fake word";
  }

  public void ShowDeathScreen() {
    Color color = DeathScreen.color;
    DeathScreen.color = new Color(color.r, color.g, color.b, 1f);
  }

  public IEnumerator SubmitWord() {
    Player winner = GameManager.Instance.P1.Points > GameManager.Instance.P2.Points ? GameManager.Instance.P1 : GameManager.Instance.P2;
    WhoDiedUI.text = $"{winner.Name} won with ${winner.Points}";
    FakeWordUI.text = $"but what does {FakeWord} mean?";
    MeaningInput.gameObject.SetActive(true);
    SubmitButton.gameObject.SetActive(true);

    IsSubmit = false;
    yield return new WaitUntil(() => IsSubmit == true);
    yield return Thesaurus.PostDatabaseWordRoutine(new() { playerName = winner.Name, word = FakeWord, definition = FakeDefinition });
  }

  public IEnumerator WhatNext() {
    MainMenu = (false, false);

    WhoDiedUI.text = "Thanks for playing, Cowboy";
    FakeWordUI.gameObject.SetActive(false);
    MeaningInput.gameObject.SetActive(false);
    SubmitButton.gameObject.SetActive(false);
    ReplayBtn.gameObject.SetActive(true);
    MenuButton.gameObject.SetActive(true);

    yield return new WaitUntil(() => MainMenu.hasChosen);

    if (MainMenu.Choice) GameManager.Instance.StartCoroutine(GameManager.Instance.MainMenu());
    else GameManager.Instance.StartCoroutine(GameManager.Instance.GameplayLoop());
  }

  public void ChangeDefinition(string definition) => FakeDefinition = definition;
  public void SubmitWordButton() => IsSubmit = !string.IsNullOrEmpty(FakeDefinition);
  public void MainMenuButton() => MainMenu = (true, true);
  public void ReplayButton() => MainMenu = (true, false);
}