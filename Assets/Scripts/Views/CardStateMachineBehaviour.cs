using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<CardView>().OnDestroyAnimationFinished();
    }
}
