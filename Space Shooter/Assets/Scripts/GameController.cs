sing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GUIText mergedHouseText;

	private int mergedHouse = 0;

	void Start() {
		mergedHouse = 0;

		mergedHouseText.text = "";

		UpdateMergedHouse ();
	}

	public void AddMergedHouse (int newValue) {
		mergedHouse += newValue;
		UpdateMergedHouse ();
	}

	void UpdateMergedHouse () {
		mergedHouseText.text = "Merged House : " + mergedHouse;
	}
}
