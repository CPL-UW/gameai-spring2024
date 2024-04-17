using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BasicAgent : Agent
{
    public GameObject target;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    Vector3 RandomWorldPoint()
    {
        Vector3 randomPoint = new(Random.Range(0f, 1f), Random.Range(0f, 1f),0);
        return Camera.main.ViewportToWorldPoint(randomPoint);
    }

    public override void OnEpisodeBegin()
    {
        gameObject.transform.position = RandomWorldPoint();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var position = gameObject.transform.position;
        var targetPosition = target.transform.position;
        sensor.AddObservation(position.y);
        sensor.AddObservation(position.x);
        sensor.AddObservation(targetPosition.y);
        sensor.AddObservation(targetPosition.x);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            discreteActionsOut[0] = 0;
        }
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;

        var action = act[0];
        dirToGo = action switch
        {
            0 => Vector3.zero,
            1 => Vector3.up,
            2 => Vector3.left,
            3 => Vector3.down,
            4 => Vector3.right,
            _ => dirToGo
        };
        var nextPoint = transform.position + dirToGo;
        if (dirToGo != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, nextPoint, .2f);
            SetReward(-0.1f);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Debug.Log("OnActionReceived");
        var distanceToTarget = Vector2.Distance(gameObject.transform.position, target.transform.position);

        if (distanceToTarget <= 1f)
        {
            SetReward(1000.0f);
            EndEpisode();
        }
        else if (!_renderer.isVisible)
        {
            EndEpisode();
        }

        MoveAgent(actionBuffers.DiscreteActions);
    }

    private void Update()
    {
        RequestDecision();
    }
}