using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager> {
  [Header("States")]
  public StateMachine Machine;
  public MainMenuState MainMenuState;
  public InputNamesState InputNamesState;
  public WordMakeState WordMakeState;
  public WordJudgeState WordJudgeState;
  public DeathState DeathState;

  [Header("Players")]
  public Player P1;
  public Player P2;
  public List<Player> Players => new() { P1, P2 };
  public Player GuessPlayer => Players.First(x => x.IsGuessing);
  public Player WordPlayer => Players.First(x => !x.IsGuessing);

  [Header("Score")]
  public TextMeshProUGUI P1ScoreUI;
  public TextMeshProUGUI P2ScoreUI;

  [Header("Letters")]
  public List<Letter> Letters;

  public void Start() => StartCoroutine(MainMenu());

  public IEnumerator MainMenu() {
    Machine.ChangeState(MainMenuState);
    yield return null;
    yield return new WaitUntil(() => MainMenuState.IsFinished);
    StartCoroutine(GameplayLoop());
  }

  public IEnumerator GameplayLoop() {
    Machine.ChangeState(InputNamesState);
    yield return null;
    yield return new WaitUntil(() => InputNamesState.IsFinished);

    P1.IsGuessing = true;
    P2.IsGuessing = false;
    Letters = new();

    while (P1.IsAlive && P2.IsAlive) {
      P1.IsGuessing = !P1.IsGuessing;
      P2.IsGuessing = !P2.IsGuessing;

      Machine.ChangeState(WordMakeState);
      yield return null;
      yield return new WaitUntil(() => WordMakeState.IsFinished);

      Machine.ChangeState(WordJudgeState);
      yield return null;
      yield return new WaitUntil(() => WordJudgeState.IsFinished);
    }

    Machine.ChangeState(DeathState);
  }

  public void GuessPlayerGainsPoints() {
    GuessPlayer.Points += Letters.Sum(x => x.Points);
    P1ScoreUI.text = $"${P1.Points}";
    P2ScoreUI.text = $"${P2.Points}";
  }

  public void WordPlayerGainsPoints() {
    WordPlayer.Points += Letters.Sum(x => x.Points);
    P1ScoreUI.text = $"${P1.Points}";
    P2ScoreUI.text = $"${P2.Points}";
  }

  public void PlayerGetsShot() => WordPlayer.GetShot();
}