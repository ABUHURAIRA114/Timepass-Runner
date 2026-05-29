using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputItem
{
    public string name;
    public List<KeyCode> keyCodes = new List<KeyCode>();  
    public event Action<KeyCode> OnKeyDown, OnKeyStay, OnKeyUp;

    public InputItem(string name, params KeyCode[] keys)
    {
        this.name = name;
        
        foreach (var key in keys)
        {
            keyCodes.Add(key);
        }
    }

    public void CheckKeyPress() 
    {
        for(int i = 0; i < keyCodes.Count; i++)
        {
            if (Input.GetKeyDown(keyCodes[i])) KeyDown(i);
            if (Input.GetKey(keyCodes[i])) KeyStay(i);
            if (Input.GetKeyUp(keyCodes[i])) KeyUp(i);
        }
    }

    public void KeyUp(int i)
    {
        OnKeyUp?.Invoke(keyCodes[i]);
    }

    public void KeyStay(int i)
    {
        OnKeyStay?.Invoke(keyCodes[i]);
    } 

    public void KeyDown(int i)
    {
        OnKeyDown?.Invoke(keyCodes[i]);
    }
}



