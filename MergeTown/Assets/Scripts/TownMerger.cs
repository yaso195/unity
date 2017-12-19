using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMerger : MonoBehaviour {
	public int houseCount = 1;
	public float collectFee = 100.0f;

	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{	
		TownMerger t = other.gameObject.GetComponent<TownMerger> ();
		if (t.CompareTag ("PickUp") && houseCount == t.houseCount) {
			t.gameObject.SetActive (false);
			GameManager.instance.AddMergedHouse (houseCount);
			houseCount *= 2;
		}
	}


	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, 45) * Time.deltaTime);
	}
}
