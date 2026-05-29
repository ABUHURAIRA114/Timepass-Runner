using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class UI : IObserverClass
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    // [SerializeField] private VolumeProfile profile;
    // [SerializeField] private AnimationCurve animationGraph;
    [SerializeField] private float animSpeed = 1;
    // private Vignette hitVignette;
    private float hitTime;
    private float hitIntensityLastTime;

    private float currentHealth;
    private float maxHealth = 100;

    private bool isDashing;
    private float currentDashStamina = 1;

    private void Start() 
    {
        hitTime = 10;
        // profile.TryGet(out hitVignette);
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (GameManager.instance.GetState() == GameState.Started)
        {
            currentHealth = maxHealth;
            currentDashStamina = 1;
            healthBar.rectTransform.localScale = new Vector3(1, 1, 1);
            staminaBar.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        // hitTime += Time.deltaTime*animSpeed;
        // hitVignette.intensity.value = animationGraph.Evaluate(hitTime-hitIntensityLastTime);
        DashSet();
    }

    public override void OnNotify(Task t)
    {
        switch (t)
        {
            case Task.OnHit:
                hitTime = 0;
                hitIntensityLastTime = hitTime;
                break;

            case Task.DashStart:
                DashStart();
                break;

            case Task.DashStop:
                DashStop();
                break;
        }
    }

    public override void OnHit(float Damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - Damage, 0, 100);
    
        float healthNormalized = currentHealth/maxHealth;
       
        healthBar.rectTransform.localScale = new Vector3(healthNormalized, 1, 1);
    }

    private void DashStart()
    {
        isDashing = true;
    }

    private void DashStop()
    {
        isDashing = false;
    }

    private void DashSet() 
    {
        if (!isDashing)
        {
            float incRate = (int)Task.DashStop;
            currentDashStamina += (incRate/50) * Time.deltaTime;
        }
        else
            currentDashStamina -= (int)Task.DashStart * Time.deltaTime;

        currentDashStamina = Mathf.Clamp(currentDashStamina, 0, 1);

        staminaBar.rectTransform.localScale = new Vector3(currentDashStamina, 1, 1);
    }
}
