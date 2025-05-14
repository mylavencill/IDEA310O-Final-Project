using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiddleDoor : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;
    public float openAngle = 90.0f;
    public float openSpeed = 2.0f;
    public GameObject[] itemsToEnableOnOpen; 

    public string interactionPromptMessage = "Press E to Answer Riddle";
    [TextArea(3, 5)]
    public string question = " ";
    public string[] answers = new string[] { " ", " ", " " };
    public int correctAnswerIndex = 0;

    public GameObject questionUIPanel;
    public TextMeshProUGUI questionTextUI;
    public Button[] answerButtonsUI;        
    public TextMeshProUGUI feedbackTextUI; 

    private bool isOpen = false;
    private bool playerInRange = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private bool isQuestionActive = false;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen && !isQuestionActive)
        {
            playerInRange = true;
            ShowInteractionPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideInteractionPrompt();
            if (isQuestionActive)
            {
                HideQuestionUI();
            }
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.unscaledDeltaTime * openSpeed);

        if (playerInRange && !isOpen && Input.GetKeyDown(interactionKey))
        {
            if (!isQuestionActive)
            {
                DisplayQuestion();
            }
        }

        if (isQuestionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            HideQuestionUI();
            if (playerInRange && !isOpen)
            {
                ShowInteractionPrompt();
            }
        }
    }

    void ShowInteractionPrompt()
    {
        if (UIManager.Instance != null && !isQuestionActive && !isOpen)
        {
            UIManager.Instance.ShowInteractionPrompt(interactionPromptMessage);
        }
    }

    void HideInteractionPrompt()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideInteractionPrompt();
        }
    }

    void DisplayQuestion()
    {
        if (isOpen || questionUIPanel == null || questionTextUI == null || answerButtonsUI == null)
        {
            return;
        }

        isQuestionActive = true;
        HideInteractionPrompt();

        questionUIPanel.SetActive(true);
        questionTextUI.text = question;

        if (feedbackTextUI != null)
            feedbackTextUI.gameObject.SetActive(false);

        for (int i = 0; i < answerButtonsUI.Length; i++)
        {
            if (i < answers.Length)
            {
                answerButtonsUI[i].gameObject.SetActive(true);
                TextMeshProUGUI buttonText = answerButtonsUI[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = answers[i];
                }

                answerButtonsUI[i].onClick.RemoveAllListeners();
                int answerIndex = i;
                answerButtonsUI[i].onClick.AddListener(() => OnAnswerSelected(answerIndex));
            }
            else
            {
                answerButtonsUI[i].gameObject.SetActive(false);
            }
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideQuestionUI()
    {
        if (questionUIPanel != null)
            questionUIPanel.SetActive(false);

        isQuestionActive = false;

        Time.timeScale = 1f;
        if (!isOpen)
        {
             Cursor.lockState = CursorLockMode.Locked;
             Cursor.visible = false;
        }
    }

    void OnAnswerSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            if (feedbackTextUI != null)
            {
                feedbackTextUI.text = "Correct!";
                feedbackTextUI.color = Color.green;
                feedbackTextUI.gameObject.SetActive(true);
            }
            HideQuestionUI();
            OpenDoor();
        }
        else
        {
            if (feedbackTextUI != null)
            {
                feedbackTextUI.text = "Incorrect. Try again.";
                feedbackTextUI.color = Color.red;
                feedbackTextUI.gameObject.SetActive(true);
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetRotation = initialRotation * Quaternion.Euler(0, openAngle, 0);

            HideInteractionPrompt();

            if (itemsToEnableOnOpen != null)
            {
                foreach (GameObject item in itemsToEnableOnOpen)
                {
                    if (item != null)
                    {
                        item.SetActive(true);
                    }
                }
            }

            Collider col = GetComponent<Collider>();
            if (col != null && col.isTrigger)
            {
                col.enabled = false;
            }
        }
    }
}