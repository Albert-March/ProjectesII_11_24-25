using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
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

	public static AudioManager instance;
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	private void Start()
	{
		musicSource.clip = music;
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
	//5 = EnemyHit
}

