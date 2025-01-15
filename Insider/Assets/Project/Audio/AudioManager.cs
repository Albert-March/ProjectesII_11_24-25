using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------Audio Source------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

	[Header("------Audio Clips------")]
	public AudioClip music;
	public AudioClip death;
	public AudioClip towerShot;
	public AudioClip selectTower;

	private void Start()
	{
		musicSource.clip = music;
		musicSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		SFXSource.PlayOneShot(clip);
	}
}

