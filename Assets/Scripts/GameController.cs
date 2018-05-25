using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

	public struct Scores{
		
		public int p1Score;
		public int p2Score;

		public Scores(int p1Score, int p2Score){
			this.p1Score = p1Score; 
			this.p2Score = p2Score; 
		}
	}
	public static GameController Instance;
	
	public enum GameStateEnum{ Idle, GameStart, Running, P1Scored, P2Scored, P1Won, P2Won, Paused};
	private GameStateEnum gameState;
	public Scores scores;
	
	public Text p1ScoreText;
	public Text p2ScoreText;
	public Text p1ScoredText;
	public Text p2ScoredText;
	public Text p1WonText;
	public Text p2WonText;
	public Button tapToStartButton;
	public Text newGameText;
	public Ball ball;
	public P1Controller p1Controller;
	public P2Controller p2Controller;

	public GameStateEnum GameState
	{
		get { return gameState; }
		
		set {gameState = SetGameState(value); }
	}

   public string LastPlayerHit { get; internal set; }

   void Awake () {
		if (Instance == null)
		{
			Instance = this;
		}
	}
	// Use this for initialization
	void Start () {
		scores = new Scores(0,0);
		GameState = GameStateEnum.GameStart;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(){
		 Debug.Log("GameState"+GameState);
		if(GameState == GameStateEnum.P1Scored){
			ball.SetInitialDirection(-1);
			GameState = GameStateEnum.Running;
		}

		if(GameState == GameStateEnum.P2Scored){
			ball.SetInitialDirection(1);
			GameState = GameStateEnum.Running;
		}
		if(GameState == GameStateEnum.GameStart){
		 Debug.Log("trigger enter");
			ball.SetInitialDirection(UnityEngine.Random.Range(0,2) > 0.5 ? -1 : 1);
			GameState = GameStateEnum.Running;
		}
		if (GameState == GameStateEnum.P1Won || GameState == GameStateEnum.P2Won)
		{
			scores.p1Score = 0;
			scores.p2Score = 0;
			GameState = GameStateEnum.GameStart;			
		}
	}

   internal void MissileCollectibleHit(int lastPLayerHit)
   {
	throw new NotImplementedException();
   }

   internal void LaserCollectibleHit(int lastPLayerHit)
   {
      throw new NotImplementedException();
   }

   internal void ShieldCollectibleHit(int lastPLayerHit)
   {
      throw new NotImplementedException();
   }

   void AddScore(int player){
		switch (player){
			case 1:
				scores.p1Score++;
				break;
			case 2:
				scores.p2Score++;
				break;
			default:
				break;		
		}		
	}
	public GameStateEnum SetGameState( GameStateEnum newState )
	{		
		if(newState == gameState)
			return newState;
		// set the new gameState
		gameState = newState;
		
		// call functions that only fire once on state change
		switch (gameState)
		{
			case GameStateEnum.GameStart:				
				scores.p1Score = 0;
				scores.p2Score = 0;
				p1ScoreText.text = scores.p1Score.ToString();
				p2ScoreText.text = scores.p2Score.ToString();
				p1ScoredText.gameObject.SetActive(false);
				p2ScoredText.gameObject.SetActive(false);
				p1WonText.gameObject.SetActive(false);
				p2WonText.gameObject.SetActive(false);
				newGameText.gameObject.SetActive(false);
				tapToStartButton.gameObject.SetActive(true);
				break;
			case GameStateEnum.Running:
				p1ScoredText.gameObject.SetActive(false);
				p2ScoredText.gameObject.SetActive(false);
				tapToStartButton.gameObject.SetActive(false);
				p1WonText.gameObject.SetActive(false);
				p2WonText.gameObject.SetActive(false);
				break;
			case GameStateEnum.P1Scored:
				p1ScoredText.gameObject.SetActive(true);
				tapToStartButton.gameObject.SetActive(true);
				AddScore(1);				
				p1ScoreText.text = scores.p1Score.ToString();
				break;
			case GameStateEnum.P2Scored:
				p2ScoredText.gameObject.SetActive(true);
				tapToStartButton.gameObject.SetActive(true);
				AddScore(2);
				p2ScoreText.text = scores.p2Score.ToString();
				break;
			case GameStateEnum.P1Won:					
				p1ScoredText.gameObject.SetActive(false);
				p1WonText.gameObject.SetActive(true);
				newGameText.gameObject.SetActive(true);
				break;
			case GameStateEnum.P2Won:
				p2ScoredText.gameObject.SetActive(false);
				p2WonText.gameObject.SetActive(true);
				newGameText.gameObject.SetActive(true);
				break;
			case GameStateEnum.Paused:
				break;
		}	

		return newState;		
	}
	
}
