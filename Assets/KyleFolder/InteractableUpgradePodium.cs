using UnityEngine;
using UnityEngine.UI;

public class InteractableUpgradePodium : MonoBehaviour
{
    public GameObject InteractPrompt;
    private bool playerInRange = false;
    public GameObject UpgardePanel;
    public GameObject ConfirmButton;
    public GameObject CancelButton;

    void Start()
    {
        UpgardePanel.SetActive(false);
        ConfirmButton.SetActive(false);
        CancelButton.SetActive(false);
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
        ConfirmButton.SetActive(true);
        CancelButton.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractPrompt.SetActive(true);
            UpgardePanel.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractPrompt.SetActive(false);
            UpgardePanel.SetActive(false);
        }
    }

    public void ConfirmOption()
    {
        ConfirmButton.SetActive(false);
        CancelButton.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CancelOption()
    {
        ConfirmButton.SetActive(false);
        CancelButton.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
