using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLimits : MonoBehaviour
{
    [SerializeField] private Collider leftLimiter;
    [SerializeField] private Collider rightLimiter;

    private float height;
    private float leftLimit;
    private float rightLimit;

    private void Update()
    {
        CalculateHeight();
    }

    private void CalculateHeight()
    {
        
    }

}
