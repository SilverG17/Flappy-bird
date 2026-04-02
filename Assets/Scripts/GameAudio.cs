using UnityEngine;

public class GameAudio : MonoBehaviour
{
    private const string AudioRoot = "Audio/";
    private const string FlapClipName = "flap";
    private const string ScoreClipName = "score";
    private const string HitClipName = "hit";
    private const string BgmClipName = "bgm";
    private const string MusicVolumePrefKey = "MusicVolume";
    private const string SfxVolumePrefKey = "SfxVolume";
    private const float DefaultMusicVolume = 0.4f;
    private const float DefaultSfxVolume = 1f;

    private static GameAudio instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private AudioClip flapClip;
    private AudioClip scoreClip;
    private AudioClip hitClip;
    private AudioClip bgmClip;
    private float musicVolume = DefaultMusicVolume;
    private float sfxVolume = DefaultSfxVolume;

    public static GameAudio Instance
    {
        get
        {
            EnsureInstance();
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        EnsureInstance();
    }

    private static void EnsureInstance()
    {
        if (instance != null) return;

        instance = FindAnyObjectByType<GameAudio>();
        if (instance != null)
        {
            instance.Initialize();
            return;
        }

        GameObject audioObject = new GameObject("GameAudio");
        instance = audioObject.AddComponent<GameAudio>();
        instance.Initialize();
    }

    private void Initialize()
    {
        if (bgmSource != null && sfxSource != null) return;

        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        LoadVolumeSettings();
        LoadClips();
        ApplyVolumes();
        PlayBgm();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Initialize();
    }

    private void OnEnable()
    {
        GameEvents.OnCollectibleCollected += PlayScore;
        GameEvents.OnObstacleHit += PlayHit;
    }

    private void OnDisable()
    {
        GameEvents.OnCollectibleCollected -= PlayScore;
        GameEvents.OnObstacleHit -= PlayHit;
    }

    private void LoadClips()
    {
        flapClip = Resources.Load<AudioClip>(AudioRoot + FlapClipName);
        scoreClip = Resources.Load<AudioClip>(AudioRoot + ScoreClipName);
        hitClip = Resources.Load<AudioClip>(AudioRoot + HitClipName);
        bgmClip = Resources.Load<AudioClip>(AudioRoot + BgmClipName);
    }

    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicVolumePrefKey, DefaultMusicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SfxVolumePrefKey, DefaultSfxVolume);
    }

    private void ApplyVolumes()
    {
        bgmSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        bgmSource.volume = musicVolume;
        PlayerPrefs.SetFloat(MusicVolumePrefKey, musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat(SfxVolumePrefKey, sfxVolume);
        PlayerPrefs.Save();
    }

    public void PlayFlap()
    {
        PlayOneShot(flapClip, FlapClipName);
    }

    private void PlayScore()
    {
        PlayOneShot(scoreClip, ScoreClipName);
    }

    private void PlayHit()
    {
        PlayOneShot(hitClip, HitClipName);
    }

    private void PlayBgm()
    {
        if (bgmClip == null)
        {
            Debug.LogWarning("Missing background music. Add Assets/Resources/Audio/bgm.*");
            return;
        }

        if (bgmSource.isPlaying && bgmSource.clip == bgmClip) return;

        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    private void PlayOneShot(AudioClip clip, string clipName)
    {
        if (clip == null)
        {
            Debug.LogWarning($"Missing audio clip: Assets/Resources/Audio/{clipName}.*");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}
