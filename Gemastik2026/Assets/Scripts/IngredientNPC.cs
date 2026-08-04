using UnityEngine;
using Yarn.Unity;

public class IngredientNPC : MonoBehaviour
{
    public static IngredientNPC CurrentNPC;

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;
    public string dialogueNode;

    [Header("Reward")]
    public FoodSO rewardFood;
    private bool rewardGiven;
    
    public Transform player;
    public float interactDistance = 2f;

    public void Awake()
    {
        dialogueRunner.AddCommandHandler("give_reward", () => {GiveReward();});
        Debug.Log("Registered StartGame");
    } 

    private void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Talk();
        }
    }

    public void Talk()
    {
        Debug.Log("Called");
        CurrentNPC = this;
        dialogueRunner.StartDialogue(dialogueNode);
    }

    public void GiveReward()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;
        IngredientManager.Instance.UnlockFood(rewardFood);
        TownGameManager.Instance.CollectIngredient(rewardFood);
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RefreshInventory();
        }
        Debug.Log($"{name} gave {rewardFood.foodName}");
    }
}