using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("..........Audio Sources..........")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("..........Audio Clip..........")]
    public AudioClip inGameMusic;
    public AudioClip jumpSound;
    public AudioClip pauseSound;
    public AudioClip winSound;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       musicSource.clip = inGameMusic;
       musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
