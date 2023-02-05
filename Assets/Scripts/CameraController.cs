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
    private float defaultZoom;

    float targetZoom;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponent<Camera>();
        recenterCoords = playerCam.transform.position;
        defaultZoom = playerCam.orthographicSize;
        targetZoom = defaultZoom;
    }

    // Update is called once per frame
    void Update()
    {
        playerCam.orthographicSize = Mathf.Lerp(playerCam.orthographicSize, targetZoom, Time.deltaTime * 10f);
    }

    public void OnCameraPan(InputAction.CallbackContext context)
    {
        
        Vector2 MoveDirection = context.ReadValue<Vector2>();
        MoveDirection *= playerCam.orthographicSize;
        playerCam.transform.position = new Vector3(MoveDirection.x, MoveDirection.y, 0) * camSpeed + playerCam.transform.position;
        
    }

    public void OnCameraZoom(InputAction.CallbackContext context)
    {
        float MoveDirection = context.ReadValue<Vector2>().y;
        if (targetZoom >= minZoom)
        {
            targetZoom += zoomSpeed * MoveDirection;
            //playerCam.transform.position = new Vector3(0, 0, MoveDirection) * camSpeed + playerCam.transform.position;
        }
        else if(MoveDirection > 0)
        {
            targetZoom += zoomSpeed * MoveDirection;
            //playerCam.transform.position = new Vector3(0, 0, MoveDirection) * camSpeed + playerCam.transform.position;
        }
        targetZoom = Mathf.Clamp(targetZoom, minZoom, 100);

    }

    public void RecenterCall(InputAction.CallbackContext context)
    {
        OnRecenterCamera();
    }

    public void OnRecenterCamera()
    {
        playerCam.transform.position = recenterCoords;
        targetZoom = defaultZoom;
    }
}
