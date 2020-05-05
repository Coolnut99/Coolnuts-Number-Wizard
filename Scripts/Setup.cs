using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

	[SerializeField]
	private Slider guessesSlider, maxValueSlider;

	[SerializeField]
	private Text maxGuessesText, maxNumberText;

	//private 

	// Use this for initialization
	void Start () {
		guessesSlider.value = 10;		// Or default from PlayerPrefs
		maxValueSlider.value = 1000;	// Or default from PlayerPrefs
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		maxGuessesText.text = guessesSlider.value.ToString();
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void MinusButtonGuessesPushed() {
		guessesSlider.value--;
		maxGuessesText.text = guessesSlider.value.ToString();
	}

	public void PlusButtonGuessesPushed() {
		guessesSlider.value++;
		maxGuessesText.text = guessesSlider.value.ToString();
	}

	public void MinusButtonNumberPushed() {
		maxValueSlider.value--;
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void PlusButtonNumberPushed() {
		maxValueSlider.value++;
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void MinusButtonNumberPushed_10() {
		maxValueSlider.value -= 10;
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void PlusButtonNumberPushed_10() {
		maxValueSlider.value += 10;
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void MinusButtonNumberPushed_100() {
		maxValueSlider.value -= 100;
		maxNumberText.text = maxValueSlider.value.ToString();
	}

	public void PlusButtonNumberPushed_100() {
		maxValueSlider.value += 100;
		maxNumberText.text = maxValueSlider.value.ToString();
	}


	public void NextScreen() {
		GameController.instance.maxSliderValue = (int)maxValueSlider.value;
		//GameController.instance.guesses = new int[(int)guessesSlider.value];
	}
}
