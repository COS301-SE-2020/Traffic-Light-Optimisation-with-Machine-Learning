/**
	@file ControlCamera.cs
*/

using UnityEngine;

/**
	This class contains the logic for controlling the camera
*/
public class ControlCamera : MonoBehaviour
{
    readonly float mainSpeed = 75.0f;
    readonly float minFOV = 15f;
    readonly float maxFOV = 90f;
    readonly float sensitivity = 40f;

	///Every frame this is called and checks for input, depending on input, the camera will move/rotate/zoom
    void Update()
    {
        float fov = Camera.main.fieldOfView;

        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;

        Vector3 p = new Vector3();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            p += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            p += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            p += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            p += new Vector3(1, 0, 0);
        }

        p = p * (mainSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position;
        transform.Translate(p);

        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        newPosition.x = Mathf.Clamp(newPosition.x, -100.0f, 100.0f);
        newPosition.z = Mathf.Clamp(newPosition.z, -100.0f, 100.0f);
        fov = Mathf.Clamp(fov, minFOV, maxFOV);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, Vector3.up, -mainSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, Vector3.up, mainSpeed * Time.deltaTime);
        }

        transform.position = newPosition;
        Camera.main.fieldOfView = fov;
    }
}
