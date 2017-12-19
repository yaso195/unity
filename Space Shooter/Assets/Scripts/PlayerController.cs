﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private AudioSource aSource;

	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;

	public float fireRate;
	private float nextFire;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		aSource = GetComponent<AudioSource> ();
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3 (
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler (0, 0, rb.velocity.x * -tilt);
	}

	void Update () {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			//GameObject clone = 
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation); // as GameObject;
			aSource.Play ();
		}
	}
}
