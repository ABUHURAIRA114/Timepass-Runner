using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class _CharacterController : IObserverClass
{
    [Header("Value")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float dashForce;

    [Header("Reference")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerInputManager inputs;
    private float gravity = -11;
    private Vector2 movVectors;
    private bool isDashing;
    private bool isGrounded;
    private float timeOnGround = 0;
    private float horizontalMovement;

    private void Update() {

        if(isDashing)  movVectors.x = dashForce;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        SetValues();
        ApplyMovements();
    }

    public override void  OnNotify(Task t) {

        if(!gameObject.activeSelf)
            return;

        switch(t)
        {
            case Task.JumpStart:
                Jump();
            break;

            case Task.DashStart:
                DashStart();
            break; 

            case Task.Forward:
                Forward();
            break;   

            case Task.Backward:
                Backward();
            break;

            case Task.Stop:
                Stop();
            break;

            case Task.JumpStop:
                JumpStop();
            break;

            case Task.DashStop:
                DashStop();
            break;

            case Task.Grounded:
                isGrounded = true;
            break;

            case Task.unGrounded:
                isGrounded = false;
            break;
        }
    }

    private void ApplyMovements() {

        controller.Move(movVectors * Time.deltaTime);
    }

    private void SetValues() {

        timeOnGround = isGrounded ? timeOnGround + Time.deltaTime : 0;
    }

    private void Jump() {

        movVectors.y = jumpForce;
    }

    private void Forward() {

        if(isDashing) 
            return;

        horizontalMovement = speed / 2f;
    }

    private void Backward() {

        if(isDashing) return;

        if(isGrounded && timeOnGround > 0.5f) horizontalMovement = -speed;
        else  horizontalMovement = -speed / 3f;
    }

    private void DashStart() {

        movVectors.x = dashForce;
        isDashing = true;
    }

    private void Stop() {

        if(isDashing) return;

        horizontalMovement = 0;
    }

    private void JumpStop() {

        movVectors.y = gravity;
    }

    private void DashStop() {

        movVectors.x = horizontalMovement;
        isDashing = false;
    }
}


   