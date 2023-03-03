using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SensorPlayer : MonoBehaviour
{
	CharacterController controller;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Vector3 movement = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) movement += transform.forward;
		if (Input.GetKey(KeyCode.S)) movement -= transform.forward;
		if (Input.GetKey(KeyCode.D)) movement += transform.right;
		if (Input.GetKey(KeyCode.A)) movement -= transform.right;

		if(Input.GetKey(KeyCode.UpArrow)) movement += transform.up;
		if (Input.GetKey(KeyCode.DownArrow)) movement -= transform.up;

		transform.localEulerAngles +=  Vector3.up * Input.GetAxis("Mouse X");

		controller.SimpleMove((Input.GetKey(KeyCode.LeftShift) ? 2 : 1) * 8f * movement);
	}
}
