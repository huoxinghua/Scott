using UnityEngine;

public class EnableShotgunOnStateEnter : StateMachineBehaviour
{
    public string shotgunObjectName = "Shotgun_rig"; 

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform shotgunTransform = animator.transform.Find(shotgunObjectName);
        if (shotgunTransform != null)
        {
            shotgunTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Shotgun GameObject not found: " + shotgunObjectName);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform shotgunTransform = animator.transform.Find(shotgunObjectName);
        if (shotgunTransform != null)
        {
            shotgunTransform.gameObject.SetActive(false);
        }
    }
}
