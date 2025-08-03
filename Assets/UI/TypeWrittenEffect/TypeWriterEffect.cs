using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";

	// Use this for initialization

	private void OnEnable()
	{
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for(int i = 0; i < fullText.Length + 1; i++){
			currentText = fullText.Substring(0,i);
			this.GetComponent<TextMeshProUGUI>().text = currentText;
			yield return new WaitForSecondsRealtime(delay);
		}
	}

	public void SetText(string text)
	{
		fullText = text;
		StartCoroutine(ShowText());
	}
}
