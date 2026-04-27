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

    // =========================
    // GRADIENT TYPES
    // =========================
    public enum DialogueColorType
    {
        Blue,
        Red
    }

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
    // MAIN FUNCTION
    // =========================
    public void ShowDialogue(string message, float duration, bool useTypewriter, DialogueColorType colorType)
    {
        dialoguePanel.SetActive(true);

        // Apply gradient color
        ApplyGradient(colorType);

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

        // Auto hide
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
    // GRADIENT LOGIC
    // =========================
    private void ApplyGradient(DialogueColorType type)
    {
        dialogueText.enableVertexGradient = true;

        VertexGradient gradient = new VertexGradient();

        switch (type)
        {
            case DialogueColorType.Blue:
                Color blueTop = new Color32(0, 146, 255, 255);   // #0092FFFF
                Color blueBottom = new Color32(0, 33, 255, 255); // #0021FFFF

                gradient.topLeft = gradient.topRight = blueTop;
                gradient.bottomLeft = gradient.bottomRight = blueBottom;
                break;

            case DialogueColorType.Red:
                Color redTop = new Color32(255, 0, 0, 255);      // #FF0000FF
                Color redBottom = new Color32(0, 0, 0, 255);     // #000000FF

                gradient.topLeft = gradient.topRight = redTop;
                gradient.bottomLeft = gradient.bottomRight = redBottom;
                break;
        }

        dialogueText.colorGradient = gradient;
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