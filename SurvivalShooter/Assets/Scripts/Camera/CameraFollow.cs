using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;

	Vector3 offset;

	void Start () {
		offset = transform.position - target.position;
	}

	void FixedUpdate() {
		Vector3 targetCamPos = target.position + offset;
		// Move the camera to its new position smoothly. This is done through Lerp.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}