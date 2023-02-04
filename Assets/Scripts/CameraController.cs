using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float camSpeed = 10;
    public float zoomSpeed = 1;
    public float minZoom = -20;
    private Camera playerCam;
    private Vector3 recenterCoords;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponent<Camera>();
        recenterCoords = playerCam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCameraPan(InputAction.CallbackContext context)
    {
        
        Vector2 MoveDirection = context.ReadValue<Vector2>();
        MoveDirection *= playerCam.orthographicSize;
        playerCam.transform.position = new Vector3(MoveDirection.x, MoveDirection.y, 0) * camSpeed + playerCam.transform.position;
        
    }

    public void OnCameraZoom(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>().y);
        float MoveDirection = context.ReadValue<Vector2>().y;
        if (playerCam.orthographicSize >= minZoom)
        {
            playerCam.orthographicSize += zoomSpeed * MoveDirection;
            //playerCam.transform.position = new Vector3(0, 0, MoveDirection) * camSpeed + playerCam.transform.position;
        }
        else if(MoveDirection > 0)
        {
            playerCam.orthographicSize += zoomSpeed * MoveDirection;
            //playerCam.transform.position = new Vector3(0, 0, MoveDirection) * camSpeed + playerCam.transform.position;
        }
        playerCam.orthographicSize = Mathf.Clamp(playerCam.orthographicSize, minZoom, 100);

    }

    public void RecenterCall(InputAction.CallbackContext context)
    {
        OnRecenterCamera();
    }

    void OnRecenterCamera()
    {
        playerCam.transform.position = recenterCoords;
    }
}