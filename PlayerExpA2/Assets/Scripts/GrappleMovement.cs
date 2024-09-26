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

    [SerializeField] float velocitySampleFrequency;

    LineRenderer grappleLine;

    Rigidbody rb;

    bool grappling;
    bool zipping;

    Vector3 grapplePointDirection;

    GameObject grappleInstance;

    public bool canMove;

    float lastTickVelocity;
    [HideInInspector] public float acceleration;
    [HideInInspector] public float velocity;

    float velocitySampleTimer;

    FMOD.Studio.EventInstance zipSound;

    bool playingZip;

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
                    FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerSounds/GrappleFire");
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

                if (playingZip)
                {
                    zipSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    zipSound.release();

                    playingZip = false;
                }

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

        SampleVelocity();
    }

    void FixedUpdate()
    {
        if (zipping & grappling)
        {
            float appliedPullForce = Mathf.Lerp(0, pullForce, pullAcceleration);

            rb.AddForce((grapplePointDirection - transform.position).normalized * appliedPullForce);

            if (!playingZip)
            {
                zipSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/Zip");
                zipSound.start();

                playingZip = true;
            }

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

    void SampleVelocity()
    {
        if (velocitySampleTimer <= 0)
        {
            velocity = rb.velocity.magnitude;

            velocityText.text = velocity.ToString("F2") + "m/s";
            accelerationText.text = acceleration.ToString("F2") + "m/s/s";

            velocitySampleTimer = velocitySampleFrequency;
        }
        else
        {
            velocitySampleTimer -= Time.deltaTime;
        }
    }
}
