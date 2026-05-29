using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObject : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private bool spawnObj;

    private GameObject[] spawnPoints;

    private void Start() 
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private void Update() 
    {
        foreach(GameObject spo in spawnPoints)
        {
            if(Vector3.Distance(endPoint.position, spo.transform.position) <= 10)
            {
                spawnObj = true;
            }
        }
    }

    public IEnumerator OnRemove()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }

    public Transform EndPoint
    {
        get{ return endPoint; }
    }

    public bool SpawnObj
    {
        get{ return spawnObj; }
    }
}

