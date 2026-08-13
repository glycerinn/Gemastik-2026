using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialoguePortrait : MonoBehaviour
{
    public static DialoguePortrait Instance;

    [Header("UI")]
    public Image characterPortrait;
    public GameObject portraitBackground;

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        Instance = this;
        portraitBackground.SetActive(false);
    }

    private void Start()
    {
        characterPortrait.enabled = false;

        dialogueRunner.onDialogueComplete.AddListener(HideCharacter);
        
    }

    public void ShowCharacter(Sprite sprite)
    {
        if (sprite == null)
        {
            characterPortrait.enabled = false;
            return;
        }

        characterPortrait.sprite = sprite;
        characterPortrait.enabled = true;
        portraitBackground.SetActive(true);
    }

    public void HideCharacter()
    {
        characterPortrait.enabled = false;
        portraitBackground.SetActive(false);
    }
}