using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalMovement : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] float cooldownTime;
    float cooldownTimer;
    public void ToggleMovement(bool enabled)
    {
        agent.enabled = enabled;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cooldownTimer = Random.Range(0, cooldownTime);
    }

    private void Update()
    {
        Wander();
    }

    void Wander()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;

            // Offset the random direction by the current position of the animal
            randomDirection += transform.position;

            NavMeshHit hit;
            // Sample the nearest valid position on the navmesh
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);

            // Get the final target position
            Vector3 targetPos = hit.position;

            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime;
        }
    }
}
