using UnityEngine;

public class TalkingObject : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    [TextArea(3, 10)] 
    public string messageToShow = "Message to say";
    public float messageDuration = 4.0f; 
    public string interactionPromptMessage = "Press E to Talk";

    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowInteractionPrompt(interactionPromptMessage);
            }
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
                UIManager.Instance.HideDialogue();
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ShowMessage();
        }
    }

    void ShowMessage()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowDialogue(messageToShow, messageDuration);

            UIManager.Instance.HideInteractionPrompt();
        }
    }
}