using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
	[System.Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;

	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	Transform boardHolder;
	List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		gridPositions.Clear ();
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns+1; x++) {
			for (int y = -1; y < rows+1; y++){
				GameObject inst = floorTiles[Random.Range(0, floorTiles.Length)];
				if (x == -1 || y == -1 || x == columns || y == rows) {
					inst = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				GameObject instance = Instantiate(inst, new Vector3 (x,y,0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tiles, int min, int max){
		int tileCount = Random.Range (min, max + 1);

		for (int i = 0; i < tileCount; i++) {
			Vector3 pos = RandomPosition ();
			GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
			Instantiate(tileChoice, pos, Quaternion.identity);
		}
	}

	public void SetupScene (int level) {
		BoardSetup ();
		InitializeList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate(exit, new Vector3(columns - 1, rows -1, 0f), Quaternion.identity);
	}
}
