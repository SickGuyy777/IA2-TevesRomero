using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class Agent : MonoBehaviour
{
    public Text UITiempoRestante;
    [HideInInspector] public Renderer rend;
    public float _Time = 60f;
    public float MaxSpeed;
    public string agentName;
    public float viewRange;
    public float rangeSpot;
    public float MaxForceRot;
    public bool ImSpot;
    public Vector3 _MySpeed;
    [SerializeField] protected Manager _manager;
    [SerializeField] protected List<Agent> _teamAgents = new List<Agent>();
    public List<FansManager> ListaDeFans;
    public Transform yo;
    public Collider colision;
}
