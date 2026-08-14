using UnityEngine;
using Yarn.Unity;

public class IngredientDialogueCommands : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("give_reward", GiveReward);
    }

    private void GiveReward()
    {
        if (IngredientNPC.CurrentNPC == null)
        {
            Debug.LogWarning("No IngredientNPC is currently talking.");
            return;
        }

        IngredientNPC.CurrentNPC.GiveReward();
    }
}