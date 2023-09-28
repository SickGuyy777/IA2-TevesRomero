using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public string agentName;
    public float viewRange;

    [SerializeField] protected Manager _manager;
    [SerializeField] protected List<Agent> _teamAgents = new List<Agent>();
}
