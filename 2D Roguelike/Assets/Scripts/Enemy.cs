using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
	public int playerDamage;
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

	Animator animator;
	Transform target;
	bool skipMove;

	protected override void Start() {
		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		if (skipMove) {
			skipMove = false;
			return;
		}

		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);
		skipMove = true;
	}

	public void MoveEnemy() {
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) 
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else 
			xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove<Player>(xDir, yDir);
	}

	protected override void OnCantMove<T> (T component)
	{
		Player p = component as Player;
		p.LoseFood (playerDamage);
		animator.SetTrigger ("enemyAttack");
		SoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);
	}
}
