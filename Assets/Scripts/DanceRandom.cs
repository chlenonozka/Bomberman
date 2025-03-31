using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceRandom : StateMachineBehaviour
{
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("Danceid", Random.Range(0, 9));
    }
}
