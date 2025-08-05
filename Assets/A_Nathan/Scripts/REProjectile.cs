using UnityEngine;

public class REProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float distanceToTarget;
    public Vector3 targetPosition;
    Rigidbody body;
    public float forwardForce;
    public float upwardsForce;
    public bool shootHigh;
    Vector3 startPos;
    Vector3 endPos;
    public float damage;
    public Transform targetTransform;

    void Awake()
    {
        //distance to target 
    }
    public void Start()
    {
        targetPosition = targetTransform.position;
        transform.SetParent(null);
        distanceToTarget = Vector3.Distance(targetPosition, transform.position);
        body = GetComponent<Rigidbody>();
        startPos = transform.position;
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPosition, 25f, Physics.gravity,shootHigh);
        body.linearVelocity = velocity;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>()!=null) {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
                }
        Destroy(gameObject);
    }

    //senior GPT provided this. Much needed. MUCH NEEDED
    public static Vector3 CalculateLaunchVelocity(Vector3 startPos, Vector3 targetPos, float launchSpeed, Vector3 gravity, bool highArc)
    {
        Vector3 delta = targetPos - startPos;
        Vector3 deltaXZ = new Vector3(delta.x, 0f, delta.z);
        float y = delta.y;
        float xz = deltaXZ.magnitude;

        float g = -gravity.y;

        float speedSquared = launchSpeed * launchSpeed;
        float underSqrt = speedSquared * speedSquared - g * (g * xz * xz + 2 * y * speedSquared);

        // If underSqrt < 0, target is not reachable with this speed
        if (underSqrt < 0f)
        {
            return Vector3.zero;
        }

        float sqrt = Mathf.Sqrt(underSqrt);
        float angle;
        // Two possible angles: high arc or low arc
        float angleLow = Mathf.Atan2(speedSquared - sqrt, g * xz);
        float angleHigh = Mathf.Atan2(speedSquared + sqrt, g * xz);
        if (highArc)
        {
             angle = angleHigh; // You can switch to angleHigh for a higher arc

        }
        else
        {
             angle = angleLow;
        }

        Vector3 velocityXZ = deltaXZ.normalized * Mathf.Cos(angle) * launchSpeed;
        float velocityY = Mathf.Sin(angle) * launchSpeed;

        Vector3 finalVelocity = velocityXZ + Vector3.up * velocityY;
        return finalVelocity;
    }
}
