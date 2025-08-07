using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RetrunToMenu());
    }

    IEnumerator RetrunToMenu()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("XHMainMenu");
    }
}


