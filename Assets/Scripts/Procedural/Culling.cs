using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culling : MonoBehaviour
{
    private Camera cam;
    
    private void Start() {

        cam = Camera.main;
    }

    private void Update() {

        Renderer r = transform.GetComponent<Renderer>();
        r.enabled = IsVisible(cam, transform);
    }

    public static bool IsVisible(Camera c, Transform t) {

        var planes = GeometryUtility.CalculateFrustumPlanes(c); 
        var point = t.position;  

        foreach (var plane in planes)
        {
            if(plane.GetDistanceToPoint(point) < -5f)
                return false;
        }    
        return true;
    }

    public static bool IsVisible(Camera c, Vector3 t) {
        
        var planes = GeometryUtility.CalculateFrustumPlanes(c); 

        foreach (var plane in planes)
        {
            if(plane.GetDistanceToPoint(t) < -5f)
                return false;
        }    
        return true;
    }
}
