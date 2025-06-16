using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogueContainer : MonoBehaviour
{
    public DialogueManager dialogueManager; // Referencia al DialogueManager
    private bool hasTalked = false;         // Evita que el diálogo se repita

    public LocalizedString[] dialogue;


    public void StartLocalizedDialogue()
    {
        string[] translatedDialogue = new string[dialogue.Length];

        for (int i = 0; i < dialogue.Length; i++)
        {
            translatedDialogue[i] = dialogue[i].GetLocalizedString();
        }

        dialogueManager.StartDialogue(translatedDialogue);
    }
}