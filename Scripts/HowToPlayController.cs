using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlayController : MonoBehaviour {

	[SerializeField]
	private GameObject [] instructionPage;

	[SerializeField]
	private Text pageText;

	private int currentPage;

	private int maxPages;

	// Use this for initialization
	void Start () {
		currentPage = 0;
		maxPages = instructionPage.Length;
		SetPagesInactive();
		instructionPage[currentPage].SetActive(true);
		pageText.text = "Page " + (currentPage+1).ToString() + "/" + maxPages.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnPageForward() {
		Debug.Log("Test");
		SetPagesInactive();
		if(currentPage + 1 >= maxPages) {
			currentPage = maxPages -1;
		} else {
			currentPage++;
		}
		instructionPage[currentPage].SetActive(true);
		pageText.text = "Page " + (currentPage+1).ToString() + "/" + maxPages.ToString();
	}

	public void TurnPageBackward() {
		SetPagesInactive();
		if(currentPage <= 0) {
			currentPage = 0;
		} else {
			currentPage--;
		}
		instructionPage[currentPage].SetActive(true);
		pageText.text = "Page " + (currentPage+1).ToString() + "/" + maxPages.ToString();
	}

	public void GoBack() {
		SceneManager.LoadScene("Main Menu");
	}

	void SetPagesInactive() {
		for (int i = 0; i < instructionPage.Length; i++) {
			instructionPage[i].SetActive(false);
		}
	}
}
