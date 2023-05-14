using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractableTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool playerInRange = false;
    public List<Dialogue> dialoguesList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.dialogueIsPlaying)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
            else if (playerInRange)
            {
                TriggerDialogue();
                GameManager.Instance.dialogueIsPlaying = true;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void TriggerDialogue ()
    {
        dialogue = dialoguesList[UnityEngine.Random.Range(0, dialoguesList.Count)];
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
