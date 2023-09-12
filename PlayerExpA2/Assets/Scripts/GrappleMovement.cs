using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMovement : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask grappleable;
    [SerializeField] float pullForce;
    [SerializeField] float pullAcceleration;
    [SerializeField] float grappleRange;

    [SerializeField] GameObject grapplePointModel;

    LineRenderer grappleLine;
    SpringJoint springJoint;

    Rigidbody rb;

    bool grappling;
    bool zipping;

    Vector3 grapplePointDirection;
    float grapplePointDistance;

    GameObject grappleInstance;

    public bool canMove;

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

                if (!zipping)
                { 
                    if (springJoint == null)
                    {
                        
                    }
                }
                else
                {
                    if (springJoint != null)
                    {
                        Destroy(springJoint);
                    }
                }  
            }
            else
            {
                grappleLine.enabled = false;

                if (GetComponent<HingeJoint>() != null)
                {
                    Destroy(GetComponent<HingeJoint>());
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (zipping & grappling)
        {
            float appliedPullForce = Mathf.Lerp(0, pullForce, pullAcceleration);

            rb.AddForce((grapplePointDirection - transform.position).normalized * appliedPullForce);
        }

        if (grappling && !zipping)
        {

        }

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

    void AddSpringJoint()
    {
        springJoint = gameObject.AddComponent<SpringJoint>();
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
