using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAreaTrigger : MonoBehaviour {

	public Dialogue dialogue;
	public List<Dialogue> dialoguesList;
	

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameManager.Instance.dialogueIsPlaying = true;
			TriggerDialogue();
		}	
	}
	
	private void OnTriggerExit(Collider other)
	{
		Destroy(gameObject);
	}
	public void TriggerDialogue()
	{
		dialogue = dialoguesList[UnityEngine.Random.Range(0, dialoguesList.Count)];
		if (DialogueManager.Instance) {
			DialogueManager.Instance.StartDialogue(dialogue);
		} else if (DialogueManagerBoss.Instance) {
			DialogueManagerBoss.Instance.StartDialogue(dialogue);
		}
	}
}
