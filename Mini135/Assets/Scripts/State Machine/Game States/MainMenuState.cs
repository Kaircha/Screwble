using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainMenuState : State {
  public CinemachineVirtualCamera VCam;
  public GameObject MainMenuCanvas;
  public Volume PostProcessing;
  public AudioClip MainMenuMusic;
  [HideInInspector] public bool IsFinished;
  private bool IsPlay;
  private DepthOfField DoF;
  public TextMeshProUGUI DatabaseStatusUI;
  public Board Board;

  public override void Enter() {
    VCam.Priority = 10;
    AudioManager.Instance.PlayMusic(null, MainMenuMusic);
    if (PostProcessing.profile.TryGet(out DepthOfField dof)) DoF = dof;
    DoF.focalLength.value = 300;
    MainMenuCanvas.SetActive(true);
    IsFinished = false;
    DatabaseStatusUI.text = "Database: Listening..";
    DatabaseStatusUI.color = Color.white;
    Board.DisableAll();
  }

  public override IEnumerator Execute() {
    yield return Thesaurus.CheckDatabaseStatus((value) => {
      if (value) {
        DatabaseStatusUI.text = "Database: Online";
        DatabaseStatusUI.color = Color.green;
      } else {
        DatabaseStatusUI.text = "Database: Offline";
        DatabaseStatusUI.color = Color.red;
      }
    });

    IsPlay = false;
    yield return new WaitUntil(() => IsPlay);
    yield return CloseMainMenu();
    IsFinished = true;
  }

  public override void Exit() {
    MainMenuCanvas.SetActive(false);
    VCam.Priority = 0;
  }

  public IEnumerator CloseMainMenu() {
    MainMenuCanvas.SetActive(false);
    for (float timer = 0; timer < 1f; timer += 2f * Time.deltaTime) {
      DoF.focalLength.value = (1 - timer) * 300;
      yield return null;
    }
    DoF.focalLength.value = 0;
  }

  public void PlayButton() => IsPlay = true;
}