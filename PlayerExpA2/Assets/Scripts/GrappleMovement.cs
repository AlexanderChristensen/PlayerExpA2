using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrappleMovement : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask grappleable;
    [SerializeField] float pullForce;
    [SerializeField] float pullAcceleration;
    [SerializeField] float grappleRange;
    [SerializeField] float springStrength;
    [SerializeField] float damperStrength;

    [SerializeField] TMP_Text velocityText;
    [SerializeField] TMP_Text accelerationText;

    [SerializeField] GameObject grapplePointModel;

    LineRenderer grappleLine;

    Rigidbody rb;

    bool grappling;
    bool zipping;

    Vector3 grapplePointDirection;

    GameObject grappleInstance;

    public bool canMove;

    float lastTickVelocity;
    float acceleration;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleLine = GetComponent<LineRenderer>();


        canMove = true;
    }


    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!grappling)
                {
                    CastRay();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                grappling = false;

                Destroy(grappleInstance);
            }

            if (Input.GetMouseButton(1))
            {
                zipping = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                zipping = false;
            }

            if (grappling)
            {
                DrawLine();
            }
            else
            {
                grappleLine.enabled = false;
            }
        }

        velocityText.text = rb.velocity.magnitude.ToString("F2") + "m/s";
        accelerationText.text = acceleration.ToString("F2") + "m/s/s";
    }

    void FixedUpdate()
    {
        if (zipping & grappling)
        {
            float appliedPullForce = Mathf.Lerp(0, pullForce, pullAcceleration);

            rb.AddForce((grapplePointDirection - transform.position).normalized * appliedPullForce);
        }

        acceleration = Mathf.Abs((rb.velocity.magnitude - lastTickVelocity) / Time.fixedDeltaTime);
        lastTickVelocity = rb.velocity.magnitude;   
    }


    void CastRay()
    {
        RaycastHit hit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, grappleRange, grappleable))
        {
            grapplePointDirection = hit.point;

            grappleInstance = Instantiate(grapplePointModel, grapplePointDirection, Quaternion.identity);

            grappling = true;
        }
    }

    void DrawLine()
    {
        grappleLine.enabled = true;

        grappleLine.SetPosition(0, transform.position);
        grappleLine.SetPosition(1, grapplePointDirection);
    }

    public void Freeze()
    {
        rb.velocity = Vector3.zero;
    }

    public void HaltMovement()
    {
        canMove = false;
        grappling = false;
    }

    public void ContinueMovement()
    {
        canMove = true;
    }
}
