using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    public float openAngle = 90.0f;
    public float openSpeed = 2.0f;
    public string lockedPromptMessage = "Press E (Locked)";
    public string unlockedPromptMessage = "Press E to Open";

    public GameObject[] itemsToEnableOnOpen;

    private bool isOpen = false;
    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            UpdateInteractionPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideInteractionPrompt();
            }
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            InteractDoor();
        }
    }

    void UpdateInteractionPrompt()
    {
        if (UIManager.Instance != null && playerInventory != null)
        {
            if (playerInventory.HasAllKeys())
            {
                UIManager.Instance.ShowInteractionPrompt(unlockedPromptMessage);
            }
            else
            {
                UIManager.Instance.ShowInteractionPrompt(lockedPromptMessage + GetMissingKeysString());
            }
        }
        else if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowInteractionPrompt(lockedPromptMessage);
        }
    }

    string GetMissingKeysString()
    {
        if (playerInventory == null || playerInventory.HasAllKeys()) return "";

        string missing = " (Need:";
        if (!playerInventory.hasKey1) missing += " Watering Can,"; 
        if (!playerInventory.hasKey2) missing += " Shovel,";       
        if (!playerInventory.hasKey3) missing += " Rake";         
        if(missing.EndsWith(",")) missing = missing.Remove(missing.Length -1);
        missing += ")";
        return missing;
    }

    void InteractDoor()
    {
        if (isOpen)
        {
            targetRotation = initialRotation;
            isOpen = false;
            UpdateInteractionPrompt();
        }
        else
        {
            if (playerInventory != null && playerInventory.HasAllKeys())
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            targetRotation = initialRotation * Quaternion.Euler(0, openAngle, 0);
            isOpen = true;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideInteractionPrompt();
            }

            if (playerInventory != null) {
                Debug.Log("Resetting keys in inventory.");
                playerInventory.hasKey1 = false;
                playerInventory.hasKey2 = false;
                playerInventory.hasKey3 = false;
            }

            if (itemsToEnableOnOpen != null)
            {
                foreach (GameObject item in itemsToEnableOnOpen)
                {
                    if (item != null)
                    {
                        item.SetActive(true);
                        Debug.Log($"Enabled item: {item.name}");
                    }
                }
            }
        }
    }
}