using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public GameObject canvas;
	public Text text;
	public Text ttext;
	public float dialogueSpeed;

	public GameObject optionsPanel;
	public GameObject option;
	public GameObject responsePanel;

	private string[] dialogueS = {"¿Hola, como puedo ayudarte?","¿dónde está la arena?","Acabas de llegar! ¿Quieres luchar ya?", "¡Sí! Enfrentar mi ira!!"};

	private string[] translationE = {"Hi, how can i help you?","Where is the arena?", "You just got here! Want to battle already?", "Yes! Face my wrath!!"};
	private string[] response; 

	private int curIndex;
	private int textPos;
	private int wordIndex;

	private int responseLength;
	private int responseCompleted;

	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		canvas.SetActive (true);

		curIndex = 0;

		// Reset dialogue string, and position within text
		textPos   = 0;
		
		// Began animation of text
		InvokeRepeating("AnimateText", 0.0f, dialogueSpeed);
	}

	public void ContinueDialogue ()
	{
		ClearOptions ();

		InvokeRepeating ("AnimateText", 0.0f, dialogueSpeed);
	}

	public void ClearOptions ()
	{
		GameObject [] tobeCleared = GameObject.FindGameObjectsWithTag ("Option");

		foreach (GameObject option in tobeCleared)
			Destroy (option);

	}

	public void OnDisable()
	{
		canvas.SetActive (false);
		CancelInvoke();
	}
	
	void AnimateText()
	{
		// Show text
		text.text = dialogueS[curIndex].Substring(0, textPos);

		if (textPos == 0) {
			text.transform.position = new Vector3(text.transform.position.x,text.transform.position.y,(float)curIndex);
		}
		if (textPos < dialogueS[curIndex].Length)
		{
			//Debug.Log(string.Format("({0}) Animating text", this.name));
			textPos++;
		}
		else
		{
			//Debug.Log(string.Format("{0} is displaying end of dialogue prompt", this.name));
			
			CancelInvoke();
			textPos = 0;

			if (curIndex % 2 == 0) {
				SetupSentenceBuilder(); 
				curIndex++;
				if (curIndex == dialogueS.Length)
					return;

			}else {
				curIndex ++;

				if (curIndex == dialogueS.Length)
					return;

				InvokeRepeating ("AnimateText", 2.0f, dialogueSpeed);
			}


		}
	}

	void SetupSentenceBuilder()
	{
		wordIndex = 0;
		InvokeRepeating("CreateOption", 1.0f, 0.25f);
	}

	void CreateOption ()
	{
		string[] words = dialogueS [curIndex].Split (' ');


		response = new string[words.Length];
		responseLength = words.Length;
		responseCompleted = 0;

		string word = words [wordIndex];

		GameObject temp = Instantiate(option) as GameObject;

		Vector2 psize = optionsPanel.GetComponent<RectTransform> ().sizeDelta;
		Vector2 size = temp.GetComponent<RectTransform> ().sizeDelta;

		temp.GetComponentInChildren<Text>().text = word;
		temp.GetComponentInChildren<Image> ().color = new Color (Random.Range (0.0f,1.0f), Random.Range (0.0f,1.0f), Random.Range (0.0f, 1.0f));
		Vector3 pos = new Vector3 (Random.Range(0,psize.x - size.x),-1 * Random.Range (size.y, psize.y),temp.transform.localPosition.z);
		Debug.Log (pos);
		temp.transform.parent = optionsPanel.transform;
		temp.transform.localPosition = pos;


		wordIndex++;

		if (wordIndex == words.Length){
			Debug.Log("cancelling invoke");
			CancelInvoke();
		}
	}


	public void DisplayTranslation()
	{
		if (text.text != null && text.text.Length > 0 && textPos == 0)
			ttext.text = translationE [(int)text.transform.position.z];

		//display translation [curindex]
	}

	public void HideTranslation()
	{
		ttext.text = "";

	}


	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateResponse (string res, int index)
	{
		response [index] = res;

		if (++responseCompleted == responseLength /*&& check for correct response*/) {
			ContinueDialogue();
		}
	}

	public void UpdateResponse (int index)
	{
		response [index] = null;

		--responseCompleted;
	}

}
