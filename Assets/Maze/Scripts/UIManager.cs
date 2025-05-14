using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI interactionPromptText;
    public TextMeshProUGUI dialogueText; 

    private Coroutine dialogueCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (interactionPromptText != null)
            interactionPromptText.gameObject.SetActive(false);
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }


    public void ShowInteractionPrompt(string message)
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.text = message;
            interactionPromptText.gameObject.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }


    public void ShowDialogue(string message, float duration)
    {
        if (dialogueText != null)
        {
            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
            }

            dialogueText.text = message;
            dialogueText.gameObject.SetActive(true);
            dialogueCoroutine = StartCoroutine(HideDialogueAfterTime(duration));
        }
    }

    private IEnumerator HideDialogueAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideDialogue();
    }

    public void HideDialogue()
    {
         if (dialogueText != null)
         {
            dialogueText.gameObject.SetActive(false);
         }
         dialogueCoroutine = null; 
    }
}