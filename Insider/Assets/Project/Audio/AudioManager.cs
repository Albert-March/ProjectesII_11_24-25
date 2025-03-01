using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

	[Header("------Music Clip------")]
	public AudioClip music;

	[Header("------SFX Clips------")]
	public List<AudioClip> SFXClips = new List<AudioClip>();

	[Header("------UI Elements------")]
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	private void Start()
	{
		// Iniciar la música
		musicSource.clip = music;
		musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
		musicSource.Play();

		// Cargar volúmenes previos
		SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

		// Asignar sliders
		if (musicSlider != null)
		{
			musicSlider.value = musicSource.volume;
			musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value, SFXSource.volume); });
		}

		if (sfxSlider != null)
		{
			sfxSlider.value = SFXSource.volume;
			sfxSlider.onValueChanged.AddListener(delegate { SetVolume(musicSource.volume, sfxSlider.value); });
		}
	}

	public void PlaySFX(int index, float volume)
	{
		if (index >= 0 && index < SFXClips.Count)
		{
			SFXSource.volume = volume;
			SFXSource.PlayOneShot(SFXClips[index]);
		}
	}
	//0 = EnemyDeath
	//1 = TowerShot
	//2 = SelectTower
	//3 = SelectButton
	//4 = UpgradeTower

	public void SetVolume(float musicVolume, float sfxVolume)
	{
		musicSource.volume = musicVolume;
		SFXSource.volume = sfxVolume;

		// Guardar valores para la próxima vez que inicie el juego
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
		PlayerPrefs.Save();
	}
}

