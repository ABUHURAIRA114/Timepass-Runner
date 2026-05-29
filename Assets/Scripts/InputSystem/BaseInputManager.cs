using System.Collections.Generic;
using UnityEngine;

public class BaseInputManager : MonoBehaviour
{
    public List<InputItem> inputItems = new List<InputItem>();

    private void Update() {

        if (GameManager.instance.GetState() == GameState.Paused 
        || GameManager.instance.GetState() == GameState.None 
        || GameManager.instance.GetState() == GameState.Dead)
            return;

        foreach (InputItem inputItem in inputItems)
            inputItem.CheckKeyPress();
    }

    public InputItem GetInput(string name) {
        
        foreach (InputItem inputItem in inputItems)
            if (name.Equals(inputItem.name))
                return inputItem;

        Debug.LogError($"Input Item with name {name} not found");
        
        return null;
    }
}
