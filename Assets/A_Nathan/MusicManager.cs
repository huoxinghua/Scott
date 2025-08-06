using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [SerializeField] private AudioClip musicClipFull;
    [SerializeField] private AudioClip musicClipIntro;
    [SerializeField] private AudioClip musicClipGameplay;
    [SerializeField] private AudioClip musicClipUpgrades;

    private AudioSource musicSource;
    private Coroutine introCoroutine;
    private bool hasStarted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        string newSceneName = newScene.name;

        // Stop any coroutine in progress
        if (introCoroutine != null)
        {
            StopCoroutine(introCoroutine);
            introCoroutine = null;
        }

        switch (newSceneName)
        {
            case "XHMainMenu":
                PlayClip(musicClipFull);
                hasStarted = false;
                break;

            case "XHProtoGym":
                if (!hasStarted)
                {
                    PlayClip(musicClipIntro);
                    introCoroutine = StartCoroutine(IntroClipCoroutine());
                    hasStarted = true;
                }
                else
                {
                    PlayClip(musicClipGameplay);
                }
                break;

            case "XHUpgradeScene":
                PlayClip(musicClipUpgrades);
             //   hasStarted = false;
                break;

            default:
                // Optionally stop music or handle other scenes
                break;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Trying to play a null music clip.");
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // Already playing

        musicSource.clip = clip;
        musicSource.Play();
    }

    private IEnumerator IntroClipCoroutine()
    {
        yield return new WaitForSeconds(30f);

        // Only change music if we're still in the ProtoGym scene
        if (SceneManager.GetActiveScene().name == "XHProtoGym")
        {
            PlayClip(musicClipGameplay);
        }
    }
}