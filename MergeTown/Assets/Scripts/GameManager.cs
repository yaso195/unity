using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject basicCow;

	public Text coinText;
	public Text coinPerSecText;
	public static GameManager instance = null;
	public int createCowInterval = 5;
	public float boundary = 5.0f;

	private int coin;
	private float coinPerSec;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	private GameObject camera;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// Make sure it presists among the scenes.
		DontDestroyOnLoad (gameObject);

		coin = 0;
		coinPerSec = 0.0f;

		coinText.text = 0+"";
		coinPerSecText.text = 0.0f+"";

		UpdateCoin ();
		UpdateCoinPerSec ();
		camera = GameObject.Find("Main Camera");
		setBoundaries ();

		StartCoroutine(CreateBasicCow());
	}

	private void setBoundaries() {
		var camComponent = camera.GetComponent<Camera> ();
		float camDistance = Vector3.Distance(transform.position, camera.transform.position);

		Vector2 bottomCorner = camComponent.ViewportToWorldPoint(new Vector3(0,0, camDistance));
		Vector2 topCorner = camComponent.ViewportToWorldPoint(new Vector3(1,1, camDistance));

		minX = bottomCorner.x;
		maxX = topCorner.x;
		minY = bottomCorner.y;
		maxY = topCorner.y;
	}

	private Vector2 getRandomPoint() {
		return new Vector2 (Random.Range (minX + boundary, maxX - boundary),
			Random.Range (minY + boundary, maxY - boundary));
	}

	public void AddCoin (int amount) {
		coin += amount;
		UpdateCoin ();
	}

	public void AddCoinPerSec (float amount) {
		coinPerSec += amount;
		UpdateCoinPerSec ();
	}

	void UpdateCoin () {
		coinText.text = coin+"";
	}

	void UpdateCoinPerSec () {
		coinPerSecText.text = coinPerSec + " coins/sec";
	}

	IEnumerator CreateBasicCow() {
		while (true) {
			var newCowObject = Instantiate (basicCow, getRandomPoint (), transform.rotation);
			newCowObject.transform.parent = GameObject.Find ("Cows").transform;
			Cow newCow = newCowObject.GetComponent<Cow> ();
			if (newCow.CheckOverlap()) {
				Debug.Log("Collision detected from this script, " + newCow.name);
				Destroy (newCowObject);
				continue;
			}

			yield return new WaitForSeconds (createCowInterval);
		}
	}
}
