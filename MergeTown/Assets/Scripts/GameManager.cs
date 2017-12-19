using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text mergedHouseText;
	public static GameManager instance = null;

	private int mergedHouse = 0;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// Make sure it presists among the scenes.
		DontDestroyOnLoad (gameObject);

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
