using UnityEngine;

public class PlayerInputManager : BaseInputManager
{
    private void Awake() {
        
        inputItems.Add(CreateInput("Forward", KeyCode.D, KeyCode.RightArrow, KeyCode.Mouse4));
        inputItems.Add(CreateInput("Backward", KeyCode.A, KeyCode.LeftArrow, KeyCode.Mouse3));
        inputItems.Add(CreateInput("Jump", KeyCode.W, KeyCode.Space, KeyCode.UpArrow, KeyCode.Mouse0));
        inputItems.Add(CreateInput("Dash", KeyCode.S, KeyCode.LeftShift, KeyCode.RightShift, KeyCode.DownArrow, KeyCode.Mouse1));
    }

    private InputItem CreateInput(string name, params KeyCode[] keys) {
        return new InputItem(name, keys);
    }
}
