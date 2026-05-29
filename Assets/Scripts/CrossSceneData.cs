using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class CrossSceneData : MonoBehaviour, ISavable
{
    public static CrossSceneData i;
    public bool clouds;
    public TimeOfDay time;
    public List<Light> lights;
    public ParticleSystem cloudsParticle;
    public List<int> highScores;
    public int shadowLevel;

    private void Awake() {

        if (i==null) i = this;
        else {
            if (i != this) 
                Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() {

        SetSetting();
        SetShadowLvl();
    }

    private void SetSetting() {

        for(int i = 0; i<lights.Count; i++)
        {
            if (i == (int)time) lights[i].gameObject.SetActive(true);
            else lights[i].gameObject.SetActive(false); 
        }

        cloudsParticle.gameObject.SetActive(clouds);
    }

    private void SetShadowLvl() {

        foreach (var light in lights)
        {
            if (shadowLevel == 0) 
                light.shadows = LightShadows.None;

            else if (shadowLevel == 1)
                light.shadows = LightShadows.Soft;
                
            else
                light.shadows = LightShadows.Hard;
        }
    }

    public object CaptureState() 
    {
        return highScores; 
    }

    public void RestoreState(object state)
    {
        highScores = (List<int>)state;
    }
}

public enum TimeOfDay
{
    Morning,
    Day,
    Evening,
}
