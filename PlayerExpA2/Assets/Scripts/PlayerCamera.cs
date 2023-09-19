using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] Transform orientation;

    [SerializeField] GameObject player;
    
    GrappleMovement grappleMovement;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        grappleMovement = player.GetComponent<GrappleMovement>();
    }

    void Update()
    {
        if (grappleMovement.canMove) 
        {
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            yRotation += mouseX;

            // rotate cam and player
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
