using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialoguePortrait : MonoBehaviour
{
    [Header("UI")]
    public Image characterPortrait;
    public GameObject portraitBackground;

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        portraitBackground.SetActive(false);
        characterPortrait.enabled = false;

        dialogueRunner.AddCommandHandler("show_character", ShowDialogueCharacter);
    }

    private void Start()
    {
        dialogueRunner.onDialogueComplete.AddListener(HideCharacter);
    }

    public void ShowDialogueCharacter()
    {
        if (TalkNPC.CurrentNPC == null)
        {
            Debug.LogWarning("No CurrentNPC found!");
            return;
        }

        Sprite sprite = TalkNPC.CurrentNPC.dialogueSprite;

        Debug.Log(
            "Showing portrait for: " +
            TalkNPC.CurrentNPC.name +
            " | Sprite: " +
            sprite
        );

        ShowCharacter(sprite);
    }

    public void ShowCharacter(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Portrait sprite is NULL!");

            characterPortrait.enabled = false;
            portraitBackground.SetActive(false);
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