using System.Collections.Generic;
using UnityEngine;

public class EnviromentToggle : MonoBehaviour
{
    [SerializeField] private List<GameObject> enviroments;

    private void Update() {

        ToggleEnviroment();
    }

    private void ToggleEnviroment() {

        if (GameManager.instance.GetState() == GameState.Scenic)
        {
            enviroments[0].SetActive(false);//endless runner
            enviroments[1].SetActive(true);//endless walk
        }
        else
        {
            enviroments[0].SetActive(true);
            enviroments[1].SetActive(false);
        }
    }
}
