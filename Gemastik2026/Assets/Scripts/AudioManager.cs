using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneBGM
{
    public string sceneName;
    public AudioClip bgm;
}

public class AudioManager : MonoBehaviour
{
    [Header("Scene BGM")]
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

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (SceneBGM sceneBGM in sceneBGMs)
        {
            if (sceneBGM.sceneName == scene.name)
            {
                if (sceneBGM.bgm != null)
                {
                    BGM.clip = sceneBGM.bgm;
                    BGM.loop = true;
                    BGM.Play();
                }

                return;
            }
        }
    }


    // =========================
    // BGM
    // =========================

    public void playLakeBGM()
    {
        BGM.clip = LakeBGM;
        BGM.Play();
    }

    public void playMountainBGM()
    {
        BGM.clip = MountainBGM;
        BGM.Play();
    }

    public void playVillageBGM()
    {
        BGM.clip = VillageBGM;
        BGM.Play();
    }

    public void playBananaBGM()
    {
        BGM.clip = BananaBGM;
        BGM.Play();
    }

    public void playCookBGM()
    {
        BGM.clip = CookBGM;
        BGM.Play();
    }

    public void playFishBGM()
    {
        BGM.clip = FishBGM;
        BGM.Play();
    }

    public void playNutBGM()
    {
        BGM.clip = NutBGM;
        BGM.Play();
    }

    public void playTempeBGM()
    {
        BGM.clip = TempeBGM;
        BGM.Play();
    }

    public void playRiceBGM()
    {
        BGM.clip = RiceBGM;
        BGM.Play();
    }

    public void playWinLoseBGM()
    {
        BGM.clip = WinLoseBGM;
        BGM.Play();
    }


    // =========================
    // AMBIENCE
    // =========================

    public void playLakeA()
    {
        AMB.clip = LakeA;
        AMB.Play();
    }

    public void playMountainA()
    {
        AMB.clip = MountainA;
        AMB.Play();
    }

    public void playVillageA()
    {
        AMB.clip = VillageA;
        AMB.Play();
    }


    // =========================
    // BANANA
    // =========================

    public void playBananaFallSFX()
    {
        SFX.PlayOneShot(BananaFallsfx);
    }

    public void playBananaCollectSFX()
    {
        SFX.PlayOneShot(BananaCollectsfx);
    }


    // =========================
    // COOK
    // =========================

    public void playRestockSFX()
    {
        SFX.PlayOneShot(Restocksfx);
    }

    public void playSubmitSFX()
    {
        SFX.PlayOneShot(Submitsfx);
    }

    public void playAttackSFX()
    {
        if (PlaceFoodsfx.Length == 0)
            return;

        int rand = Random.Range(0, PlaceFoodsfx.Length);

        SFX.PlayOneShot(PlaceFoodsfx[rand]);
    }


    // =========================
    // FISH
    // =========================

    public void playFishingSFX()
    {
        SFX.PlayOneShot(Fishingsfx);
    }

    public void playFishingSuccessSFX()
    {
        SFX.PlayOneShot(FishingSuccesssfx);
    }

    public void playFishingFailSFX()
    {
        SFX.PlayOneShot(FishingFailsfx);
    }


    // =========================
    // NUT
    // =========================

    public void playSwipeSFX()
    {
        SFX.PlayOneShot(Swipesfx);
    }


    // =========================
    // TEMPEH
    // =========================

    public void playWrapSFX()
    {
        SFX.PlayOneShot(Wrapsfx);
    }


    // =========================
    // TRASH / HARVEST / TAKE
    // =========================

    public void playTrashHarvestTakeSFX()
    {
        if (trashharvesttakesfx.Length == 0)
            return;

        int rand = Random.Range(0, trashharvesttakesfx.Length);

        SFX.PlayOneShot(trashharvesttakesfx[rand]);
    }


    // =========================
    // UI
    // =========================

    public void playBookClickSFX()
    {
        SFX.PlayOneShot(BookClicksfx);
    }

    public void playClickSFX()
    {
        SFX.PlayOneShot(Clicksfx);
    }

    public void playDoorSFX()
    {
        SFX.PlayOneShot(Doorsfx);
    }

    public void playHoverSFX()
    {
        SFX.PlayOneShot(Hoversfx);
    }

    public void playNewsCloseSFX()
    {
        SFX.PlayOneShot(NewsClosesfx);
    }

    public void playNewsOpenSFX()
    {
        SFX.PlayOneShot(NewsOpensfx);
    }
}