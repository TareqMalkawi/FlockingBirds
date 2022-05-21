using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] birdsPrefab;
    [SerializeField] private int birdsCount;
    [SerializeField] public Vector3 flyLimits;
    [SerializeField] private Vector3 entireFlockTravelSpeed;

    [Header("Settings")]
    [Range(0.0f, 10.0f)]
    public float minSpeed;
    [Range(0.0f, 10.0f)]
    public float maxSpeed;
    [Range(0.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float avoidanceDistance;
    [Range(0.0f, 10.0f)]
    public float rotationSpeed;

    [HideInInspector] public GameObject[] birds;
    [HideInInspector] public Vector3 goalPos;

    private Vector3 birdPos;

    private void Start()
    {
        birds = new GameObject[birdsCount];

        for (var i = 0; i < birdsCount; i++)
        {
            birdPos = transform.position + new Vector3(Random.Range(-flyLimits.x, flyLimits.x),
                Random.Range(0.4f, 1.8f), Random.Range(-flyLimits.z, flyLimits.z));

            birds[i] = Instantiate(birdsPrefab[Random.Range(0, 3)], birdPos, quaternion.identity);
        }

        goalPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(0.0f,entireFlockTravelSpeed.y * Time.deltaTime, entireFlockTravelSpeed.z * Time.deltaTime);

        if (Random.Range(0, 100) < 15)
            goalPos = transform.position +  new Vector3(Random.Range(-flyLimits.x, flyLimits.x),
                Random.Range(-flyLimits.y, flyLimits.y), Random.Range(-flyLimits.z, flyLimits.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, 0.15f);
        Gizmos.DrawCube(transform.position, flyLimits * 2);
    }
}
