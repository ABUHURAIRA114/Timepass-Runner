using UnityEngine;
using System.Collections.Generic;

public class MultiMenus : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new();
    private int currentMenu;

    private void Start() {
        
        ToggleMenus();
    }

    public void AppNegMenu(int i) { //Append or negate currentMenu from button
        
        currentMenu = Mathf.Clamp(currentMenu + i, 0, menus.Count-1);
        ToggleMenus();
    }

    private void ToggleMenus() {

        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(false);

            if (i == currentMenu) 
                menus[i].SetActive(true);
        }
    }
}
