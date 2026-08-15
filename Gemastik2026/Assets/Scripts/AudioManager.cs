using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneBGM
{
    public string sceneName;
    public AudioClip bgm;
    public AudioClip ambience;
}

public class AudioManager : MonoBehaviour
{
    [Header("Scene Audio")]
    public SceneBGM[] sceneBGMs;

    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource AMB;
    [SerializeField] AudioSource SFX;

    [Header("Ambience")]
    public AudioClip LakeA;
    public AudioClip MountainA;
    public AudioClip VillageA;

    [Header("Sidescroller")]
    public AudioClip LakeBGM;
    public AudioClip MountainBGM;
    public AudioClip VillageBGM;

    [Header("Banana")]
    public AudioClip BananaBGM;
    public AudioClip BananaFallsfx;
    public AudioClip BananaCollectsfx;

    [Header("Cook")]
    public AudioClip CookBGM;
    public AudioClip Restocksfx;
    public AudioClip Submitsfx;
    public AudioClip[] PlaceFoodsfx;

    [Header("Fish")]
    public AudioClip FishBGM;
    public AudioClip Fishingsfx;
    public AudioClip FishingSuccesssfx;
    public AudioClip FishingFailsfx;

    [Header("Nut")]
    public AudioClip NutBGM;
    public AudioClip Swipesfx;

    [Header("Tempeh")]
    public AudioClip TempeBGM;
    public AudioClip Wrapsfx;

    [Header("Rice")]
    public AudioClip RiceBGM;

    [Header("Trash/harvest/take")]
    public AudioClip WinLoseBGM;
    public AudioClip[] trashharvesttakesfx;

    [Header("UI")]
    public AudioClip BookClicksfx;
    public AudioClip Clicksfx;
    public AudioClip Doorsfx;
    public AudioClip Hoversfx;
    public AudioClip NewsClosesfx;
    public AudioClip NewsOpensfx;

    private float savedCardMusicTime;

    private static AudioManager _instance;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<AudioManager>();

                if (_instance == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("AudioManager");
                    if (prefab != null)
                    {
                        GameObject go = Instantiate(prefab);
                        _instance = go.GetComponent<AudioManager>();
                        go.name = "AudioManager (AutoSpawn)";
                    }
                    else
                    {
                        Debug.LogError("ERROR: Prefab 'AudioManager' tidak ditemukan di dalam folder 'Resources'!");
                    }
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopSFX();

        foreach (SceneBGM sceneAudio in sceneBGMs)
        {
            if (sceneAudio.sceneName == scene.name)
            {
                if (sceneAudio.bgm != null)
                {
                    if (BGM.clip != sceneAudio.bgm || !BGM.isPlaying)
                    {
                        BGM.clip = sceneAudio.bgm;
                        BGM.loop = true;
                        BGM.Play();
                    }
                }

                if (sceneAudio.ambience != null)
                {
                    if (AMB.clip != sceneAudio.ambience || !AMB.isPlaying)
                    {
                        AMB.clip = sceneAudio.ambience;
                        AMB.loop = true;
                        AMB.Play();
                    }
                }
                else
                {
                    AMB.Stop();
                    AMB.clip = null;
                }

                return;
            }
        }
    }

    public void StopSFX()
    {
        if (SFX != null)
        {
            SFX.Stop();
            SFX.clip = null;
        }
    }


    // =========================
    // BGM
    // =========================

    private void PlayBGM(AudioClip clip)
    {
        if (BGM.clip == clip && BGM.isPlaying) return;
        BGM.clip = clip;
        BGM.loop = true;
        BGM.Play();
    }

    public void playLakeBGM() => PlayBGM(LakeBGM);
    public void playMountainBGM() => PlayBGM(MountainBGM);
    public void playVillageBGM() => PlayBGM(VillageBGM);
    public void playBananaBGM() => PlayBGM(BananaBGM);
    public void playCookBGM() => PlayBGM(CookBGM);
    public void playFishBGM() => PlayBGM(FishBGM);
    public void playNutBGM() => PlayBGM(NutBGM);
    public void playTempeBGM() => PlayBGM(TempeBGM);
    public void playRiceBGM() => PlayBGM(RiceBGM);
    public void playWinLoseBGM() => PlayBGM(WinLoseBGM);


    // =========================
    // AMBIENCE
    // =========================

    private void PlayAmbience(AudioClip clip)
    {
        if (AMB.clip == clip && AMB.isPlaying) return;

        AMB.clip = clip;
        AMB.loop = true;
        AMB.Play();
    }

    public void playLakeA() => PlayAmbience(LakeA);
    public void playMountainA() => PlayAmbience(MountainA);
    public void playVillageA() => PlayAmbience(VillageA);


    // =========================
    // SFX BANANA DLL
    // =========================
    public void playBananaFallSFX() { SFX.PlayOneShot(BananaFallsfx); }
    public void playBananaCollectSFX() { SFX.PlayOneShot(BananaCollectsfx); }

    public void playRestockSFX() { SFX.PlayOneShot(Restocksfx); }
    public void playSubmitSFX() { SFX.PlayOneShot(Submitsfx); }
    public void playAttackSFX()
    {
        if (PlaceFoodsfx.Length == 0) return;
        int rand = Random.Range(0, PlaceFoodsfx.Length);
        SFX.PlayOneShot(PlaceFoodsfx[rand]);
    }

    public void playFishingSFX() { SFX.PlayOneShot(Fishingsfx); }
    public void playFishingSuccessSFX() { SFX.PlayOneShot(FishingSuccesssfx); }
    public void playFishingFailSFX() { SFX.PlayOneShot(FishingFailsfx); }

    public void playSwipeSFX() { SFX.PlayOneShot(Swipesfx); }
    public void playWrapSFX() { SFX.PlayOneShot(Wrapsfx); }

    public void playTrashHarvestTakeSFX()
    {
        if (trashharvesttakesfx.Length == 0) return;
        int rand = Random.Range(0, trashharvesttakesfx.Length);
        SFX.PlayOneShot(trashharvesttakesfx[rand]);
    }

    public void playBookClickSFX() { SFX.PlayOneShot(BookClicksfx); }
    public void playClickSFX() { SFX.PlayOneShot(Clicksfx); }
    public void playDoorSFX() { SFX.PlayOneShot(Doorsfx); }
    public void playHoverSFX() { SFX.PlayOneShot(Hoversfx); }
    public void playNewsCloseSFX() { SFX.PlayOneShot(NewsClosesfx); }
    public void playNewsOpenSFX() { SFX.PlayOneShot(NewsOpensfx); }
}