using UnityEngine;

public class ToggleControlsLayout : MonoBehaviour
{
    public GameObject Title;
    public GameObject MainMenuButtons;
    public GameObject Controls;
    public GameObject Inputs;
    public GameObject BackButton;

    private void Start()
    {
        Controls.SetActive(false);
        BackButton.SetActive(false);
        Inputs.SetActive(false);
    }

    public void ControlsDisplay()
    {
        Title.SetActive(false);
        MainMenuButtons.SetActive(false);
        Controls.SetActive(true);
        BackButton.SetActive(true);
        Inputs.SetActive(true);
    }
    public void BackToMainMenu()
    {
        Title.SetActive(true);
        MainMenuButtons.SetActive(true);
        Controls.SetActive(false);
        BackButton.SetActive(false);
        Inputs.SetActive(false);
    }
}
