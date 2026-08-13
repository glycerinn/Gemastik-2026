using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SFX;

    public AudioClip MainMenuBGM;
    public AudioClip Clicksfx;
    public AudioClip[] playerattacksfx;

    private float savedCardMusicTime;

    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void playMainMenuBGM()
    {
        BGM.clip = MainMenuBGM;
        BGM.Play();
    }

    public void playClickSFX()
    {
        SFX.PlayOneShot(Clicksfx);
    }

    public void playAttackSFX()
    {   
        // if (isGameOver) return;
        // if (isPaused) return;
        int rand = Random.Range(0, playerattacksfx.Length);
        SFX.PlayOneShot(playerattacksfx[rand]);
    }

}