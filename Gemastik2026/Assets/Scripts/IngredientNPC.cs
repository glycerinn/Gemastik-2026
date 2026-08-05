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

    [Header("Highlight")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        dialogueRunner.AddCommandHandler("give_reward", () => {GiveReward();});
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        Debug.Log("Registered StartGame");
    } 

    private void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = distance <= interactDistance
                ? highlightColor
                : normalColor;
        }

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