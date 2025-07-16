using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRBs;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        RagdollState(false);
    }

    public void AvtivateRagdoll(Vector3 hitDir, Vector3 hitPoint, float hitForce)
    {
        RagdollState(true);
        anim.enabled = false;

        Rigidbody closestRB = null;
        float closestDistance = 0f;
        foreach(Rigidbody rb in ragdollRBs)
        {
            float distance = Vector3.Distance(rb.position, hitDir);

            if(closestRB == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestRB = rb;
            }
        }

        closestRB.AddForceAtPosition(hitDir * hitForce, hitPoint, ForceMode.Impulse);
    }

    private void RagdollState(bool active)
    {
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = active;
        }
        
        foreach(Rigidbody rb in ragdollRBs)
        {
            rb.isKinematic = !active;
        }
    }
}
