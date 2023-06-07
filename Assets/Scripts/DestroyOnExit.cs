using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.runtimeAnimatorController.name);
        if (animator.runtimeAnimatorController.name.Contains("Teleport"))
        {
            MapManager.player.Invoke("show", animator.GetCurrentAnimatorStateInfo(0).length - 0.5f);
            Debug.Log("teleport");
        }else if (animator.runtimeAnimatorController.name.Contains("Fly"))
        {
            Destroy(animator.gameObject.transform.parent.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            return;
        }
        
        Destroy(animator.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        
    }
}
