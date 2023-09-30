using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Agent : MonoBehaviour
{
    public Text UITiempoRestante;
    public float _Time = 60f;
    public float MaxSpeed;
    public string agentName;
    public float viewRange;
    public float MaxForceRot;
    public bool ImSpot;
    [SerializeField] protected Manager _manager;
    [SerializeField] protected List<Agent> _teamAgents = new List<Agent>();
}
