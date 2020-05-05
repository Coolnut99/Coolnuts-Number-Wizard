using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	[SerializeField]
	private Text introText, startButtonText;

	[SerializeField]
	private Animator gameStatsAnimator;

	[SerializeField]
	private GameObject gameStatsPanel, soundOff, noSymbol, exitPanel;

	[SerializeField]
	private StatsController statsController;

	private bool panelIsExiting;

	// Use this for initialization
	void Start () {
		soundOff.SetActive(false);
		noSymbol.SetActive(false);
		exitPanel.SetActive(false);
		panelIsExiting = false;
		gameStatsPanel.SetActive(false);
		if(GameController.instance.wussedOut > 0) {
			introText.text = "YOU THINK YOU CAN WUSS OUT OF THIS GAME? WUSS! WUSS! WUSS! WUSS! WUSS!!!";
			startButtonText.text = "I AM SORRY FOR BEING A WUSS";
			GameController.instance.wussedOut = 0;
			GameController.instance.numberOfLosses++; //Change to wussed out?
			GameController.instance.SaveTotals();
			GameController.instance.SaveWuss();
		} else if(GameController.instance.youCheatedAThirdTime) {
			introText.text = "YOU ARE A WUSS FOR CHEATING. YOU CAN NEVER PLAY THIS GAME AGAIN!";
			startButtonText.text = "WUSS";
		} else if (GameController.instance.youCheatedAgain) {
			introText.text = "YOU ARE A CHEATER AND LOSER. I WILL NEVER PLAY WITH YOU AGAIN!!!!!";
			startButtonText.text = "LOSER";
		} else if(GameController.instance.youCheated){
			introText.text = "Oh boy. THE CHEATER is back.\nYou here to cheat AGAIN???";
			startButtonText.text = "CHEATER";
		} else {
			introText.text = "Try and beat me, FOOL!\nAnd if you cheat, you are gonna regret it.";
		}

		SetSound();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void StartGame() {
		Debug.Log("Start game!");
		GameController.instance.wussedOut = 1;
		GameController.instance.SaveWuss();
		if(GameController.instance.youCheatedAThirdTime) {
			Debug.Log("YOU ARE A WUSS! I WILL NEVER PLAY THIS GAME WITH YOU EVER AGAIN!!!!!");
		} else {
			SceneManager.LoadScene("Setup");
		}
	}

	public void HowToPlay() {
		Debug.Log("Going to How to Play section");
		SceneManager.LoadScene("How to Play");
	}


	public void GameStats() {
		Debug.Log("Going to Game Stats section.");
		gameStatsPanel.SetActive(true);
		gameStatsAnimator.Play("Stats Panel Enter");
	}

	public void GameStatsExit() {
		if(!panelIsExiting) {
			StartCoroutine(GameStatsExitCoroutine());
		}
	}

	public void ToggleSound() {
		if(GameController.instance.soundOn == 0) {
			Debug.Log("Toggling sound ON.");
			GameController.instance.soundOn = 1;
			AudioListener.volume = 1f;
			soundOff.SetActive(false);
			noSymbol.SetActive(false);
			GameController.instance.SaveSound();
		} else {
			Debug.Log("Toggling sound OFF.");
			GameController.instance.soundOn = 0;
			AudioListener.volume = 0f;
			soundOff.SetActive(true);
			noSymbol.SetActive(true);
			GameController.instance.SaveSound();
		}
	}

	public void ExitGame() {
		exitPanel.SetActive(true);
	}

	public void ResetCheats() {
		GameController.instance.ClearCheats();
		statsController.SetFields();
	}

	public void ResetGame() {
		GameController.instance.Reset();
		GameController.instance.SetTotals();
		statsController.SetFields();
	}

	IEnumerator GameStatsExitCoroutine() {
		panelIsExiting = true;
		gameStatsAnimator.Play("Stats Panel Exit");
		yield return new WaitForSecondsRealtime(1.1f);
		gameStatsPanel.SetActive(false);
		panelIsExiting = false;
	}

	public void SetSound() {
		if(GameController.instance.soundOn == 0) {
			GameController.instance.soundOn = 0;
			AudioListener.volume = 0f;
			soundOff.SetActive(true);
			noSymbol.SetActive(true);
		} else {
			GameController.instance.soundOn = 1;
			AudioListener.volume = 1f;
			soundOff.SetActive(false);
			noSymbol.SetActive(false);
		}
	}

	public void CancelExit() {
		exitPanel.SetActive(false);
	}

	public void ConfirmExit() {
		Application.Quit();
	}
}
