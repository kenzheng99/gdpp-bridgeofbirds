using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerBoss : MonoBehaviour
{

	public static DialogueManagerBoss Instance;
	public Text nameText;
	public Text dialogueText;
	public bool dialogueIsPlaying = false;
	public GameObject dialogueBox;
	public GameObject JadeEmpressPortrait;
	public GameObject CowherdPortrait;
	public GameObject WeaverGirlPortrait;
	public GameObject CowSpiritPortrait;
	private Queue<string> sentences;
	private Queue<string> speakers;


	// dialogue objects
	public Dialogue TeachJadeEmpressAttack;
	// trigger bools
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
	void Start()
	{
		dialogueBox.SetActive(false);
		WeaverGirlPortrait.SetActive(false);
		CowherdPortrait.SetActive(false);
		JadeEmpressPortrait.SetActive(false);
		CowherdPortrait.SetActive(false);

		sentences = new Queue<string>();
		speakers = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		SoundManager.Instance.PlayDialogueStartSound();
		dialogueIsPlaying = true;
		if (SkipDialogue)
		{
			return;
		}
		Time.timeScale = 0;
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

		if (Input.GetKeyDown(KeyCode.E) && (dialogueIsPlaying))
		{
			DisplayNextSentence();
		}
	}
	public void DisplayNextSentence()
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

	IEnumerator TypeSentence(string sentence, string speaker)
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

		dialogueIsPlaying = false;
	}
}
