using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    List<GameObject> objectsInRange = new List<GameObject>();

    [SerializeField] TMP_Text interactText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectsInRange.Count > 0)
            {
                objectsInRange[0].gameObject.GetComponent<IInteractable>().Interact();
            }
        }

        if (objectsInRange.Count > 0)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
