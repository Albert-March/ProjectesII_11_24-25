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
	public AudioClip selectButton;
	public AudioClip collectReward;
	public AudioClip upgradeTower;

	private void Start()
	{
		musicSource.clip = music;
		musicSource.volume = 0.2f;
		musicSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		SFXSource.PlayOneShot(clip);
	}
}

