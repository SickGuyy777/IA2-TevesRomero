using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public Agent[] agents;
    public float XLine;
    public float ZLine;
    public Text UITiempoRestante;
    public float _Time=60f;
    private void Start()
    {
        var yellowAg = agents.Where(x => x is YellowAgent)
                        .Select(x => x.agentName + " is Yellow"); //IA2-LINQ

        IEnumerable<T> Agents<T>(IEnumerable<T> col)
        {
            foreach (var item in col)
            {
                yield return item;
            }
        }

        int _spot = Random.Range(0, agents.Length);
        agents[_spot].ImSpot=true;
    }
    private void Update()
    {
        _Time -= Time.deltaTime;
        UITiempoRestante.text = "" + _Time.ToString("f1");
        if(_Time<=0)
        {
            _Time=0;
        }

    }
    public void ManageTimer()
    {
        _Time -= Time.deltaTime;
        UITiempoRestante.text = "" + _Time.ToString("f1");
        if (_Time <= 0)//aca estaba pensando en parar todos y mostrar quienes fueron los primeros 3 ganadores a lo mejor con una lista de puntajes para usar linq
        {
            _Time = 0;
        }
    }
    public Vector3 TransportPosition(Vector3 Actualposition)
    {
        if (Actualposition.z > ZLine / 2) Actualposition.z = -ZLine / 2;
        if (Actualposition.z < -ZLine / 2) Actualposition.z = ZLine / 2;
        if (Actualposition.x < -XLine / 2) Actualposition.x = XLine / 2;
        if (Actualposition.x > XLine / 2) Actualposition.x = -XLine / 2;
        return Actualposition;
    }


    private void OnDrawGizmos()
    {
        Vector3 SupLeftRight = new Vector3(-XLine / 2, 0, ZLine / 2);
        Vector3 SupRightLeft = new Vector3(XLine / 2, 0, ZLine / 2);
        Vector3 InfLeftRight = new Vector3(-XLine / 2, 0, -ZLine / 2);
        Vector3 InfRightLeft = new Vector3(XLine / 2, 0, -ZLine / 2);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(SupLeftRight, SupRightLeft);
        Gizmos.DrawLine(SupRightLeft, InfRightLeft);
        Gizmos.DrawLine(InfRightLeft, InfLeftRight);
        Gizmos.DrawLine(InfLeftRight, SupLeftRight);
    }
}
