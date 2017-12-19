using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public AudioClip wallChop1;
	public AudioClip wallChop2;
	public Sprite dmgSprite;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public void DamageWall(int loss) {
		spriteRenderer.sprite = dmgSprite;
		SoundManager.instance.RandomizeSfx (wallChop1, wallChop2);
		hp -= loss;
		if (hp <= 0)
			gameObject.SetActive (false);
		

	}
}
