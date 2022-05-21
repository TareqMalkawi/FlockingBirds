using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockingController : MonoBehaviour
{
    private FlockingManager flockingManager;
    private GameObject[] birds;

    private Vector3 centerPos;
    private Vector3 avoidance;
    private Vector3 direction;

    private float flyingSpeed;
    private float speed;
    private float groupSize;
    private float dist;

    private bool turnBird;

    private RaycastHit raycastHit;

    private void Start()
    {
        raycastHit = new RaycastHit();
        flockingManager = GameObject.FindGameObjectWithTag("Flocking Manager").GetComponent<FlockingManager>();
        if (!flockingManager) return;
        flyingSpeed = Random.Range(flockingManager.minSpeed, flockingManager.maxSpeed);
        birds = flockingManager.birds;
    }

    private void Update()
    {
        var birdsBound = new Bounds(flockingManager.transform.position, flockingManager.flyLimits * 2.0f);

        // to determine if the bird hit something , if it does, should rotate and a void it
        //Physics.Raycast(transform.position, transform.forward * 40.0f, out raycastHit);

        if (!birdsBound.Contains(transform.position))
        {
            turnBird = true;
            direction = flockingManager.transform.position - transform.position;
        }
        //else if (raycastHit.collider != null)
        //{
        //    turnBird = true;
        //    direction = Vector3.Reflect(transform.forward, raycastHit.normal);
        //    // Debug.DrawRay(transform.position, transform.forward * 40.0f, Color.red);
        //}
        else
            turnBird = false;

        if (turnBird)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                quaternion.LookRotationSafe(direction, transform.up),
                flockingManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                FlockingRules();
        }
        transform.Translate(0.0f, 0.0f, flyingSpeed * Time.deltaTime);
    }

    private void FlockingRules()
    {
        foreach (var bird in birds)
        {
            if (bird == gameObject) continue;

            dist = Vector3.Distance(transform.position, bird.transform.position);
            if (!(dist <= flockingManager.neighbourDistance)) continue;

            centerPos += bird.transform.position;
            ++groupSize;

            if (dist <= flockingManager.avoidanceDistance)
                avoidance += (transform.position - bird.transform.position);

            speed += bird.GetComponent<FlockingController>().flyingSpeed;
        }

        if (!(groupSize > 0)) return;
        centerPos = centerPos / groupSize + (flockingManager.goalPos - transform.position);
        flyingSpeed = speed / groupSize;

        direction = (centerPos + avoidance) - transform.position;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                quaternion.LookRotationSafe(direction, transform.up),
                flockingManager.rotationSpeed * Time.deltaTime);
    }
}
