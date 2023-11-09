using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public IUserInput pi;
    public float horizontalSpeed = 50.0f;
    public float verticalSpeed = 30.0f;
    public float cameraDampValue = 0.1f;
    
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20.0f;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;
        
        playerHandle.transform.Rotate(Vector3.up,pi.JRight *Time.fixedDeltaTime * horizontalSpeed);
        tempEulerX -= pi.Jup * Time.fixedDeltaTime * verticalSpeed;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX,0,0);

        model.transform.eulerAngles = tempModelEuler;
        //camera.transform.position = Vector3.Lerp(camera.transform.position,transform.position,0.2f);
        camera.transform.position =
            Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        //camera.transform.eulerAngles = transform.eulerAngles;
        camera.transform.LookAt(cameraHandle.transform);
    }

    public void LockUnlock()
    {
        print("lockUnlock");
    }
}
