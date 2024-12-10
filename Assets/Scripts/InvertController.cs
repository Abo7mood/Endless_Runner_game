using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertController : MonoBehaviour
{
    PlayerMovement playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isInvert = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isInvert = false;

        }
    }
}
