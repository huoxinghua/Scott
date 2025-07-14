using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakePositionAmount = 0.1f;
    public Vector3 shakeRotationAmount = new Vector3(2f, 2f, 2f);
    internal bool isShake = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine currentShake;

    public void Shake()
    {

        if (currentShake != null)
        {
            StopCoroutine(currentShake);
            ResetTransform();
        }

        currentShake = StartCoroutine(ShakeRoutine());

    }

    private void ResetTransform()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
    private IEnumerator ShakeRoutine()
    {
        Debug.Log("camera shake");
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            transform.localPosition = originalPosition + Random.insideUnitSphere * shakePositionAmount;
            transform.localRotation = originalRotation * Quaternion.Euler(
                Random.Range(-shakeRotationAmount.x, shakeRotationAmount.x),
                Random.Range(-shakeRotationAmount.y, shakeRotationAmount.y),
                Random.Range(-shakeRotationAmount.z, shakeRotationAmount.z)
            );
            isShake = false;
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        Debug.Log("camera shake pos");
    }
}
