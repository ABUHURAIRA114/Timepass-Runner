using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerToggle : MonoBehaviour
{
    private void Start() {

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        SceneManager.activeSceneChanged += ToggleChildObjects;
    }

    private void ToggleChildObjects(Scene args0, Scene args1) {
        
        if (args1.buildIndex == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        else if (GameManager.instance.scenic)
        {
            transform.GetChild(1).position = new Vector3(0, 0, 0);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (args1.buildIndex != 0 || GameManager.instance.GetState() == GameState.Restarting)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = new Vector3(0, 0, 0);
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
