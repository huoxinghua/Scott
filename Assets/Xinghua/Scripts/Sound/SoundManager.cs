using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private string bgmName;
    [SerializeField] private Sound[] bgm, sfx;
    [SerializeField] private AudioSource bgmSource, sfxSource;
    [Range(0f, 1f)][SerializeField] private float bgmVolume = 1.0f;
   // [Range(0f, 1f)][SerializeField] private float sfxVolume = 1.0f;

 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


       // DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PlayBGM("BGM",1f);
    }

    public void PlayBGM(string name, float vol)
    {
        Sound s = Array.Find(bgm, x => x.name == name);

        if (s != null)
        {
            bgmSource.clip = s.clip;
            bgmVolume = vol;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
            bgmSource.loop = true;
        }
        else
        {
            Debug.Log("BGM not found");
        }
    }


    public void PlaySFX(string name, float volume)
    {
        Sound s = Array.Find(sfx, x => x.name == name);

        if (s != null)
        {
            float originalBgmVolume = bgmSource.volume;
            //sfxSource.volume = sfxVolume;
            sfxSource.PlayOneShot(s.clip);
            sfxSource.volume = volume;
        }
        else
        {
            Debug.Log("SFX not found");
        }
    }

    public void StopSFX(string name)
    {
        Sound s = Array.Find(sfx, x => x.name == name);
        if (s != null)
        {
            sfxSource.Stop();
        }
        else
        {
            Debug.Log("SFX not found");
        }
    }

    public void StopBGM(string name)
    {
        Sound s = Array.Find(sfx, x => x.name == name);
        if (s != null)
        {
            bgmSource.Stop();
        }
        else
        {
            Debug.Log("SFX not found");
        }
    }


}
