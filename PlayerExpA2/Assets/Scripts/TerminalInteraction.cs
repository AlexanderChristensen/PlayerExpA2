using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteraction : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject terminalCanvas;
    [SerializeField] GameObject openPanelCanvas;
    [SerializeField] GameObject player;

    [SerializeField] Animator openPanel;

    GrappleMovement grappleMovement;

    public bool interacting;

    bool terminalActive;
    bool closing;
    bool openeing;

    void Start()
    {
        terminalCanvas.SetActive(false);

        interacting = false;


        grappleMovement = player.GetComponent<GrappleMovement>();
    }

    void Update()
    {
        if (openPanelCanvas.activeInHierarchy && terminalActive)
        {
            if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("FinishedScreen"))
            {
                terminalCanvas.SetActive(true);
                openPanel.SetBool("Open", false);
                openeing = true;
            }

            if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("EmptyState") && openeing)
            {
                Time.timeScale = 0;
                openeing = false;
            }

                if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("EmptyState") && closing)
            {
                openPanelCanvas.SetActive(false);
                closing = false;
                terminalActive = false;
            }

            if (openPanel.GetCurrentAnimatorStateInfo(0).IsName("PanelClose"))
            {
                closing = true;
                openPanel.SetBool("Close", false);
            }
        }
    }

    public void Interact()
    {
        if (!interacting) 
        {
            openPanelCanvas.SetActive(true);
            openPanel.SetBool("Open", true);


            interacting = true;
            terminalActive = true;

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
