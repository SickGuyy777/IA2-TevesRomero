using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class YellowAgent : Agent
{
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
        LimitsFronts();
        CheckTeam();
        if(_manager._Time>0)
        {
            if (ImSpot == true)
            {
                Walk();
                rend.material.color = Color.blue;
                gameObject.tag = "Spot";
                Persuit();
            }
            else
            {
                Walk();
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
    //no usa linq la region Movement
    #region movement 
    public void Walk()
    {
        transform.position += _MySpeed * Time.deltaTime;
        transform.forward = _MySpeed;
    }
    public void MyForce(Vector3 force)
    {
        _MySpeed += force;
        if (_MySpeed.magnitude >= MaxSpeed)
        {
            _MySpeed = _MySpeed.normalized * MaxSpeed;
        }
    }
    public Vector3 SteeringCalculate(Vector3 Desired)
    {
        return Vector3.ClampMagnitude(Desired.normalized * MaxSpeed - _MySpeed, MaxForceRot);
    }
    public void LimitsFronts()
    {
        transform.position = _manager.TransportPosition(transform.position);
    }
    #endregion
    public void DesactivarTodosLosFans()//IA2-LINQ 
    {
        ListaDeFans.SelectMany(fan => fan.GetComponentsInChildren<FansManager>(true))
            .ToList()
            .ForEach(fan => fan.gameObject.SetActive(false));
    }
    void CheckTeam()//IA2-LINQ
    {
        foreach (var item in _manager.agents)
        {
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= viewRange && dist.magnitude >= 1 && item.tag == "YellowTeam" && !_teamAgents.Contains(item))
                _teamAgents.Add(item);
        }
    }

    YellowAgent EncontrarObjetoMasCercano()//IA2-LINQ la posicion del mas cercano
    {
        YellowAgent objetoMasCercano = FindObjectsOfType<YellowAgent>()
            .Where(objeto => objeto != this)
            .OrderBy(objeto => Vector3.Distance(transform.position, objeto.transform.position))
            .FirstOrDefault();
        return objetoMasCercano;
    }

    public Vector3 Evade() //IA2-LINQ aplico el persuit
    {
        var desired = Vector3.zero;
        var nearbySpots = _teamAgents
            .Where(spot => spot != this && (spot.transform.position - transform.position).magnitude <= viewRange && spot.ImSpot)
            .ToList();
        if (nearbySpots.Count > 0)
        {
            var closestSpot = nearbySpots
                .OrderBy(spot => (spot.transform.position - transform.position).sqrMagnitude)
                .First();
            desired = (closestSpot.transform.position + closestSpot._MySpeed * Time.deltaTime) - transform.position;
        }

        return SteeringCalculate(desired);
    }
    public void TimeNotSpot()
    {
        _Time += Time.deltaTime;
        UITiempoRestante.text = "" + _Time.ToString("f1");
    }
    //no usa linq la region de abajo
    #region cambio de papeles 

    public void CambiarPapeles(YellowAgent otroAgente)
    {
        ImSpot = false;
        otroAgente.ImSpot = true;
    }

    #endregion

    Vector3 Persuit()
    {
        Vector3 desired = Vector3.zero;
        YellowAgent objetoMasCercano = EncontrarObjetoMasCercano();//se que puedo poner este metodo dentro del persuit pero de esta forma tambien puedo chequear el gizmos que hice
        if (objetoMasCercano != null)
        {
            Vector3 futurePos = objetoMasCercano.transform.position + _MySpeed * Time.deltaTime;
            desired = futurePos - transform.position;
            transform.position += desired.normalized * _MySpeed.magnitude * MaxSpeed * Time.deltaTime;
            transform.forward = desired.normalized;
        }
        return SteeringCalculate(desired);
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
