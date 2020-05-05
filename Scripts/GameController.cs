using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// This is a singleton
	public static GameController instance;

	public static string YouWon = "TimesYouWon";
	public static string YouLost = "TimesYouLost";
	public static string YouWonButCheated = "TimesYouWonButCheated";
	public static string YouLostButLetCPUWin = "TimesYouLostButLetCPUWin";
	public static string YouCheatedFool = "TimesWeCaughtYouCheating";
	public static string YouWussedOut = "YouWussedOut";
	public static string StartGame = "StartGame";
	public static string SoundOnValue = "SoundOnValue";

	public int minValue,		// Lowest value of guess
		maxValue,				// Highest value 
		previousGuess, 			// Second previous guess it made
		secondPreviousGuess,	// Third previous guess it made 
		lastGuess;				// Last guess it made. If the last guess is between the previous guess and second previous guess by one,
								//		(e.g. guess is 212 and the previous guesses are 213 and 211), and you lie to it, it will declare
								//		you a CHEATER.

	public int numberOfGuesses;	// Number of guesses computer has.
	public int playerValue;		// The player's input. The computer will try to guess this number.
	public int maxSliderValue;	// Max value on the slider.

	public int soundOn;			// 0 = off, 1 = on

	//public int [] guesses;		// Stores the previous guesses -- may not be necessary

	//These are the min and max values for guesses when you start up the game
	const int minUpperValue = 5;
	const int maxUpperValue = 100000;
	const int minGuessesPossible = 3;
	const int maxGuessesPossible = 20;

	public bool youWin,		// The computer didn't guess your number and you won
		youLost,			// The computer guessed your number and lost
		youWinByCheating,	// You cheated but the computer didn't know
		youLetWin,			// You let the computer win
		youCheated;			// YOU CHEATED and you lose. Cheat too many times (3) and the the game won't let you play anymore (without resetting).
							// 		It catches you if (a) a guess it made previously matches your number, (b) the number is out of range of the previous
							//		guesses (e.g. it knows it's a number between 200 and 210, but you lied and said it was, say, 100 or 500)
							//		or (c) its two previous guesses each minus its current guess is the absolute value of 1.
	public bool youCheatedAgain; // Do it a second time...
	public bool youCheatedAThirdTime; // and a third in a row...

	public int wussedOut;	// If you leave the game before you clear it (it is 1 when you start a game and goes to 0 when you finish)

	bool computerGetsYou;	// If you cheat, the computer will get your number the second time. This turns off if it wins (without cheating).
							//		If this is on, the computer will say stuff like, "Oh, it's THE CHEATER again."

	// Stats from PlayerPrefs
	public int numberOfWins,	// Number of times you win
		numberOfLosses,		// Number of times you lost
		numberOfWinCheats,	// Number of times you cheated and got away with it (also a win)
		numberOfYouLetWin,	// Number of times you let the computer win (also a loss)
		numberOfCheats;		// Number of times the computer caught you CHEATING (also a loss). Three cheats and Mr. Computer will NEVER play with
							//		you EVER again. (Unless you reset your stats.)

	void Awake() {
		MakeSingleton();
		LoadSound();
	}

	// Use this for initialization
	void Start () {
		if(!PlayerPrefs.HasKey(StartGame)) {
			PlayerPrefs.SetInt(StartGame, 713);
			Reset();
			Debug.Log("Reset!");
		} else {
			Debug.Log("Key is already set up!");
		}
		SetTotals();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeSingleton() {
		if(instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void Reset() {
		PlayerPrefs.SetInt(YouWon, 0);
		PlayerPrefs.SetInt(YouLost, 0);
		PlayerPrefs.SetInt(YouWonButCheated, 0);
		PlayerPrefs.SetInt(YouLostButLetCPUWin, 0);
		PlayerPrefs.SetInt(YouCheatedFool, 0);
		PlayerPrefs.SetInt(YouWussedOut, 0);
		Debug.Log("Game reset!");
	}

	public void SetTotals() {
		numberOfWins = PlayerPrefs.GetInt(YouWon);
		numberOfLosses = PlayerPrefs.GetInt(YouLost);
		numberOfWinCheats = PlayerPrefs.GetInt(YouWonButCheated);
		numberOfYouLetWin = PlayerPrefs.GetInt(YouLostButLetCPUWin);
		numberOfCheats = PlayerPrefs.GetInt(YouCheatedFool);
		wussedOut = PlayerPrefs.GetInt(YouWussedOut);
	}

	public void SaveTotals() {
		PlayerPrefs.SetInt(YouWon, numberOfWins);
		PlayerPrefs.SetInt(YouLost, numberOfLosses);
		PlayerPrefs.SetInt(YouWonButCheated, numberOfWinCheats);
		PlayerPrefs.SetInt(YouLostButLetCPUWin, numberOfYouLetWin);
		PlayerPrefs.SetInt(YouCheatedFool, numberOfCheats);
		PlayerPrefs.SetInt(SoundOnValue, soundOn);
	}

	public void ClearCheats() {//You must watch an ad to clear cheats
		numberOfCheats = 0;
		PlayerPrefs.SetInt(YouCheatedFool, numberOfCheats);
	}

	public void SaveWuss() {
		PlayerPrefs.SetInt(YouWussedOut, wussedOut);
	}

	public void SaveSound() {
		PlayerPrefs.SetInt(SoundOnValue, soundOn);
	}

	public void LoadSound() {
		soundOn = PlayerPrefs.GetInt(SoundOnValue);
	}
}
