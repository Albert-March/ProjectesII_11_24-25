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
    [SerializeField] AudioSource SFXSourcePrefab;

    [Header("------Music Clip------")]
    public AudioClip music;

    [Header("------SFX Clips------")]
    public List<AudioClip> SFXClips = new List<AudioClip>();

    public static AudioManager instance;

    private List<AudioSource> sfxPool = new List<AudioSource>();
    private int poolSize = 30;

    private Dictionary<int, AudioSource> loopingSFX = new Dictionary<int, AudioSource>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSFXPool();
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

    private void CreateSFXPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource sfx = Instantiate(SFXSourcePrefab, transform);
            sfx.playOnAwake = false;
            sfxPool.Add(sfx);
        }
    }

    public void PlaySFX(int index, float volume)
    {
        if (index >= 0 && index < SFXClips.Count)
        {
            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.clip = SFXClips[index];
                availableSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                availableSource.volume = volume * UnityEngine.Random.Range(0.9f, 1.0f);
                availableSource.Play();
            }
        }
    }
    public void PlaySFX_P(int index, float volume, float pitch)
    {
        if (index >= 0 && index < SFXClips.Count)
        {
            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.clip = SFXClips[index];
                availableSource.pitch = pitch * UnityEngine.Random.Range(0.95f, 1.05f);
                availableSource.volume = volume * UnityEngine.Random.Range(0.9f, 1.0f);
                availableSource.Play();
            }
        }
    }

    public AudioSource PlaySFXLoopUnique(int index, float volume)
    {
        if (index >= 0 && index < SFXClips.Count)
        {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.Stop();
                source.clip = SFXClips[index];
                source.volume = volume;
                source.loop = true;
                source.pitch = 1f;
                source.Play();
                return source; // <<< Important: retorna'l!
            }
        }
        return null;
    }

    public void SetLoopPitch(int index, float pitch)
    {
        if (loopingSFX.ContainsKey(index))
        {
            AudioSource source = loopingSFX[index];
            source.pitch = pitch;
        }
    }

    public void StopSFXLoop(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            source.loop = false;
        }
    }

    public bool IsLoopPlaying(int index)
    {
        if (loopingSFX.ContainsKey(index))
        {
            return loopingSFX[index].isPlaying;
        }
        return false;
    }

    private AudioSource GetAvailableSFXSource()
    {
        for (int i = 0; i < sfxPool.Count; i++)
        {
            if (!sfxPool[i].isPlaying)
            {
                return sfxPool[i];
            }
        }

        // Si totes estan ocupades, recicla la primera
        return sfxPool[0];
    }

    // 0 = EnemyDeath
    // 1 = TowerShot
    // 2 = SelectTower
    // 3 = SelectButton
    // 4 = UpgradeTower
    // 5 = TowerSpawn
    // 6 = LvlStart
    // 7 = LaserType0Shoot
    // 8 = AttackBopper
    // 9 = PuddleDMG
    // 10 = Bomb
    // 11 = AttackCanoner
    // 12 = Chain
    // 13 = LaserLoop
}

