using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class AssignTargetForLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform target;
    public MultiAimConstraint aimConstraint;
    public Animator animator;
   public RigBuilder rigBuilder;
    void Start()
    {

        target = GameObject.Find("FirstPersonController").transform;
        aimConstraint = GetComponent<MultiAimConstraint>();
        WeightedTransformArray sources = aimConstraint.data.sourceObjects;
        sources.SetTransform(0,target);
        aimConstraint.data.sourceObjects = sources;
        RebuildRig();
    }
    void RebuildRig()
    {
       

        if (animator != null && rigBuilder != null)
        {
            animator.enabled = false;
            rigBuilder.Build();
            animator.enabled = true;
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
