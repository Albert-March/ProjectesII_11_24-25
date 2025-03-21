using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
	[Header("------UI Elements------")]
	[SerializeField] private AudioMixer myMixer;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider SFXSlider;
	private void Start()
	{
		if (PlayerPrefs.HasKey("musicVolume"))
		{
			LoadValue();
		}
		else
		{
			SetMusicVolume();
			SetSFXVolume();
		}
	}

	public void SetMusicVolume()
	{
		float musicVolume = musicSlider.value;
		myMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
		PlayerPrefs.SetFloat("musicVolume", musicVolume);
	}
	public void SetSFXVolume()
	{
		float SFXVolume = SFXSlider.value;
		myMixer.SetFloat("SFX", Mathf.Log10(SFXVolume) * 20);
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
	}

	public void LoadValue()
	{
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
		SetMusicVolume();

		SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
		SetSFXVolume();
	}
}
