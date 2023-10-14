using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class YellowAgent : Agent
{
    private Renderer rend;
    private Vector3 _MySpeed;
    private void Start()
    {
        Vector3 AleDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        AleDir.Normalize();
        AleDir *= MaxSpeed;
        MyForce(AleDir);
        rend = GetComponent<Renderer>();
        if (ImSpot && rend != null && rend.material != null)
        {
            rend.material.color = Color.blue;
        }
    }
    private void Update()
    {
        transform.position += _MySpeed * Time.deltaTime;
        transform.forward = _MySpeed;
        LimitsFronts();
        CheckTeam();
        if(_manager._Time>0)
        {
            if (ImSpot == true)
            {
                YellowAgent objetoCercano = EncontrarObjetoMasCercano();
                rend.material.color = Color.blue;
                gameObject.tag = "Spot";
            }
            else
            {
                rend.material.color = Color.yellow;
                TimeNotSpot();
                gameObject.tag = "YellowTeam";
                MyForce(-Evade());
            }
        }
        else if(_manager._Time<=0)
        {
            MaxSpeed = 0;
        }

    }
    public void MyForce(Vector3 force)
    {
        _MySpeed += force;
        if (_MySpeed.magnitude >= MaxSpeed)
        {
            _MySpeed = _MySpeed.normalized * MaxSpeed;
        }
    }
    void CheckTeam()
    {
        foreach (var item in _manager.agents)
        {
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= viewRange && dist.magnitude >= 1 && item.tag == "YellowTeam" && !_teamAgents.Contains(item))
                _teamAgents.Add(item);
        }
    }

    YellowAgent EncontrarObjetoMasCercano()//linq calculo la posicion del NPC mas cercano 
    {
        YellowAgent objetoMasCercano = FindObjectsOfType<YellowAgent>()
            .Where(objeto => objeto != this)
            .OrderBy(objeto => Vector3.Distance(transform.position, objeto.transform.position))
            .FirstOrDefault();

        return objetoMasCercano;
    }
    public Vector3 SteeringCalculate(Vector3 Desired)
    {
        return Vector3.ClampMagnitude(Desired.normalized * MaxSpeed - _MySpeed, MaxForceRot);
    }
    public Vector3 Evade()
    {
        Vector3 Desired = Vector3.zero;
        foreach (var Hunter in _manager.agents)
        {
            Vector3 DistAllHunters = Hunter.transform.position - transform.position;
            if (Hunter == this)
            {
                continue;
            }
            if (DistAllHunters.magnitude <= viewRange && gameObject.CompareTag("Spot"))
            {
                Vector3 ProxPos = Hunter.transform.position + transform.position * Time.deltaTime;
                Desired = ProxPos - transform.position;
            }
        }
        return SteeringCalculate(Desired);
    }
    public void TimeNotSpot()
    {
        _Time += Time.deltaTime;
        UITiempoRestante.text = "" + _Time.ToString("f1");
    }
    //no usa linq la region de abajo
    #region cambio de papeles 
    private void CambiarPapeles(YellowAgent otroAgente)
    {
        ImSpot = false;
        otroAgente.ImSpot = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        YellowAgent otroAgente = collision.collider.GetComponent<YellowAgent>();

        if (otroAgente != null)
        {
            if (ImSpot && !otroAgente.ImSpot)
            {
                CambiarPapeles(otroAgente);
            }
        }
    }
    #endregion   
    public void LimitsFronts()
    {
        transform.position = _manager.TransportPosition(transform.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        YellowAgent objetoMasCercano = EncontrarObjetoMasCercano();
        if (objetoMasCercano != null && ImSpot==true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, objetoMasCercano.transform.position);
            Gizmos.DrawSphere(objetoMasCercano.transform.position, 0.1f);
        }

    }
}
