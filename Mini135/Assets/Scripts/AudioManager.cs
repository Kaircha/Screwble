using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEditor;

/// <summary>
/// The AudioManager requires both Singleton and DataManager scripts to function. <br></br> <br></br>
/// 
/// Additionally, the project must be set up with an AudioMixer, containing a Master, Music and SFX Controller. <br></br>
/// Each of which with their volume parameter exposed as MasterVolume, MusicVolume and SFXVolume respectively. <br></br> <br></br>
/// 
/// The volume parameter is in Decibel values, calculated as 20 * Log10(Percent), with Percent being a range of 0.0001f to 1f. <br></br>
/// This range doesn't start at 0 as to not break the Log10 calculation. Optionally, adjust the Sliders' minimum values to 0.0001f.
/// </summary>
public class AudioManager : Singleton<AudioManager> {
  public AudioMixerGroup AudioMixerGroup;
  public AudioSource Active;
  public AudioSource Inactive;
  [Range(0.1f, 2f)] public float CrossfadeDuration = 0.5f;

  public Slider MusicSlider;
  public Slider SFXSlider;

  // Contains all audio channels in the AudioMixerGroup
  public static readonly string[] Channels = { "MasterVolume", "MusicVolume", "SFXVolume", "UIVolume" };

  private void Start() {
    MusicSlider.value = DataManager.Instance.GetFloat("MusicVolume", 1f);
    SFXSlider.value = DataManager.Instance.GetFloat("MusicVolume", 1f);

    foreach (string channel in Channels)
      SetVolume(channel,DataManager.Instance.GetFloat(channel, 1f));
  }

  public void MasterVolume(float value) => SetVolume("MasterVolume", value);
  public void MusicVolume(float value) => SetVolume("MusicVolume", value);
  public void SFXVolume(float value) => SetVolume("SFXVolume", value);
  public void UIVolume(float value) => SetVolume("UIVolume", value);
  public void SetVolume(string channel = "MasterVolume", float value = 1f) {
    value = Mathf.Clamp(value, 0.0001f, 1f);
    DataManager.Instance.SetFloat(channel, value);
    AudioMixerGroup.audioMixer.SetFloat(channel, Mathf.Log10(value) * 20);
  }

  public void PlayMusic(AudioClip start, AudioClip loop) {
    if (loop == null) return;
    if (loop == Active.clip) return;
    Inactive.clip = loop;
    if (start == null) Inactive.Play();
    else {
      Inactive.PlayOneShot(start);
      Inactive.PlayScheduled(AudioSettings.dspTime + start.length);
    }
    Crossfade();
  }

  private void Crossfade() {
    Fadeout(Active);
    Fadein(Inactive);
    (Active, Inactive) = (Inactive, Active);
  }

  private void Fadein(AudioSource audio) => StartCoroutine(FadeinRoutine(audio));
  private void Fadeout(AudioSource audio) => StartCoroutine(FadeoutRoutine(audio));

  private IEnumerator FadeinRoutine(AudioSource audio) {
    if (CrossfadeDuration <= 0f) CrossfadeDuration = 0.1f;
    while (audio.volume < 1f) {
      audio.volume += Time.deltaTime / CrossfadeDuration;
      yield return null;
    }
  }

  private IEnumerator FadeoutRoutine(AudioSource audio) {
    if (CrossfadeDuration <= 0f) CrossfadeDuration = 0.1f;
    while (audio.volume > 0f) {
      audio.volume -= Time.deltaTime / CrossfadeDuration;
      yield return null;
    }
    audio.Stop();
    audio.clip = null;
  }
}