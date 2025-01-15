using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

	[Header("------Music Clip------")]
	public AudioClip music;

	[Header("------SFX Clips------")]
	public List<AudioClip> SFXClips = new List<AudioClip>();

	private void Start()
	{
		musicSource.clip = music;
		musicSource.volume = 0.2f;
		musicSource.Play();
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
}

