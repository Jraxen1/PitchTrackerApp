using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PitchManager : MonoBehaviour
{

	public enum gameStage {menu, playerInput, bidding, midRound};
	public gameStage currentStage;

	public GameObject menuCanvas;
	public GameObject playerInputCanvas;
	public GameObject biddingCanvas;
	public GameObject midRoundCanvas;
	public GameObject rundownCanvas;

	public bool isTeamGame;


	public List<string> pastActions;
	public List<string> pastActionsTeam1;
	public List<string> pastActionsTeam2;


	public string[] playerNames;

	public string biddingPlayer;

	bool biddingPlayerLoss;


	public int roundNumber = 0;


	public TextMeshProUGUI bidScore1;
	public TextMeshProUGUI bidScore2;


	// Playercount Variables
	public enum playerCounts { fourPlayers, threePlayers, fivePlayers, sixPlayers, twoTeams};
	public playerCounts playerCount;
	public TMP_Dropdown playerCountSelector;
	int PCS_Value;

	public GameObject[] stringEntryHolders;
	public TMP_InputField[] stringInputFields;


	// Bidding Variables

	public enum bidTypes { twoBid, threeBid, fourBid, smudge };
	public bidTypes currentBid;

	public TMP_Dropdown bidLevelSelector;
	int Bid_Value;

	public TMP_Dropdown bidPlayerSelector;
	int BidPlayerName_Value;

	// SCORE KEEPING

	public TextMeshProUGUI scoreName1;
	public TextMeshProUGUI scoreName2;

	public TextMeshProUGUI score1Text;
	public TextMeshProUGUI score2Text;

	public TextMeshProUGUI tempScore1Text;
	public TextMeshProUGUI tempScore2Text;


	public int tempScore1 = 0;
	public int tempScore2 = 0;
	public int score1 = 0;
	public int score2 = 0;

	public TMP_Dropdown scoreChange1;
	public int scoreChangeNum1;
	public TMP_Dropdown scoreChange2;
	public int scoreChangeNum2;

	public int scoreToAdd1 = 0;
	public int scoreToAdd2 = 0;

	//TRUMP MENU

	public Sprite[] trumpImages;
	public int currentTrumpPosition = 0;
	public Image trumpImage;

	// RUNDOWN

	public TextMeshProUGUI rundownTextTeam1;
	public TextMeshProUGUI rundownTextTeam2;

	// Start is called before the first frame update
	void Start()
    {
		menuCanvas.SetActive(true);
		playerInputCanvas.SetActive(false);
		biddingCanvas.SetActive(false);
		midRoundCanvas.SetActive(false);
		rundownCanvas.SetActive(false);

		
		//midRoundCanvas.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
    }

    public void movePlayerInput()
	{

		currentStage = gameStage.playerInput;
		menuCanvas.SetActive(false);
		playerInputCanvas.SetActive(true);


	}

    

    

    public void Round()
	{

	}

    public void SetPlayerCounts()
	{
        // Grabs the value that the dropdown was set to
		PCS_Value = playerCountSelector.value;
		playerCounts cPC = playerCounts.fourPlayers;

        // Sets the current player count variable to the correct enumerator
        if(PCS_Value == 0)
		{
			cPC = playerCounts.threePlayers;
			playerNames = new string[3];
		}else if(PCS_Value == 1)
		{
			cPC = playerCounts.fourPlayers;
			playerNames = new string[2];
		} else if(PCS_Value == 2)
		{
			cPC = playerCounts.fivePlayers;
			playerNames = new string[5];
		} else if(PCS_Value == 3)
		{
			cPC = playerCounts.sixPlayers;
			playerNames = new string[6];
		}
		else if (PCS_Value == 4)
		{
			cPC = playerCounts.twoTeams;
			playerNames = new string[2];
		}


		// Sets the global playercount variable to the correct #
		playerCount = cPC;
		if (PCS_Value == 4)
		{
			EnablePlayerNameEntryFields(PCS_Value + 3,true);
			isTeamGame = true;
		}
		else
		{
			EnablePlayerNameEntryFields(PCS_Value + 3,false);
			isTeamGame = false;
		}
		

	}

    public void EnablePlayerNameEntryFields(int numberToEnable, bool isTeams)
	{
		if (isTeams)
		{
			numberToEnable = 2;
		}
        for(int i = 0; i < 6; i++)
		{
			stringEntryHolders[i].SetActive(false);
		}

        for(int i = 0; i < numberToEnable; i++)
		{
			stringEntryHolders[i].SetActive(true);
		}
	}

	public void moveBidding()
	{
		int PCSCHANGED = PCS_Value + 3;
		if (isTeamGame)
		{
			PCSCHANGED = 2;
		}
		playerNames = new string[PCSCHANGED];
        for(int i = 0; i < PCSCHANGED; i++)
		{
			playerNames[i] = stringInputFields[i].text;
			//print("PLAYERNAME ADDED " + playerNames[i]);
		}
		SetUpPlayerMenu();
		biddingCanvas.SetActive(true);

		bidScore1.text = "" + score1;
		bidScore2.text = "" + score2;


		playerInputCanvas.SetActive(false);
	}

    public void SetBidNumber()
	{
		// Grabs the value that the dropdown was set to
		Bid_Value = bidLevelSelector.value;
		bidTypes cBid = bidTypes.twoBid;

		// Sets the current player count variable to the correct enumerator
		if (Bid_Value == 0)
		{
			cBid = bidTypes.twoBid;
		}
		else if (Bid_Value == 1)
		{
			cBid = bidTypes.threeBid;
		}
		else if (Bid_Value == 2)
		{
			cBid = bidTypes.fourBid;
		}
		else if (Bid_Value == 3)
		{
			cBid = bidTypes.smudge;
		}

		//print("CURRENT BID SET TO " + cBid);
		currentBid = cBid;
	}

    public void SetUpPlayerMenu()
	{
		bidPlayerSelector.ClearOptions();
		List<string> optionsToAdd = new List<string>();

        for(int i = 0; i < playerNames.Length; i++)
		{
			optionsToAdd.Add(playerNames[i]);
		}
		bidPlayerSelector.AddOptions(optionsToAdd);
	}

    public void SetBiddingPlayer()
	{
		BidPlayerName_Value = bidPlayerSelector.value;
		string cPlayer = "";
		cPlayer = playerNames[BidPlayerName_Value];
		biddingPlayer = cPlayer;
		//print("BIDDING PLAYER IS " + cPlayer);
	}

	public void moveStartRound()
	{
		SetBiddingPlayer();
		SetBidNumber();
		biddingCanvas.SetActive(false);
		midRoundCanvas.SetActive(true);
		SetUpRoundInfo();
	}

    public void MoveBiddingFromRound()
	{
		bool isPosT1 = true;
		bool isPosT2 = true;

		pastActions.Add(" - Round " + roundNumber + " - ");

		if (biddingPlayerLoss)
		{
            if(biddingPlayer == playerNames[0])
			{
				pastActions.Add(biddingPlayer + " lost their " + currentBid + " and went from " + score1 + " to " + (score1 + scoreToAdd1));
				isPosT1 = false;
			}
			else if (biddingPlayer == playerNames[1])
			{
				pastActions.Add(biddingPlayer + " lost their " + currentBid + " and went from " + score2 + " to " + (score2 + scoreToAdd2));
				isPosT2 = false;
			}
		}
		else if(biddingPlayer == playerNames[0])
		{
			pastActions.Add(biddingPlayer + " won their " + currentBid + " and went from " + score1 + " to " + (score1 + scoreToAdd1));
		} else if(biddingPlayer == playerNames[1])
		{
			pastActions.Add(biddingPlayer + " won their " + currentBid + " and went from " + score2 + " to " + (score2 + scoreToAdd2));
		}

        if(biddingPlayer != playerNames[0])
		{
            if(scoreToAdd1 != 0)
			{
				pastActions.Add(playerNames[0] + " got " + scoreToAdd1 + " points and went from " + score1 + " to " + (score1 + scoreToAdd1));
			}
		} else if(biddingPlayer != playerNames[1])
		{
            if(scoreToAdd2 != 0)
			{
				pastActions.Add(playerNames[1] + " got " + scoreToAdd2 + " points and went from " + score2 + " to " + (score2 + scoreToAdd2));
			}
		}

		if (isPosT1)
		{
			pastActionsTeam1.Add("----" + "\n" + score1 + "\n" + "+" + scoreToAdd1);
		}
		else
		{
			pastActionsTeam1.Add("----" + "\n" + score1 + "\n"+ scoreToAdd1);
		}

		if (isPosT2)
		{
			pastActionsTeam2.Add("----" + "\n" + score2 + "\n" + "+" + scoreToAdd2);
		}
		else
		{
			pastActionsTeam2.Add("----" + "\n" + score2 + "\n" + scoreToAdd2);
		}
       

        

		//pastActions.Add(playerNames[0] + " had a score change of " + scoreToAdd1 + ", going from " + score1 + " to " + (score1 + scoreToAdd1));
		//pastActions.Add(playerNames[1] + " had a score change of " + scoreToAdd2 + ", going from " + score2 + " to " + (score2 + scoreToAdd2));



		score1 += tempScore1;
		score2 += tempScore2;

		midRoundCanvas.SetActive(false);
		SetScoreChange();
		moveBidding();
	}

    public void AdjustFinalScores()
	{
		pastActions.Add(" - Final Round - ");
		bool isPosT1 = true;
		bool isPosT2 = true;

		if (biddingPlayerLoss)
		{
			if (biddingPlayer == playerNames[0])
			{
				pastActions.Add(biddingPlayer + " lost their " + currentBid + " and went from " + score1 + " to " + (score1 + scoreToAdd1));
				isPosT1 = false;
			}
			else if (biddingPlayer == playerNames[1])
			{
				pastActions.Add(biddingPlayer + " lost their " + currentBid + " and went from " + score2 + " to " + (score2 + scoreToAdd2));
				isPosT2 = false;
			}

		}
		else if (biddingPlayer == playerNames[0])
		{
			pastActions.Add(biddingPlayer + " won their " + currentBid + " and went from " + score1 + " to " + (score1 + scoreToAdd1));
		}
		else if (biddingPlayer == playerNames[1])
		{
			pastActions.Add(biddingPlayer + " won their " + currentBid + " and went from " + score2 + " to " + (score2 + scoreToAdd2));
		}

		if (biddingPlayer != playerNames[0])
		{
			if (scoreToAdd1 != 0)
			{
				pastActions.Add(playerNames[0] + " got " + scoreToAdd1 + " points and went from " + score1 + " to " + (score1 + scoreToAdd1));
			}
		}
		else if (biddingPlayer != playerNames[1])
		{
			if (scoreToAdd2 != 0)
			{
				pastActions.Add(playerNames[1] + " got " + scoreToAdd2 + " points and went from " + score2 + " to " + (score2 + scoreToAdd2));
			}
		}

		if (isPosT1)
		{
			pastActionsTeam1.Add("----" + "\n" + score1 + "\n" + "+" + scoreToAdd1);
		}
		else
		{
			pastActionsTeam1.Add("----" + "\n" + score1 + "\n" + (score1+scoreToAdd1));
		}

		if (isPosT2)
		{
			pastActionsTeam2.Add("----" + "\n" + score2 + "\n" + "+" + scoreToAdd2);
		}
		else
		{
			pastActionsTeam2.Add("----" + "\n" + score2 + "\n" + (score2+scoreToAdd2));
		}


		//pastActions.Add(playerNames[0] + " had a score change of " + scoreToAdd1 + ", going from " + score1 + " to " + (score1 + scoreToAdd1));
		//pastActions.Add(playerNames[1] + " had a score change of " + scoreToAdd2 + ", going from " + score2 + " to " + (score2 + scoreToAdd2));



		score1 += tempScore1;
		score2 += tempScore2;

		SetScoreChange();
	}

    public void SetScoreChange()
	{
		scoreToAdd1 = 0;
		scoreToAdd2 = 0;

		switch (scoreChange1.value)
		{
			case 0:
				scoreToAdd1 = 0;
				break;
			case 1:
				scoreToAdd1 = 1;
				break;
			case 2:
				scoreToAdd1 = 2;
				break;
			case 3:
				scoreToAdd1 = 3;
				break;
			case 4:
				scoreToAdd1 = 4;
				break;
		}

		switch (scoreChange2.value)
		{
			case 0:
				scoreToAdd2 = 0;
				break;
			case 1:
				scoreToAdd2 = 1;
				break;
			case 2:
				scoreToAdd2 = 2;
				break;
			case 3:
				scoreToAdd2 = 3;
				break;
			case 4:
				scoreToAdd2 = 4;
				break;
		}

		biddingPlayerLoss = false;

        if(biddingPlayer == playerNames[0])
		{
            if(currentBid == bidTypes.twoBid && scoreToAdd1 < 2)
			{
				scoreToAdd1 = -2;
				biddingPlayerLoss = true;
			} else if(currentBid == bidTypes.threeBid && scoreToAdd1 < 3)
			{
				scoreToAdd1 = -3;
				biddingPlayerLoss = true;
			} else if(currentBid == bidTypes.fourBid && scoreToAdd1 < 4)
			{
				scoreToAdd1 = -4;
				biddingPlayerLoss = true;
			}
		} else if(biddingPlayer == playerNames[1])
		{
			if (currentBid == bidTypes.twoBid && scoreToAdd2 < 2)
			{
				scoreToAdd2 = -2;
				biddingPlayerLoss = true;
			}
			else if (currentBid == bidTypes.threeBid && scoreToAdd2 < 3)
			{
				scoreToAdd2 = -3;
				biddingPlayerLoss = true;
			}
			else if (currentBid == bidTypes.fourBid && scoreToAdd2 < 4)
			{
				scoreToAdd2 = -4;
				biddingPlayerLoss = true;
			}
		}

		

		

		tempScore1 = scoreToAdd1;
		tempScore2 = scoreToAdd2;

		UpdateRoundInfo(tempScore1, tempScore2);

	}

	public void SetUpRoundInfo()
	{
        if(playerCount == playerCounts.twoTeams)
		{
			scoreName1.text = playerNames[0];
			scoreName2.text = playerNames[1];
			score1Text.text = score1 + "";
			score2Text.text = score2 + "";
		}

		roundNumber++;

		UpdateRoundInfo(0,0);

		//CycleTrump();
		
	}

    public void UpdateRoundInfo(int sc1, int sc2)
	{
        

		if (playerCount == playerCounts.twoTeams)
		{
			if(sc1 >= 0)
			{
				tempScore1Text.text = "+" + sc1 + "";
			}
			else
			{
				tempScore1Text.text = sc1 + "";
			}

            if(sc2 >= 0)
			{
				tempScore2Text.text = "+" + sc2 + "";
			}
			else
			{
				tempScore2Text.text = sc2 + "";
			}
			
			
		}
	}

    public void CycleTrump()
	{
        if(currentTrumpPosition == 3)
		{
			currentTrumpPosition = 0;
		}
		else
		{
			currentTrumpPosition++;
		}

		trumpImage.sprite = trumpImages[currentTrumpPosition];

	}

    public void RestartGame()
	{
		menuCanvas.SetActive(true);
		playerInputCanvas.SetActive(false);
		biddingCanvas.SetActive(false);
		midRoundCanvas.SetActive(false);
		rundownCanvas.SetActive(false);

		score1 = 0;
		score2 = 0;

		roundNumber = 0;

		pastActions.Clear();
		pastActionsTeam1.Clear();
		pastActionsTeam2.Clear();
	}

    public void MoveToRundown(bool isEnding)
	{
		midRoundCanvas.SetActive(false);
		biddingCanvas.SetActive(false);
		rundownCanvas.SetActive(true);

		if (isEnding)
		{
			AdjustFinalScores();
		}
		

		string rundownTeam1String = playerNames[0] + "\n";

		string rundownTeam2String = playerNames[1] + "\n";
		for (int i = 0; i < pastActionsTeam1.Count; i++)
		{
			rundownTeam1String += pastActionsTeam1[i] + "\n";
		}

		for (int i = 0; i < pastActionsTeam2.Count; i++)
		{
			rundownTeam2String += pastActionsTeam2[i] + "\n";
		}


		rundownTeam1String += "" + score1;
		rundownTeam2String += "" + score2;

		rundownTextTeam1.text = rundownTeam1String;
		rundownTextTeam2.text = rundownTeam2String;

	}

    public void MoveBackToGame(bool goGame)
	{
		if (goGame)
		{
			midRoundCanvas.SetActive(true);
		}
		else
		{
			biddingCanvas.SetActive(true);
		}
		rundownCanvas.SetActive(false);
		
	}

    

}

