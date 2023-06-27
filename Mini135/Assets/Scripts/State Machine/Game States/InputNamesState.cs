using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class InputNamesState : State {
  public CinemachineVirtualCamera VCam;
  public Image SubmitButton;
  [HideInInspector] public bool IsFinished;
  private string P1Name;
  private string P2Name;

  public override void Enter() {
    VCam.Priority = 10;
    SubmitButton.raycastTarget = true;
    IsFinished = false;
  }

  public override void Exit() {
    SubmitButton.raycastTarget = false;
    VCam.Priority = 0;
  }

  public void WriteP1Name(string name) => P1Name = name;
  public void WriteP2Name(string name) => P2Name = name;

  public void ConfirmNames() {
    if (string.IsNullOrEmpty(P1Name) || string.IsNullOrEmpty(P2Name)) {
      Debug.Log("Names aren't filled in!");
      return;
    }

    GameManager.Instance.P1 = new() { Name = P1Name, IsAlive = true, Points = 0 };
    GameManager.Instance.P2 = new() { Name = P2Name, IsAlive = true, Points = 0 };
    IsFinished = true;
  }
}