using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;  // Texto donde se muestra el diálogo
    public GameObject dialoguePanel;     // Panel del diálogo
    public float typingSpeed = 0.05f;    // Velocidad de escritura

    private string[] dialogueLines;      // Líneas del diálogo
    private int currentLineIndex;        // Índice de la línea actual
    private bool isTyping;               // Controla si se está escribiendo
    private Coroutine typingCoroutine;   // Referencia a la corrutina de escritura

    public delegate void DialogueStateChanged(bool isActive);
    public event DialogueStateChanged OnDialogueStateChanged;
    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // Ocultar el panel al inicio
        }
    }

    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;

        if (dialoguePanel != null && !dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(true);
        }

        OnDialogueStateChanged?.Invoke(true); // Notifica que el diálogo ha empezado
        typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    private void Update()
    {
        // Avanzar al siguiente diálogo al hacer clic con el mouse
        if (Input.GetMouseButtonDown(0) && dialoguePanel.activeSelf)
        {
            ShowNextLine();
        }
    }

    public void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            if (isTyping)
            {
                StopTyping();
                dialogueText.text = dialogueLines[currentLineIndex]; // Completa la línea actual
                //currentLineIndex++;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Length)
                {
                    typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
                }
                              
            }
        }
       
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTyping = false;
    }

    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        OnDialogueStateChanged?.Invoke(false); // Notifica que el diálogo ha terminado
    }

}
