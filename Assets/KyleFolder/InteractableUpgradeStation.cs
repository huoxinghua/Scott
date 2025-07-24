using UnityEngine;
using UnityEngine.UI;

public class InteractableUpgradeStation : MonoBehaviour
{
    public GameObject InteractPrompt;
    private bool playerInRange = false;
    public GameObject UpgardePrompt;

    void Start()
    {
        UpgardePrompt.SetActive(false);
        if (InteractPrompt != null)
        {
            InteractPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        InteractPrompt.SetActive(false);
        UpgardePrompt.SetActive(true);  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractPrompt.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractPrompt.SetActive(false);
            UpgardePrompt.SetActive(false);
        }
    }

}
