using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentObjectBeha : MonoBehaviour
{
    private enum behaviourCase
    {
        selfRotation,
        upDown,
        followPlayer
    }

    [SerializeField] behaviourCase behaviour;
    [SerializeField] Vector3 rotationAxis;

    [SerializeField] float UpMax;
    private bool elevatorDown = false;

    [SerializeField] Transform player;
    NavMeshAgent agent;

    void Start()
    {
        if (behaviour == behaviourCase.selfRotation)
        {
            StartCoroutine(SelfRotation());
        }
        if(behaviour == behaviourCase.upDown)
        {
            StartCoroutine(Elevator(transform.position));
        }
        if(behaviour == behaviourCase.followPlayer)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.destination = player.position;
            StartCoroutine(FollowPlayer());
        }
    }

    IEnumerator SelfRotation()
    {
        while (true)
        {
            transform.RotateAround(transform.position, rotationAxis, 20 * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            if(Vector3.Distance(player.transform.position, transform.position) > 5)
            {
                agent.destination = player.position;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Elevator(Vector3 originalPosition)
    {
        while (true)
        {

            if (transform.position.y > originalPosition.y + UpMax)
            {
                elevatorDown = true;
            }
            if (transform.position.y <= originalPosition.y)
            {
                elevatorDown = false;
            }

            if (elevatorDown)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            }

            yield return new WaitForSeconds(0.05f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
