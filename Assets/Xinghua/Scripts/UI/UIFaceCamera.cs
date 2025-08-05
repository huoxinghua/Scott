using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    void Update()
    {
        var cam = Camera.main;
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                             cam.transform.rotation * Vector3.up);
        }
    }
}
