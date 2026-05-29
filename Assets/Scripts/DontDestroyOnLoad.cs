using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }
}
