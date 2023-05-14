using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndBarrier : MonoBehaviour
{
    [SerializeField] private Dialogue TutorialEndBarrierDialogue;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.completedTutorial)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            DialogueManager.Instance.StartDialogue(TutorialEndBarrierDialogue);
        }
    }
}
