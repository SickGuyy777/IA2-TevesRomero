using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Agent[] agents;
    [SerializeField] Text[] _agentsNames;
    [SerializeField] Text _cantFans;
    public float XLine;
    public float ZLine;
    public Text UITiempoRestante;
    public float _Time = 60f;
    public Transform finalRange;
    public GameObject plantilla;
    public List<FansManager> yellowFans = new List<FansManager>();
    public List<FansManager> blueFans = new List<FansManager>();
    bool _showTime = false;
    int _textIndex;

    private void Start()
    {
        var allAgents = agents.Select(x => x.agentName); //IA2-LINQ
        var noSpotAgent = agents.Where(x => x.ImSpot = false); //IA2-LINQ
        var allFans = yellowFans.Concat(blueFans); //IA2-LINQ

        _cantFans.text = "Attendance: " + allFans.Count();

        foreach (var item in noSpotAgent)
            item.rend.material.color = Color.yellow;

        foreach (var item in allAgents)
        {
            _agentsNames[_textIndex].text = item;
            if(_textIndex < _agentsNames.Length) _textIndex++;
        }

        int _spot =Random.Range(0, agents.Length);
        agents[_spot].ImSpot=true;




    }

    private void Update()
    {
        ManageTimer();
        if(_Time<=0)
        {
            _Time = 0;
            finalRange.gameObject.SetActive(true);
            if(!_showTime)
            {
                registrotabla();
                ShareData();
                _showTime = true;
            }
        }
    }
    public void ManageTimer()
    {
        _Time -= Time.deltaTime;
        UITiempoRestante.text = "" + _Time.ToString("f1");
    }

    public void registrotabla()
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject ins = Instantiate(plantilla, finalRange);
            ins.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -150f);
            ins.name = i.ToString();
        }
    }
    [ContextMenu("pasar los datos a la pabla")]
    public void ShareData()
    {
        Agent jugadorConPuntajeMasAlto = EncontrarPuntajeMasAlto(agents);

        for (int i = 0; i < 1; i++)
        {
            finalRange.GetChild(i).GetChild(0).GetComponent<Text>().text = agents[i].name;
            finalRange.GetChild(i).GetChild(1).GetComponent<Text>().text = agents[i]._Time.ToString();
        }
    }

    static Agent EncontrarPuntajeMasAlto(Agent[] jugadores)
    {
        return jugadores.OrderByDescending(j => j._Time).First(); //IA2-LINQ
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
