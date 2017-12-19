using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour {
	public int cowCount = 1;
	public int clickCoin = 1;
	public int collectCoin = 1;
	public int collectCoinSecond = 2;

	public GameObject nextCow;

	private Vector3 screenPoint;
	private Vector3 offset;
	Rigidbody2D rb;
	SpriteRenderer sr;
	private int screenBoundsWidth;
	private int screenBoundsHeight;
	private int boundary = 20;

	private Vector2 currentPos;
	private CircleCollider2D cc;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer> ();
		cc = GetComponent<CircleCollider2D> ();
		screenBoundsWidth = Screen.width;
		screenBoundsHeight = Screen.height;

		StartCoroutine(AddCoin());

		GameManager.instance.AddCoinPerSec ((float) collectCoin / (float)collectCoinSecond);
	}

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		sr.sortingOrder = 1;

		// Get the current position when it is clicked
		currentPos.x = transform.position.x;
		currentPos.y = transform.position.y;
	}

	void OnMouseDrag()
	{	
		if ((Input.mousePosition.x <= screenBoundsWidth - boundary)
		    && (Input.mousePosition.y <= screenBoundsHeight - boundary)
		    && (Input.mousePosition.x >= boundary)
		    && (Input.mousePosition.y >= boundary)) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}

	void OnMouseUp()
	{
		sr.sortingOrder = 0;
		if (transform.position.x == currentPos.x && transform.position.y == currentPos.y) {
			GameManager.instance.AddCoin (clickCoin);
		}
	}

	void OnTriggerStay2D(Collider2D other) 
	{	
		if (other != null) {
			Cow t = other.gameObject.GetComponent<Cow> ();
			if (t.CompareTag ("PickUp") && cowCount == t.cowCount && Input.GetMouseButtonUp (0)) {
				Destroy (other.gameObject);
				var newCow = Instantiate (nextCow, transform.position, transform.rotation);
				newCow.transform.parent = GameObject.Find ("Cows").transform;
				Destroy (gameObject);
				cowCount *= 2;
			}
		}
	}


	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, 45) * Time.deltaTime);
	}

	void OnDestroy() {
		GameManager.instance.AddCoinPerSec (-1 * (float) collectCoin / (float)collectCoinSecond);
	}

	IEnumerator AddCoin() {
		yield return new WaitForSeconds (collectCoinSecond);
		while (true) {
			GameManager.instance.AddCoin (collectCoin);
			yield return new WaitForSeconds (collectCoinSecond);
		}
	}

	public bool CheckOverlap() {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		Collider2D[] results = Physics2D.OverlapCircleAll (pos, cc.radius);
		for (int i = 0; i < results.Length; i++) {
			if (results [i].gameObject.GetComponent<Cow> ().CompareTag ("PickUp") && 
				results[i].gameObject != gameObject) {
				return true;
			}
		}

		return false;
	}
}
