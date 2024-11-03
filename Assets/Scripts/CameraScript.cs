using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float offset = 5;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 4f;
    private float maxZoom = 15f;
    private float camVelocity = 0f;
    

    [SerializeField] Camera cam;



    // Start is called before the first frame update
    void Start()
    {
     zoom = cam.orthographicSize;   
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = new Vector3(player.position.x, player.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref camVelocity, smoothSpeed);
    }
}
