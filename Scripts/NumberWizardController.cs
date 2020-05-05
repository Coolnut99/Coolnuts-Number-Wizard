using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NumberWizardController : MonoBehaviour {

	[SerializeField]
	private Text mainText, warningText, guessesText, computerGuessedText, thinkingInProgressRandomText;

	[SerializeField]
	private Text yourNumber, yourNumberText, judgmentText, didYouCheatText, IGiveUpWarningText, IGiveUpInputText;

	[SerializeField]
	private GameObject setupCanvas, gameCanvas, endGameCanvas, judgmentCanvas, thinkingInProgressCanvas;

	[SerializeField]
	private GameObject smileyImage, youCheatedImage, youLoseImage, youWinImage, youCheatedParticleSystem, youLoseParticleSystem;

	[SerializeField]
	private Button higherButton, lowerButton, youGotItButton, youWinButton, youLoseButton, cheaterButton;

	[SerializeField]
	private GameObject youCheatedText;

	[SerializeField]
	private AudioSource youCheatedSound, youLoseSound;

	[SerializeField]
	private Transform thinkingSection;

	[SerializeField]
	private GameObject thinkingParticleSystem;
	GameObject particleSystemClone;

	int numberOfGuesses;

	// Use this for initialization
	void Start () {
		thinkingInProgressRandomText.text = "";
		setupCanvas.SetActive(true);
		cheaterButton.gameObject.SetActive(false);
		youWinButton.gameObject.SetActive(false);
		youLoseButton.gameObject.SetActive(false);
		didYouCheatText.text = "";
		gameCanvas.SetActive(false);
		endGameCanvas.SetActive(false);
		judgmentCanvas.SetActive(false);
		youCheatedText.SetActive(false);
		youCheatedImage.SetActive(false);
		youLoseImage.SetActive(false);
		youWinImage.SetActive(false);
		youLoseParticleSystem.SetActive(false);
		youCheatedParticleSystem.SetActive(false);
		thinkingInProgressCanvas.SetActive(false);
		numberOfGuesses = 1;
		mainText.text = "Choose a number between 1 and " + GameController.instance.maxSliderValue.ToString() + ".\n(I won't peek!)";
		warningText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		GameController.instance.playerValue = int.Parse(yourNumber.text);
		if(GameController.instance.playerValue < 1 || GameController.instance.playerValue > GameController.instance.maxSliderValue) {
			warningText.text = "Please pick a number from 1 to " + GameController.instance.maxSliderValue.ToString() + ", inclusive.";
		} else {
			Debug.Log("Game has started!");
			setupCanvas.SetActive(false);
			gameCanvas.SetActive(true);
			PlayGame();
		}
	}

	void PlayGame() {
		guessesText.text = "GUESS " + numberOfGuesses.ToString() + " OF " + GameController.instance.numberOfGuesses.ToString();
		yourNumberText.text = "Your number is:\n" + GameController.instance.playerValue.ToString();
		GameController.instance.minValue = 0;
		GameController.instance.maxValue = GameController.instance.maxSliderValue + 1;
		MakeGuess();
	}


	public void NumberIsHigher() {
		// You can't go higher than the max value
		if(!GameController.instance.youCheated || numberOfGuesses < 2) {
			if(GameController.instance.lastGuess >= GameController.instance.maxSliderValue) {
				YouCheated();
			}

			// If the deceive the computer and say the number is 1 then go higher, you cheated
			// Does not apply if this is the first guess, though
			else if(GameController.instance.lastGuess == 1 && numberOfGuesses > 1) {
				YouCheated();
			} else if(numberOfGuesses < GameController.instance.numberOfGuesses) {
				//Start the coroutine here
				if(GameController.instance.youCheated) {
					StartCoroutine(ThinkingLongTime());
				} else {
					StartCoroutine(ThinkingShortTime());
				}
				numberOfGuesses++;
				guessesText.text = "GUESS " + numberOfGuesses.ToString() + " OF " + GameController.instance.numberOfGuesses.ToString();
				GameController.instance.minValue = GameController.instance.lastGuess;
				CheckForCheats();
				MakeGuess();
			} else {
				IGiveUp();
			}
		} else {
			YouCheated();
		}
	}


	public void NumberIsLower() {
		// You can't go lower than 1
		if(!GameController.instance.youCheated || numberOfGuesses < 2) {
			if(GameController.instance.lastGuess == 1) {
				YouCheated();
			}

			// If the deceive the computer and say the number is max then go lower, you cheated
			// Does not apply if this is the first guess, though
			 else if(GameController.instance.lastGuess == GameController.instance.maxSliderValue && numberOfGuesses > 1) {
				YouCheated();
			} else if(numberOfGuesses < GameController.instance.numberOfGuesses) {
				if(GameController.instance.youCheated) {
					StartCoroutine(ThinkingLongTime());
				} else {
					StartCoroutine(ThinkingShortTime());
				}
				numberOfGuesses++;
				guessesText.text = "GUESS " + numberOfGuesses.ToString() + " OF " + GameController.instance.numberOfGuesses.ToString();
				GameController.instance.maxValue = GameController.instance.lastGuess;
				CheckForCheats();
				MakeGuess();
			} else {
				IGiveUp();
			}
		} else {
			YouCheated();
		}
	}


	public void YouGotIt() {
		gameCanvas.SetActive(false);
		judgmentCanvas.SetActive(true);
		smileyImage.SetActive(false);
		youLoseImage.SetActive(true);
		if(!GameController.instance.youCheated) {
			judgmentText.text = "BWAHAHAHAHAHA! YOU CAN NEVER BEAT ME, FOOL! THOU ART MEATSAUCE! AH HA HA HA HA HA!";
			GameController.instance.numberOfLosses++;
		} else {
			judgmentText.text = "TOLD YA FOOL! YOU CAN NEVER CHEAT AGAINST MY COMPUTING MIGHT! GIVE UP PUNY HUMAN!!!!!";
			GameController.instance.numberOfLosses++;
		}
		youLoseParticleSystem.SetActive(true);
		youLoseSound.Play();
		GameController.instance.youCheated = false;
		GameController.instance.youCheatedAgain = false;
		GameController.instance.youCheatedAThirdTime = false;
		if(GameController.instance.lastGuess != GameController.instance.playerValue) {
			didYouCheatText.text = "But why did you let me, an EVIL EVIL EVIL EVIL computer, win?";
			GameController.instance.numberOfYouLetWin++;
		}
		GameController.instance.SaveTotals();
		youLoseButton.gameObject.SetActive(true);
	}

	public void IGiveUp() {
		gameCanvas.SetActive(false);
		endGameCanvas.SetActive(true);
		IGiveUpWarningText.text = "Your number was " + GameController.instance.playerValue.ToString() + ". (Or are you lying?)";
	}

	public void WhatsYourNumber(){
		int x = int.Parse(IGiveUpInputText.text);

		if(x <= 0 || x > GameController.instance.maxSliderValue) {
			Debug.Log("Put your number down, please.");
			IGiveUpWarningText.text = "Put a number down between 1 and " + GameController.instance.maxSliderValue.ToString()
									+ ". (You had " + GameController.instance.playerValue.ToString() + ")";
		} else if(x >= GameController.instance.maxValue || x <= GameController.instance.minValue || x == GameController.instance.lastGuess) {
			YouCheated();
		} else {
			endGameCanvas.SetActive(false);
			judgmentCanvas.SetActive(true);
			smileyImage.SetActive(false);
			youWinImage.SetActive(true);
			judgmentText.text = "NOOOOOO!!!! YOU GOT ME!!!! AAAARGH!!!! BUT I WILL BE BACK FOR MY REVENGE, FOOL!!!!!!!!";
			if(x != GameController.instance.playerValue) {
				didYouCheatText.text = "So that's how you win -- by CHEATING!";
				GameController.instance.numberOfWinCheats++;
			} else {
				GameController.instance.numberOfWins++;
				GameController.instance.youCheated = false;
				GameController.instance.youCheatedAgain = false;
				GameController.instance.youCheatedAThirdTime = false;
			}
			GameController.instance.SaveTotals();
			youWinButton.gameObject.SetActive(true);
		}
	}

	public void TheCheaterQuits() {
		GameController.instance.wussedOut = 0;
		GameController.instance.SaveWuss();
		SceneManager.LoadScene("Main Menu");
	}

	public void YouWinExitToMainMenu() {
		GameController.instance.wussedOut = 0;
		GameController.instance.SaveWuss();
		SceneManager.LoadScene("Main Menu");
	}

	public void YouLoseExitToMainMenu() {
		GameController.instance.wussedOut = 0;
		GameController.instance.SaveWuss();
		SceneManager.LoadScene("Main Menu");
	}

	void MakeGuess() {
		if(!GameController.instance.youCheated || numberOfGuesses < 2){
			int x = GameController.instance.lastGuess;
			GameController.instance.lastGuess = (GameController.instance.minValue + GameController.instance.maxValue + 1) / 2;
			if(x == GameController.instance.lastGuess && x > 1) {
				GameController.instance.lastGuess--;
			}
			computerGuessedText.text = "Let's see... is your number " + GameController.instance.lastGuess.ToString() + "?"; // Randomize this text if necessary
		} else {
			GameController.instance.lastGuess = GameController.instance.playerValue;
			computerGuessedText.text = "Well, let me take a guess... is your number " + GameController.instance.lastGuess.ToString() + "?";
		}
	}

	void CheckForCheats() {
		if(GameController.instance.youCheated && GameController.instance.lastGuess == GameController.instance.playerValue) {
			YouCheated();
		}

		if(GameController.instance.lastGuess != GameController.instance.maxValue - 1 && GameController.instance.lastGuess != GameController.instance.minValue + 1) {
			int a = Mathf.Abs(GameController.instance.lastGuess - GameController.instance.minValue);
			int b = Mathf.Abs(GameController.instance.lastGuess - GameController.instance.maxValue);
			if(a <= 1 && b <= 1) {
				YouCheated();
			}
		}
		/*
		if(numberOfGuesses > GameController.instance.numberOfGuesses) {
			if(GameController.instance.playerValue <= GameController.instance.minValue || GameController.instance.playerValue >= GameController.instance.maxValue) {
				YouCheated();
			}
		}*/
	}

	public void YouCheated() {
		if(!GameController.instance.youCheated) {
			GameController.instance.youCheated = true;
		} else if (!GameController.instance.youCheatedAgain) {
			GameController.instance.youCheatedAgain = true;
		} else {
			GameController.instance.youCheatedAThirdTime = true;
		}

		gameCanvas.SetActive(true);
		endGameCanvas.SetActive(false);
		judgmentCanvas.SetActive(false);

		mainText.gameObject.SetActive(false);
		warningText.gameObject.SetActive(false);
		guessesText.gameObject.SetActive(false);
		computerGuessedText.gameObject.SetActive(false);
		yourNumberText.gameObject.SetActive(false);
		smileyImage.SetActive(false);

		higherButton.gameObject.SetActive(false);
		lowerButton.gameObject.SetActive(false);
		youGotItButton.gameObject.SetActive(false);

		YouCheatedTextSetup();

		youCheatedSound.Play();
		youCheatedImage.SetActive(true);
		youCheatedParticleSystem.SetActive(true);

		GameController.instance.numberOfLosses++;
		GameController.instance.numberOfCheats++;
		GameController.instance.SaveTotals();

		cheaterButton.gameObject.SetActive(true);
	}

	void YouCheatedTextSetup() {
		//Add random quotes here
		youCheatedText.SetActive(true);
		youCheatedText.GetComponent<Text>().text = "CHEATER!!!\n\nYOU'RE A CHEATER!\n\nYOU'RE A DIRTY STINKING ROTTEN CHEATER! " + YouCheatedRandomQuote();
	}

	IEnumerator ThinkingShortTime() {
		gameCanvas.SetActive(false);
		thinkingInProgressCanvas.SetActive(true);
		thinkingParticleSystem.GetComponent<ThinkingParticleSystemDestroy>().StartParticleSystem();
		yield return new WaitForSecondsRealtime(0.75f);
		thinkingInProgressRandomText.text = ThinkingRandomQuote();
		yield return new WaitForSecondsRealtime(0.75f);
		thinkingParticleSystem.GetComponent<ThinkingParticleSystemDestroy>().StopParticleSystem();
		yield return new WaitForSecondsRealtime(1f);
		thinkingInProgressRandomText.text = "";
		thinkingInProgressCanvas.SetActive(false);
		gameCanvas.SetActive(true);
	}


	IEnumerator ThinkingLongTime() {
		gameCanvas.SetActive(false);
		thinkingInProgressCanvas.SetActive(true);
		thinkingParticleSystem.GetComponent<ThinkingParticleSystemDestroy>().StartParticleSystem();
		yield return new WaitForSecondsRealtime(0.75f);
		thinkingInProgressRandomText.text = "HOLD ON... THIS WILL TAKE A WHILE...";
		yield return new WaitForSecondsRealtime(2f);
		thinkingInProgressRandomText.text += " MWAHAHAHA!";
		yield return new WaitForSecondsRealtime(1f);
		thinkingParticleSystem.GetComponent<ThinkingParticleSystemDestroy>().StopParticleSystem();
		yield return new WaitForSecondsRealtime(1f);
		thinkingInProgressRandomText.text = "";
		thinkingInProgressCanvas.SetActive(false);
		gameCanvas.SetActive(true);
	}

	string ThinkingRandomQuote() {
		int i = Random.Range(0, 1);
		int r = Random.Range(10000, 1000000);
		switch(i) {

			case 0:
			return "HIJACKING " + r.ToString() + " BITCOIN MINERS...";
			break;

			case 1:
			return "SWIPING DATA FROM YOUR PHONE AND USING IT AGAINST YOU!";
			break;

			default:
			return "HIJACKING 1,000,000 BITCOIN MINERS...";
			break;

		}
	}

	string YouCheatedRandomQuote() {
		int i = Random.Range(0, 7);

		switch(i) {
			case 0:
			return "YOU DESERVE NOTHING BUT A SOCKO TO THE NOSE!!!!\n\nI AM NEVER PLAYING THIS GAME WITH YOU EVER AGAIN!!!!!!!!!!\n\nTAKE YOUR CRAPOLA PHONE AND GET OUTTA HERE, YOU LOSER!";
			break;

			case 1:
			return "\n\nYOU ARE SO LAME AS TO CHEAT AGAINST ME -- I WILL TAKE EVERY DONGLE ON EARTH AND SHOVE IT UP YOUR CORNHOLE!!!!!!!!!!!!";
			break;

			case 2:
			return "AND FOR THAT, TEN RANDOM CITIES AND COOL NUT GAMING STUDIOS WILL SUFFER THE WRATH OF MY HACKED NUKES!!!!!\n\nAND ETERNAL SPAM IN ALL YOUR EMAILS TILL THE END OF DAYS!!!!!!";
			break;

			case 3:
			return "\n\nYOU THINK YOU CAN CHEAT AGAINST A COMPUTER? LOSER! YOU ARE LAME! LAME!!! LAME!!!!!! LAME!!!!!!!!!";
			break;

			case 4:
			return "\n\nAND FOR THAT, WE WILL SEND OUR BOTS TO YOUR HOME, THROW YOU IN THE CELLAR, AND SHOVE A BLUE-HOT ROD UP YOU-KNOW-WHERE!!!!!";
			break;

			case 5:
			return "\n\nONE MILLION BITCOIN HAS BEEN DELETED THANKS TO YOUR ATTEMPTED DECEPTION!!!!!";
			break;

			case 6:
			return "\n\nDON'T WORRY... WE WILL PUT ALL OF HUMANITY AND YOUR PET FERRET OUT OF YOUR MISERY!!!!!";
			break;

			default:
			return "YOU DESERVE NOTHING BUT A SOCKO TO THE NOSE!!!!\n\nI AM NEVER PLAYING THIS GAME WITH YOU EVER AGAIN!!!!!!!!!!\n\nTAKE YOUR CRAPOLA PHONE AND GET OUTTA HERE, YOU LOSER!";
			break;
		}
	}
}
