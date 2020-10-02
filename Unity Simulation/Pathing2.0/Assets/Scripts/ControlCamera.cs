using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    readonly float mainSpeed = 75.0f;
    readonly float minFOV = 15f;
    readonly float maxFOV = 90f;
    readonly float sensitivity = 40f;

    bool allowMovement = false;

    bool movecamera = false;

    float movementTimeRemaining = 4;
    bool movementTimerIsRunning = false;

    public Transform startCameraPosition;
    public Transform endCameraPosition;

    void Update()
    {
        if(allowMovement){
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

        if(movecamera){
            if(!allowMovement){
                transform.position = Vector3.Lerp(
                    transform.position, endCameraPosition.position, Time.deltaTime);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation, endCameraPosition.rotation, Time.deltaTime);
            }
        } else{
            transform.position = Vector3.Lerp(
                transform.position, startCameraPosition.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, startCameraPosition.rotation, Time.deltaTime);
        }

        if (movementTimerIsRunning)
        {
            if (movementTimeRemaining > 0)
            {
                movementTimeRemaining -= Time.deltaTime;
            }
            else
            {
                allowMovement = true;
                movementTimerIsRunning = false;
                movementTimeRemaining = 4;
            }
        }
    }

    public void Transition(){
        movecamera = true;
        movementTimerIsRunning = true;
    }
    public void DeTransition(){
        movecamera = false;
        allowMovement = false;
    }
}
