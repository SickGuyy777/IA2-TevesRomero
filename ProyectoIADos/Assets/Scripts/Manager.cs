using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Agent[] agents;
    [SerializeField] Text[] _agentsNames;
    [SerializeField] Text _cantFans, _howIsTheDay;
    public float XLine;
    public float ZLine;
    public Text UITiempoRestante;
    public float _Time = 60f;
    public Transform finalRange;
    public GameObject plantilla;
    public List<FansManager> yellowFans = new List<FansManager>();
    public List<FansManager> blueFans = new List<FansManager>();
    [SerializeField] List<GameObject> _borders = new List<GameObject>();

    (string[] timeOfTheDay, string[] weather) weatherInGame; //IA2-LINQ

    bool _showTime = false;
    int _textIndex;

    private void Start()
    {
        weatherInGame.timeOfTheDay = new string[] { "Morning", "Afternoon", "Evening", "Night" };
        weatherInGame.weather = new string[] { "Sunny", "Cloudy", "Rainy", "Snowy" };

        var border = _borders.SelectMany(border => border.GetComponentsInChildren<Renderer>()).ToList();//IA2-LINQ
        var day = weatherInGame.timeOfTheDay.Zip(weatherInGame.weather, (t, w) => t + ", " + w).ToList();//IA2-LINQ
        var allAgents = agents.OfType<YellowAgent>().Select(x => x.agentName); //IA2-LINQ
        var noSpotAgent = agents.Where(x => x.ImSpot = false); //IA2-LINQ
        var allFans = yellowFans.Concat(blueFans); //IA2-LINQ

        foreach (var item in border)
        {
            item.material.color = Color.red;
        }

        if (day.Count > 0)
        {
            int randomIndex = Random.Range(0, day.Count);
            _howIsTheDay.text = day[randomIndex];
        }
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
                var recompensasParaGanador = agents.GenerarRecompensaGanador();
                foreach (var recompensa in recompensasParaGanador)
                {
                    Debug.Log($"{recompensa.Descripcion}");
                }

                DesactivarFansEnTodosLosAgentes(agents);
                DeterminarGanador();
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
    private void DesactivarFansEnTodosLosAgentes(Agent[] agentes)
    {
        foreach (var agente in agentes)
        {
            if (agente is YellowAgent yellowAgent)
            {

                yellowAgent.DesactivarTodosLosFans();
            }
        }
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
            finalRange.GetChild(i).GetChild(0).GetComponent<Text>().text = jugadorConPuntajeMasAlto.name;
            finalRange.GetChild(i).GetChild(1).GetComponent<Text>().text = jugadorConPuntajeMasAlto._Time.ToString();
        }
    }

    static Agent EncontrarPuntajeMasAlto(Agent[] jugadores)
    {
        return jugadores.OrderByDescending(j => j._Time).First(); //IA2-LINQ
    }


    void DesactivarOtrosJugadores(Agent ganador)
    {
        foreach (var agente in agents.Except(new[] { ganador }))
        {
            agente.gameObject.SetActive(false); // Desactivar al jugador
        }
    }

    void MostrarGanadorEnEscena(Agent ganador)
    {
        ganador.gameObject.SetActive(true); // Activar al jugador ganador
    }

    void DeterminarGanador()//IA2-LINQ 
    {
        var ganador = agents
            .OrderByDescending(a => a._Time)
            .Take(1)
            .FirstOrDefault();

        if (ganador != null)
        {
            DesactivarOtrosJugadores(ganador);
            MostrarGanadorEnEscena(ganador);
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