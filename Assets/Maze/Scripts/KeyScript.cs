using UnityEngine;

public enum KeyType { Key1, Key2, Key3 }

public class KeyScript : MonoBehaviour
{
    public KeyType keyIdentifier; 
    public KeyCode interactionKey = KeyCode.E;
    public string interactionPromptMessage = "Press E to Grab Key";

    [TextArea(2, 5)]
    public string entryMessage = " ";
    public float entryMessageDuration = 3.0f;

    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            if (UIManager.Instance != null)
            {
                if (!string.IsNullOrEmpty(entryMessage))
                {
                    UIManager.Instance.ShowDialogue(entryMessage, entryMessageDuration);
                }
                UIManager.Instance.ShowInteractionPrompt(interactionPromptMessage); 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideInteractionPrompt();
                UIManager.Instance.HideDialogue();
            }
        }
    }

    void Update()
    {
        if (playerInRange && playerInventory != null && Input.GetKeyDown(interactionKey))
        {
            PickUpKey();
        }
    }

    void PickUpKey()
    {
        if (playerInventory != null)
        {
            switch (keyIdentifier)
            {
                case KeyType.Key1:
                    if (!playerInventory.hasKey1)
                    {
                        playerInventory.hasKey1 = true;
                        FinalizePickup();
                    }
                    break;
                case KeyType.Key2:
                    if (!playerInventory.hasKey2)
                    {
                        playerInventory.hasKey2 = true;
                        FinalizePickup();
                    }
                    break;
                case KeyType.Key3:
                    if (!playerInventory.hasKey3)
                    {
                        playerInventory.hasKey3 = true;
                        FinalizePickup();
                    }
                    break;
            }
        }
    }

    void FinalizePickup()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideInteractionPrompt();
            UIManager.Instance.HideDialogue();
        }
        Destroy(gameObject);
    }
}