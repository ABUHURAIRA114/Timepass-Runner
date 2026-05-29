using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStates : MonoBehaviour, IHitable
{
    [SerializeField] private float currentHealth = 100;
    private readonly float maxHealth = 100;
    public float healthNormalized;
    [SerializeField] private PlayerInputManager inputs;
    private PlayerInput PlayerInput;
    private PlayerInputs PlayerInputs;

    private bool isDashing;
    private bool canDash;
    private float currentDashStamina;
    private float maxDashStamina = 1;
    public bool shouldDamage => !isDashing;

    private bool isGrounded;

    private bool hasJumped;
    private float timeAfterJump = 0;
    private int jumps;

    [SerializeField] private List<IObserverClass> observers = new List<IObserverClass>();

    private void Awake()
    {
        PlayerInputs = new();
    }
    private void OnEnable() {
        
        if (!GameManager.instance.scenic)
        {
             PlayerInputs.Enable();
            
             PlayerInputs.Movement.Forward.performed += OnForward;
             PlayerInputs.Movement.Backward.performed += OnBackward;
             PlayerInputs.Movement.Jump.performed += OnJumpStart;
             PlayerInputs.Movement.Dash.performed += OnDash;

             PlayerInputs.Movement.Forward.canceled += OnStop;
             PlayerInputs.Movement.Backward.canceled += OnStop;
            
        }
    }

    private void OnDisable() {


        PlayerInputs.Movement.Forward.performed -= OnForward;
        PlayerInputs.Movement.Backward.performed -= OnBackward;
        PlayerInputs.Movement.Jump.performed -= OnJumpStart;
        PlayerInputs.Movement.Dash.performed -= OnDash;

        PlayerInputs.Movement.Forward.canceled -= OnStop;
        PlayerInputs.Movement.Backward.canceled -= OnStop;
        PlayerInputs.Disable();
    }

    private void Update() 
    {
        if (GameManager.instance.GetState() == GameState.Started)
        {
            currentHealth = maxHealth;
            currentDashStamina = maxDashStamina;
        }

        healthNormalized = currentHealth / maxHealth;
        CheckGrounded();
        JDStop();
        JDTimers();

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Notify(Task.isDead);
            GameManager.instance.SetState(GameState.Dead);
        }

        if(isGrounded && !hasJumped)
            jumps = 0;
    }

    private void JDTimers()
    {
        if (!isDashing && currentDashStamina < maxDashStamina)
        {
            float incRate = (int)Task.DashStop;
            currentDashStamina += (incRate/50) * Time.deltaTime;
            canDash = false;
        }
        else    
            canDash = true;
        
        if (currentDashStamina <= 0)
            isDashing = false;

        if(isDashing)
            currentDashStamina -= (int)Task.DashStart * Time.deltaTime;

        timeAfterJump += Time.deltaTime;

        if (timeAfterJump >= .35f)
            hasJumped = false;
    }

    private void JDStop()
    {
        if(!isDashing) 
            Notify(Task.DashStop);
        
        if(!hasJumped)
            Notify(Task.JumpStop);
    }
    
    private void CheckGrounded()
    {
        RaycastHit hit;
        float distance = .25f;
        Vector3 origin = transform.position;

        if (Physics.Raycast(origin, Vector3.down, out hit, distance))
        {
            isGrounded = true;

            Notify(Task.Grounded);
        }
        else
        {
            isGrounded = false;

            Notify(Task.unGrounded);
        }
    }

    public void OnJumpStart(InputAction.CallbackContext c)
    {
        if (jumps < 2 && !hasJumped)
        {
            hasJumped = true;
            timeAfterJump = 0;
            jumps++;

            Notify(Task.JumpStart);
        }
    }

    public void OnDash(InputAction.CallbackContext c)
    {
        if (canDash && !hasJumped)
            isDashing = true;

        if(isDashing)
            Notify(Task.DashStart);
    }

    public void OnForward(InputAction.CallbackContext c)
    {
        if(isDashing) return;

        Notify(Task.Forward);
    }

    public void OnBackward(InputAction.CallbackContext c)
    {
        if(isDashing) return;

        Notify(Task.Backward);    
    }

    public void OnStop(InputAction.CallbackContext c)
    {
        Notify(Task.Stop);
    }

    public void OnHitObstacle(float damage)
    {
        currentHealth -= damage;
        Notify(Task.OnHit);

        foreach (var observer in observers)
        {
            observer.OnHit(damage);
        } 
    }

    private void Notify(Task t)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(t);
        } 
    }
}

[Serializable]
public class IObserverClass :  MonoBehaviour, IObserver
{
    public virtual void OnNotify(Task t){}
    public virtual void OnHit(float Damage){}
}

public interface IHitable
{
    public bool shouldDamage {get;}
    public void OnHitObstacle(float damage);
}

public interface IObserver
{
    public void OnNotify(Task t);
}

public enum Task 
{
    JumpStart,
    JumpStop,
    DashStart = 4,
    DashStop,
    Forward,
    Backward,
    Stop,
    Grounded,
    unGrounded,
    OnHit,
    isDead
}
