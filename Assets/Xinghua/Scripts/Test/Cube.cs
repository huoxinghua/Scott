using UnityEngine;

public class Cube : MonoBehaviour,IDamageable
{
    public void TakeDamage(float a)
    {
        Debug.Log("TakeDamage ");
        Destroy(gameObject);
    }
}
