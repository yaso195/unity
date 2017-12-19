using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = 0f;
	public BoardManager boardScript;
	public static GameManager instance = null;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	int level = 1;
	List<Enemy> enemies;
	bool enemiesMoving;
	private Text levelText;
	GameObject levelImage;
	bool doingSetup;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// Make sure it presists among the scenes.
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	//this is called only once, and the paramter tell it to be called only after the scene was loaded
	//(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static public void CallbackInitialization()
	{
		//register the callback to be called everytime the scene is loaded
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		Debug.Log ("Level : " + instance.level);
		Debug.Log ("Scene Name : " + arg0.name);
		instance.level++;
		instance.InitGame();
	}

	void InitGame() {
		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);

		enemies.Clear ();
		boardScript.SetupScene (level);

		Invoke ("HideLevelImage", levelStartDelay);
	}

	private void HideLevelImage() {
		levelImage.SetActive (false);
		doingSetup = false;
	}


	public void GameOver () {
		enabled = false;
		levelText.text = "After " + level + " days, you starved.";
		levelText.fontSize = 16;
		levelImage.SetActive (true);
	}

	void Update () {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList(Enemy e) {
		enemies.Add (e);
	}

	IEnumerator MoveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);

		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}
		playersTurn = true;
		enemiesMoving = false;
	}
}
