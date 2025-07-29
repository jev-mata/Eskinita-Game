using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;

    private void Awake()
    {
      if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        //StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
        Debug.Log("🔊 Requested to play: " + trackName);
        AudioClip clip = musicLibrary.GetClipFromName(trackName);

        if (clip == null)
        {
            Debug.LogWarning("❌ No AudioClip found for: " + trackName);
        }
        else
        {
            StartCoroutine(AnimateMusicCrossfade(clip, fadeDuration));
        }
    }
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack,float fadeDuration = 0.5f)
    {
        if (nextTrack == null)
            yield break;

        // Stop if same track is already playing
        if (musicSource.clip == nextTrack && musicSource.isPlaying)
        {
            Debug.Log("🎵 Same track is already playing: " + nextTrack.name);
            yield break;
        }

        Debug.Log("🎧 Fading from: " + (musicSource.clip != null ? musicSource.clip.name : "None") + " to: " + nextTrack.name);

        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }

    public void PlayMusicInstant(string trackName)
    {
        AudioClip nextTrack = musicLibrary.GetClipFromName(trackName);
        if (nextTrack != null)
        {
            musicSource.Stop();
            musicSource.clip = nextTrack;
            musicSource.volume = 1f;
            musicSource.Play();
        }
    }


    public void PlaySFX(string sfxName)
    {
        AudioClip sfx = musicLibrary.GetSFXClip(sfxName);
        if (sfx != null)
            sfxSource.PlayOneShot(sfx);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded Scene: " + scene.name);

        if (scene.name == "EndlessMode") // Replace with your actual scene name
        {
            Debug.Log("Trying to play InGame music");
            PlayMusic("InGame");
        }
        else if (scene.name == "MainMenu")
        {
            Debug.Log("Trying to play MainMenu music");
            PlayMusic("MainMenu");
        }
    }



    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}
