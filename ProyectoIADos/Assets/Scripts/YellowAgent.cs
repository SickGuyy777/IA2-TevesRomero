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
        //currentSpot = GameObject.FindGameObjectWithTag("Spot");
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
                //MyForce(Evade());
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

    public Vector3 Evade()
    {
        Vector3 evade = Vector3.zero;
        if (gameObject.tag == "YellowTeam")
        {
            GameObject[] spots = GameObject.FindGameObjectsWithTag("Spot");
            GameObject closestSpot = null;
            float closestDistance = float.MaxValue;
    
            foreach (GameObject spot in spots)
            {
                float distance = Vector3.Distance(transform.position, spot.transform.position);
                if (distance <= viewRange && distance < closestDistance)
                {
                    closestSpot = spot;
                    closestDistance = distance;
                }
            }
    
            if (closestSpot != null)
            {
                Vector3 dir = closestSpot.transform.position - transform.position;
                evade = new Vector3(-dir.y, -dir.x, 0).normalized;
    
                MyForce(evade * MaxSpeed * Time.deltaTime);
            }
        }
        return evade;
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
        YellowAgent otroAgente = collision.gameObject.GetComponent<YellowAgent>();

        if (otroAgente != null)
        {
            if (ImSpot && !otroAgente.ImSpot)
            {
                CambiarPapeles(otroAgente);
            }
        }
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeSpot);
        YellowAgent objetoMasCercano = EncontrarObjetoMasCercano();
        if (objetoMasCercano != null && ImSpot==true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, objetoMasCercano.transform.position);
            Gizmos.DrawSphere(objetoMasCercano.transform.position, 0.1f);
        }
    }
}
