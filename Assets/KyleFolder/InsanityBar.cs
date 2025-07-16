using UnityEngine;
using UnityEngine.UI;

public class InsanityBar : MonoBehaviour
{
    public Slider slider;

    public void Update()
    {
        slider.value += Time.deltaTime;
        if (slider.value >= 10)
        {
            Debug.Log("Im insane now!!!!");
        }
    }


}
