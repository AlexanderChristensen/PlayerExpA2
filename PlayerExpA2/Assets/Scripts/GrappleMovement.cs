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
    [SerializeField] float springStrength;
    [SerializeField] float damperStrength;

    [SerializeField] GameObject grapplePointModel;

    LineRenderer grappleLine;
    ConfigurableJoint joint;

    Rigidbody rb;

    bool grappling;
    bool zipping;
    bool jointSet;

    Vector3 grapplePointDirection;
    float grapplePointDistance;

    GameObject grappleInstance;

    public bool canMove;

    SoftJointLimitSpring springLimit;
    SoftJointLimit distLimit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleLine = GetComponent<LineRenderer>();
        joint = GetComponent<ConfigurableJoint>();

        SoftJointLimit distLimit = new SoftJointLimit();

        canMove = true;

        springLimit = joint.linearLimitSpring;

        distLimit = joint.linearLimit;
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
                    if (!jointSet)
                    {
                        ActivateJoint();
                    }
                }
                else
                {
                    if (jointSet)
                    {
                        DisconnectJoint();
                    }
                }  
            }
            else
            {
                grappleLine.enabled = false;

                if (jointSet)
                {
                    DisconnectJoint();
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

    void ActivateJoint()
    {
        grapplePointDistance = Vector3.Distance(transform.position, grappleInstance.transform.position);

        joint.connectedBody = grappleInstance.GetComponent<Rigidbody>();
        joint.anchor = grapplePointDirection;

        springLimit.spring = 0.1f;

        distLimit.limit = grapplePointDistance;
        joint.linearLimit = distLimit;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;


        Debug.Log(distLimit.limit);
        jointSet = true;
    }

    void DisconnectJoint()
    {
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;

        springLimit.spring = 0;

        distLimit.limit = 0;

        jointSet = false;
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

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("joint broke");
    }
}
