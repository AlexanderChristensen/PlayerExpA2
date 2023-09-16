using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteraction : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject terminalCanvas;
    [SerializeField] GameObject player;

    [SerializeField] Animator openPanel;

    GrappleMovement grappleMovement;

    public bool interacting;

    void Start()
    {
        terminalCanvas.SetActive(false);

        interacting = false;


        grappleMovement = player.GetComponent<GrappleMovement>();
    }

    void Update()
    {
        if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("FinishedScreen"))
        {
            terminalCanvas.SetActive(true);
            openPanel.SetBool("Open", false);

            if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("EmptyState"))
            {
                Time.timeScale = 0;
            }

            if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("PanelClose"))
            {
                openPanel.SetBool("Close", false);
            }
        }
    }

    public void Interact()
    {
        if (!interacting) 
        {
            openPanel.SetBool("Open", true);


            interacting = true;

            grappleMovement.Freeze();
            grappleMovement.HaltMovement();
        }
    }

    public void Exit()
    {
        terminalCanvas.SetActive(false);

        interacting = false;

        grappleMovement.ContinueMovement();

        Time.timeScale = 1;

        openPanel.SetBool("Close", true);
    }
}
