using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectSpawn : MonoBehaviour
{
    private enum SpawnType
    {
        EndToEnd,
        Random
    }

    [Header("SpawnReferences")]
    [SerializeField] private List<Transform> objPrefabs;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform despawnPoint;

    [SerializeField] private List<Transform> objects = new List<Transform>();

    [Header("TimeBeforeSpawn")]
    [SerializeField] private float maxTime;
    private float currentMaxTime;
    private float currentMinTime;
    private float currentTime;

    [Header("Speed")]
    [SerializeField] private float startSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedIncrement;
    private float currentSpeed;

    [Header("Spawn")]
    [SerializeField] private SpawnType spawnType;

    private event Action OnEvent;
    public static Action<float> OnHit;

    private void Start()
    {
        currentMaxTime = maxTime;
        OnEvent += Randomizations;
        OnHit += HitPunishment;
        OnEvent?.Invoke();
        currentSpeed = startSpeed;
    }

    private void Update()
    {
        SpawnTimer();
        ObjSimulate();
        SpeedSet();

        if(GameManager.instance.GetState() == GameState.Dead && currentSpeed>0)
            currentSpeed -= currentSpeed*Time.deltaTime*1.4f;
    }

    private float randomTime;
    private int randomObj;
    private void SpawnTimer()
    {
        currentTime += Time.deltaTime;

        if (spawnType == SpawnType.Random)
        {
            if (currentTime >= randomTime)
            {
                OnEvent?.Invoke();
                SpawnObj();
                currentTime = 0;
            }
        }
        else
        {
            if (objects.Count > 0 && objects[objects.Count - 1].GetComponent<ProceduralObject>().SpawnObj)
            {
                OnEvent?.Invoke();
                SpawnObj();
                currentTime = 0;
            }
            else if (objects.Count == 0)
            {
                OnEvent?.Invoke();
                SpawnObj();
                currentTime = 0;
            }
        }
    }

    private void Randomizations()
    { 
        randomObj = UnityEngine.Random.Range(0, objPrefabs.Count);

        float min = Mathf.Min(currentMinTime, currentMaxTime);
        float max = Mathf.Max(currentMinTime, currentMaxTime);

        randomTime = UnityEngine.Random.Range(min, max);
        randomTime = Mathf.Clamp(randomTime, min, currentMaxTime);
    }

    private void SpawnObj()
    {
        Transform obj = Instantiate(objPrefabs[randomObj]);

        if (spawnType == SpawnType.EndToEnd)
        {
            if (objects.Count > 0)
                obj.position = new Vector3(objects[objects.Count - 1].GetComponent<ProceduralObject>().EndPoint.position.x, 0, objects[objects.Count - 1].GetComponent<ProceduralObject>().EndPoint.position.z);
            else
                obj.position = startPoint.position;
        }
        else
            obj.position = startPoint.position;
        

        obj.rotation = startPoint.rotation;

        ProceduralObject proceduralObject = obj.GetComponent<ProceduralObject>();

        if (proceduralObject == null)
            proceduralObject = obj.gameObject.AddComponent<ProceduralObject>();

        objects.Add(obj);
    }

    private void ObjSimulate()
    {
        foreach (Transform obj in objects)
        {
            if (obj == null)
                continue;

            obj.position -= new Vector3(currentSpeed * Time.deltaTime, 0, 0);

            if (Between(obj.position.x, despawnPoint.position.x - 5f, despawnPoint.position.x + 5f))
            {
                StartCoroutine(obj.GetComponent<ProceduralObject>().OnRemove());
                objects.Remove(obj);
                break;
            }
        }
    }

    private void HitPunishment(float hitPunishAmount)
    {
        if (currentSpeed-(hitPunishAmount*Time.deltaTime) >= startSpeed)
            currentSpeed -= hitPunishAmount*Time.deltaTime;
    }

    private void SpeedSet()
    {
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += Time.deltaTime * speedIncrement / 10;
            currentMinTime = 30 / currentSpeed;
            currentMaxTime = 200 / currentSpeed;
        }
    }

    private bool Between(float modelValue, float lowerValue, float upperValue)
    {
        return modelValue > lowerValue && modelValue < upperValue;
    }
}
