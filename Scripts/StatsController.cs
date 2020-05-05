using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour {

	[SerializeField]
	private Text timesYouWonText, timesYouLostText, timesYouWonButCheatedText, timesYouLostButLetCPUWinText, timesYouCheatedText, playerRank;

	// Use this for initialization
	void Start () {
		StartCoroutine(SetFieldsCoroutine());
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetFields() {
		timesYouWonText.text = "Times You Won: " + GameController.instance.numberOfWins.ToString();
		timesYouLostText.text = "Times You Lost: " + GameController.instance.numberOfLosses.ToString();
		timesYouWonButCheatedText.text = "Times You Won But Cheated: " + GameController.instance.numberOfWinCheats.ToString();
		timesYouLostButLetCPUWinText.text = "Times You Let The Computer Win: " + GameController.instance.numberOfYouLetWin.ToString();
		timesYouCheatedText.text = "Times You Cheated You Stinking Loser!!!: " + GameController.instance.numberOfCheats.ToString();
		playerRank.text = "Player Rank: " + SetPlayerRank(); //Add a function that returns a string
	}

	IEnumerator SetFieldsCoroutine() {
		yield return new WaitForSecondsRealtime(1f);
		SetFields();
	}

	public string SetPlayerRank() {
		if(GameController.instance.numberOfCheats == 1) {
			return "CHEATER";
		} else if (GameController.instance.numberOfCheats == 2) {
			return "BIG STINKIN' LOSER!!!!!";
		} else if (GameController.instance.numberOfCheats == 3) {
			return "ABSOLUTE LAME-O!!!!!!!!!!!";
		} else if (GameController.instance.numberOfCheats == 4) {
			return "WUSSY!";
		} else if (GameController.instance.numberOfCheats >= 5 && GameController.instance.numberOfCheats <= 9) {
			return "WHY ARE YOU STILL PLAYING THIS?";
		} else if (GameController.instance.numberOfCheats >= 10) {
			return "GO HOME LOSER";
		} else if(GameController.instance.numberOfWins > 0 && GameController.instance.numberOfLosses == 0) {
			return "WINNING!... BUT I STILL THINK YOU CHEAT"; //Have a function that gives random rank
		} else if(GameController.instance.numberOfWins > GameController.instance.numberOfLosses) {
			return "WINNING!.... MAYBE";
		} else if(GameController.instance.numberOfWins < GameController.instance.numberOfLosses) {
			return "FAIL! BWAHAHAHA!";
		} else if(GameController.instance.numberOfWins == GameController.instance.numberOfLosses && GameController.instance.numberOfWins > 0) {
			return "A DRAW... BUT NOT FOR LONG.";
		} else {
			return "NOVICE";
		}
	}
}
