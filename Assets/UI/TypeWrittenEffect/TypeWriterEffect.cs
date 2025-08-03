using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	public bool playSound;
	
	[Header("Show on End")]
	public bool showOnEnd;
	public GameObject obj;

	[Header("Typing Sound")]
	public AudioSource source;
	public AudioClip clip;
	
	
	private string currentText = "";
	

	// Use this for initialization

	private void OnEnable()
	{
		if (showOnEnd) obj.SetActive(false);
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for(int i = 0; i < fullText.Length + 1; i++){
			currentText = fullText.Substring(0,i);
			this.GetComponent<TextMeshProUGUI>().text = currentText;
			if (playSound) source.PlayOneShot(clip);
			yield return new WaitForSecondsRealtime(delay);
		}
		if (showOnEnd) obj.SetActive(true);
	}

	public void SetText(string text)
	{
		fullText = text;
		StartCoroutine(ShowText());
	}
}
