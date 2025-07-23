using UnityEngine;

public class REProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float distanceToTarget;
    public Vector3 targetPosition;
    Rigidbody body;
    public float forwardForce;
    public float upwardsForce;
    Vector3 startPos;
    Vector3 endPos;
    [SerializeField] Transform targetTransform;

    void Start()
    {
        //distance to target 
    }
    public void Awake()
    {
        targetPosition = targetTransform.position;
        distanceToTarget = Vector3.Distance(targetPosition, transform.position);
        body = GetComponent<Rigidbody>();
      //  transform.LookAt(targetPosition);
        Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
        startPos = transform.position;
      //  body.AddForce(transform.forward * forwardForce*distanceToTarget + transform.up * upwardsForce);
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPosition, 20f, Physics.gravity);
        body.linearVelocity = velocity;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 0)
        {
            endPos = transform.position;
            Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
            Debug.Log("Distance Traveled = " + Vector3.Distance(startPos, endPos));
        }
    }

    //senior GPT provided this. Much needed. MUCH NEEDED
    public static Vector3 CalculateLaunchVelocity(Vector3 startPos, Vector3 targetPos, float launchSpeed, Vector3 gravity)
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

        // Two possible angles: high arc or low arc
        float angleLow = Mathf.Atan2(speedSquared - sqrt, g * xz);
        float angleHigh = Mathf.Atan2(speedSquared + sqrt, g * xz);

        float angle = angleHigh; // You can switch to angleHigh for a higher arc

        Vector3 velocityXZ = deltaXZ.normalized * Mathf.Cos(angle) * launchSpeed;
        float velocityY = Mathf.Sin(angle) * launchSpeed;

        Vector3 finalVelocity = velocityXZ + Vector3.up * velocityY;
        return finalVelocity;
    }
}
