using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NumberWizardSetup : MonoBehaviour {

	[SerializeField]
	private Text introText, maxGuessesText, maxNumberText, warningText;

	[SerializeField]
	private InputField maxGuessesInput, maxNumberInput;

	int maxGuesses, maxNumber;

	// Use this for initialization
	void Start () {
		if(GameController.instance.youCheated) {
			introText.text = "IF YOU THINK YOU CAN CHEAT ME AGAIN, YOU WILL PAY!!!!!!";
		}
		maxGuessesInput.text = "10";
		maxNumberInput.text = "1000";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NextScreen() {
		maxGuesses = int.Parse(maxGuessesText.text);
		maxNumber = int.Parse(maxNumberText.text);

		warningText.text = "";
		warningText.resizeTextForBestFit = true;

		if(maxGuesses < 3 || maxGuesses > 20) {
			warningText.text = "Your number of guesses needs to be between 3 and 20 inclusive. ";
		}

		if(maxNumber < 10 || maxNumber > 100000) {
			warningText.text += "Your max number must be between 10 and 100000 inclusive.";
		}

		if((maxGuesses >= 3 && maxGuesses <= 20) && (maxNumber >= 10 && maxNumber <= 100000)) {
			Debug.Log("Go to the next screen!");
			GameController.instance.maxSliderValue = maxNumber;
			GameController.instance.numberOfGuesses = maxGuesses;
			SceneManager.LoadScene("Gameplay");

		}
	}
}
