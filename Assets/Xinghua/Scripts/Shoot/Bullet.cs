using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 moveDirection;
    private float damage;
    public void Shoot(Vector3 direction, float amount, float bulletSpeed)
    {
        speed = bulletSpeed;
     
        moveDirection = direction.normalized;

        Destroy(gameObject, 3f);
    }
 
    private void FixedUpdate()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.gameObject.GetComponentInChildren<PlayerMovement>();
        if (player != null)
        {
            //player TakeDamage
            Destroy(gameObject);
        }

    }
}
