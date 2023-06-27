using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class WordJudgeState : State {
  public CinemachineVirtualCamera VCam;
  public AudioClip WordJudgeMusic;
  [HideInInspector] public bool IsFinished;
  public (bool isChosen, bool choice) IsRealCheck;
  public ThesaurusEntry? ThesaurusEntry;
  public DatabaseEntry? DatabaseEntry;
  public List<Letter> Letters => GameManager.Instance.Letters;
  public TextMeshProUGUI TurnUI;
  public TextMeshProUGUI IsRealUI;
  public TextMeshProUGUI MeaningUI;
  public Image Gun;
  public Image Drink;

  public AudioSource Source;
  public List<AudioClip> SipSFX;
  public AudioClip ClickSFX;
  public AudioClip ShootSFX;
  public AudioClip CoughSFX;
  public AudioClip RiserSFX;

  private string Word;

  public override void Enter() {
    Gun.raycastTarget = true;
    Drink.raycastTarget = true;
    AudioManager.Instance.PlayMusic(null, WordJudgeMusic);
    Word = new string(Letters?.Select(x => x.Value).ToArray());
    IsFinished = false;
  }

  public override IEnumerator Execute() {
    yield return SlideCamera();
    yield return CheckWord();
    yield return JudgeWord();
  }

  public override void Exit() {
    Gun.raycastTarget = false;
    Drink.raycastTarget = false;
    VCam.Priority = 0;
    AudioManager.Instance.Active.volume = 1f;
    ThesaurusEntry = null;
  }

  public IEnumerator SlideCamera() {
    VCam.Priority = 10;
    yield return new WaitForSeconds(1f);
  }

  public IEnumerator CheckWord() {
    if (Letters.Count > 1) yield return Thesaurus.RequestDictionaryWordRoutine(Word, (value) => ThesaurusEntry = value);
    else ThesaurusEntry = null;
  }

  public IEnumerator JudgeWord() {
    TurnUI.text = $"{GameManager.Instance.GuessPlayer.Name}'s Guess";
    IsRealUI.text = "Is it a real word?";

    IsRealCheck = (false, false);
    yield return new WaitUntil(() => IsRealCheck.isChosen);

    Source.PlayOneShot(RiserSFX);
    yield return new WaitForSeconds(1f);
    for (float timer = 0; timer < 1f; timer += Time.deltaTime) {
      AudioManager.Instance.Active.volume = Mathf.Lerp(1f, 0f, timer);
      yield return null;
    }
    AudioManager.Instance.Active.volume = 0f;
    yield return new WaitForSeconds(3f);

    if (ThesaurusEntry != null) {
      IsRealUI.text = "It IS a real word!";
      Word word = ThesaurusEntry.Value.words.GetRandom();
      Meaning meaning = word.meanings.GetRandom();
      MeaningUI.text = $"{word.word} ({meaning.partOfSpeech}) - {meaning.definitions.GetRandom().definition}";

      if (IsRealCheck.choice) {
        Source.PlayOneShot(SipSFX.GetRandom());
        GameManager.Instance.GuessPlayerGainsPoints(); // P1 writes a real word. P2 says it's real
      } else {
        Source.PlayOneShot(ClickSFX);
        GameManager.Instance.WordPlayerGainsPoints(); // P1 writes a real word. P2 says it's fake
      }

      yield return new WaitForSeconds(4f);
    } else {
      yield return Thesaurus.RequestDatabaseWordRoutine(Word, (value) => DatabaseEntry = value);

      // P1 writes a fake word. P2 says it's fake. P1 gets shot.
      if (!IsRealCheck.choice && DatabaseEntry == null) {
        Source.PlayOneShot(ShootSFX);
        GameManager.Instance.GuessPlayerGainsPoints();
        GameManager.Instance.PlayerGetsShot();
      } else {
        // The word is in the database!
        IsRealUI.text = "It's NOT a real word!";
        MeaningUI.text = "";
        if (IsRealCheck.choice) {  // P1 writes a fake word. P2 says it's real
          Source.PlayOneShot(CoughSFX);
          GameManager.Instance.WordPlayerGainsPoints();
        } else { // P1 writes a fake word. P2 says it's fake. P1 was saved by the database
          Source.PlayOneShot(ClickSFX);
          GameManager.Instance.GuessPlayerGainsPoints();
        }
        yield return new WaitForSeconds(2f);
        if (DatabaseEntry != null) {
          IsRealUI.text = $"Wait! {Word} does exist?";
          MeaningUI.text = $"{DatabaseEntry.Value.definition}<br><br>- Submitted by {DatabaseEntry.Value.playerName}!";
          yield return new WaitForSeconds(4f);
        }
      }

    }

    IsFinished = true;
  }

  public void MakeChoice(bool choice) => IsRealCheck = (true, choice);
}