using System.Collections;
using TMPro;
using UnityEngine;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    // =========================
    // UPDATED FUNCTION
    // =========================

    public void ShowDialogue(string message, float duration, bool useTypewriter)
    {
        dialoguePanel.SetActive(true);

        // Stop previous coroutines
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (useTypewriter)
        {
            typingCoroutine = StartCoroutine(TypeText(message));
        }
        else
        {
            dialogueText.text = message;
        }

        // Start auto-hide timer
        hideCoroutine = StartCoroutine(HideAfterTime(duration));
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        dialoguePanel.SetActive(false);
    }

    // =========================
    // COROUTINES
    // =========================

    private IEnumerator TypeText(string message)
    {
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HideDialogue();
    }
}