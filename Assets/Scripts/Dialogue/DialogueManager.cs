using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	
	public static DialogueManager Instance;
	public Text nameText;
	public Text dialogueText;
	public GameObject dialogueBox;
	public GameObject JadeEmpressPortrait;
	public GameObject CowherdPortrait;
	public GameObject WeaverGirlPortrait;
	public GameObject CowSpiritPortrait;
	private Queue<string> sentences;
	private Queue<string> speakers;
	

	// dialogue objects
	public Dialogue TeachMagpieBridge;
	public Dialogue TeachHealthAndQi;
	public Dialogue TeachQiAbilities;
	public Dialogue TeachBuffsDialogue;
	public Dialogue TeachUltimateAbilityDialogue;
	public Dialogue TeachJadeEmpressAttack;
	// trigger bools
	public bool triggeredTeachHealthAndQiDialogue;
	public bool triggeredTeachQiHealAbilitiesDialogue;
	public bool triggeredTeachUltimateAbilityDialogue;
	public bool triggeredTeachBuffsDialogue;
	public bool triggeredTeachMagpieBridgeDialogue;
	public bool triggeredTeachJadeEmpressAttackDialogue;

	// debug
	private const bool SkipDialogue = false;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	// Use this for initialization
	void Start ()
	{
		dialogueBox.SetActive(false);
		WeaverGirlPortrait.SetActive(false);
		CowherdPortrait.SetActive(false);
		JadeEmpressPortrait.SetActive(false);
		CowherdPortrait.SetActive(false);

		sentences = new Queue<string>();
		speakers = new Queue<string>();
	}

    public void StartDialogue (Dialogue dialogue) {
		if (SkipDialogue) {
			return;}
		Time.timeScale = 0;
		SoundManager.Instance.PlayDialogueStartSound();
		dialogueBox.SetActive(true);
		
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		speakers.Clear();

		foreach (string speaker in dialogue.speakers)
		{
			speakers.Enqueue(speaker);
		}

		DisplayNextSentence();
	}

	public void StartTeachMagpieBridgeDialogue()
	{
		StartDialogue(TeachMagpieBridge);
	}
	public void StartTeachHealthAndQiDialogue()
	{
		StartDialogue(TeachHealthAndQi);
	}
	public void StartTeachQiAbilitiesDialogue()
	{
		StartDialogue(TeachQiAbilities);
	}

	public void StartTeachUltimateAbilityDialogue()
	{
		if (triggeredTeachUltimateAbilityDialogue == false)
		{
			StartDialogue(TeachUltimateAbilityDialogue);
		}
	}
	public void StartTeachBuffsDialogue()
	{
		if (triggeredTeachBuffsDialogue == false)
		{
			StartDialogue(TeachBuffsDialogue);
		}
	}

	public void StartTeachJadeEmpressAttackDialogue()
    {
		if (triggeredTeachJadeEmpressAttackDialogue == false)
        {
			StartDialogue(TeachJadeEmpressAttack);
			triggeredTeachJadeEmpressAttackDialogue = true;
		}
		triggeredTeachJadeEmpressAttackDialogue = true;
	}
    public void Update()
    {
        if (nameText.text == "Cowherd")
        {
			CowherdPortrait.SetActive(true);
			CowSpiritPortrait.SetActive(false);
			JadeEmpressPortrait.SetActive(false);
			WeaverGirlPortrait.SetActive(false);
		}
		else if (nameText.text == "Cow Spirit")
		{
			CowSpiritPortrait.SetActive(true);
			CowherdPortrait.SetActive(false);
			JadeEmpressPortrait.SetActive(false);
			WeaverGirlPortrait.SetActive(false);
		}
		else if (nameText.text == "Jade Empress")
		{
			JadeEmpressPortrait.SetActive(true);
			CowherdPortrait.SetActive(false);
			CowSpiritPortrait.SetActive(false);
			WeaverGirlPortrait.SetActive(false);
		}
		else if (nameText.text == "Weaver Girl")
		{
			WeaverGirlPortrait.SetActive(true);
			CowSpiritPortrait.SetActive(false);
			JadeEmpressPortrait.SetActive(false);
			CowherdPortrait.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.F) && (GameManager.Instance.dialogueIsPlaying))
        {
			DisplayNextSentence();
        }
    }
    public void DisplayNextSentence ()
    {
	    
		Debug.Log("next sentence");
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string speaker = speakers.Dequeue();
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence, speaker));
	}

	IEnumerator TypeSentence (string sentence, string speaker)
	{
		nameText.text = speaker;
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		Time.timeScale = 1;
		dialogueBox.SetActive(false);

		GameManager.Instance.dialogueIsPlaying = false;
	}
}
