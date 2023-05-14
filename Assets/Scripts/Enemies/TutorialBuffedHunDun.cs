using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialBuffedHunDun : MonoBehaviour
{
    private AttributesManager atm;
    
    private void Start() 
    {
        atm = GetComponent<AttributesManager>();
    }

    void Update()
    {
        if (atm.health < (0.35*atm.maxHealth) && DialogueManager.Instance.triggeredTeachUltimateAbilityDialogue == false)
        {
            Debug.Log("here");
            GameManager.Instance.unlockedUltimate = true;
            DialogueManager.Instance.StartTeachUltimateAbilityDialogue();
            GameManager.Instance.dialogueIsPlaying = true;
            DialogueManager.Instance.triggeredTeachUltimateAbilityDialogue = true;
        }
        
        else if (atm.health <= 0 && DialogueManager.Instance.triggeredTeachBuffsDialogue == false)
        {
            GameManager.Instance.completedTutorial = true;
        }
    }
}
