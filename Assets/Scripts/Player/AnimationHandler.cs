using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : IObserverClass
{
    private bool jumping;
    private bool isGrounded;
    private bool isRunning;
    private bool isDashing;

    [SerializeField] private Animator anim;
    [SerializeField] private List<RuntimeAnimatorController> animCont;

    private void OnEnable() {
    
        anim.runtimeAnimatorController = GameManager.instance.scenic ? animCont[1] : animCont [0];
    }

    private void Update() {

        if ( !GameManager.instance.scenic )
            SetAnim();
    }

    public override void OnNotify(Task t) {

        switch(t)
        {
            case Task.JumpStart:
                jumping = true;
                isRunning = false;
                isGrounded = false;
            break;

            case Task.JumpStop:
                jumping = false;
                isRunning = true;
            break;

            case Task.DashStart:
                isDashing = true;
            break;
            
            case Task.DashStop:
                isDashing = false;
            break;

            case Task.Grounded:
                isGrounded = true;
                isRunning = true;
            break;

            case Task.unGrounded:
                isGrounded = false;
                isRunning = false;
            break;
        }
    }

    private void SetAnim() {

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("Jumping", jumping);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isDashing", isDashing);
    }
}
