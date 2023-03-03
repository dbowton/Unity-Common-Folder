using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GPSPlayer : MonoBehaviour
{
    [SerializeField] GPS gps;
    Vector3 destination = Vector3.zero;

    CharacterController controller;

	private void Start()
	{
        controller= GetComponent<CharacterController>();

		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	void Update()
    {
		Vector3 movement = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) movement += transform.forward;
		if (Input.GetKey(KeyCode.S)) movement -= transform.forward;
		if (Input.GetKey(KeyCode.D)) movement += transform.right;
		if (Input.GetKey(KeyCode.A)) movement -= transform.right;

		if (Input.GetKey(KeyCode.UpArrow)) movement += transform.up;
		if (Input.GetKey(KeyCode.DownArrow)) movement -= transform.up;

		transform.localEulerAngles += Vector3.up * Input.GetAxis("Mouse X");
		controller.SimpleMove((Input.GetKey(KeyCode.LeftShift) ? 2 : 1) * 8f * movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            destination = Vector3.zero;
            destination.x = Random.Range(-24, 24);
            destination.y = transform.position.y;
            destination.z = Random.Range(-24, 24);

            gps.enabled = true;
            gps.SetLocation(destination);
        }

        if(Input.GetKeyDown(KeyCode.T))
            gps.enabled = !gps.enabled;

        if (Vector3.Distance(transform.position, destination) < 4)
        {
            gps.enabled = false;
        }
    }
}
