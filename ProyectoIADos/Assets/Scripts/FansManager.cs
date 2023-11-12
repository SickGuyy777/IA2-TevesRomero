using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FansManager : MonoBehaviour
{
    [SerializeField] Manager _manager;
    Renderer _rend;
    List<int> NumRifa = new List<int>();
    float _num;
    [SerializeField] GameObject lentes;
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _num = Random.Range(0, 3);

        switch (_num)
        {
            case 0:
                _rend.material.color = Color.gray;
                break;
            case 1: 
                _manager.yellowFans.Add(this);
                _rend.material.color = Color.yellow;
                break;
            case 2:
                _manager.blueFans.Add(this);
                _rend.material.color = Color.blue;
                break;
        }
        for (int i = 0; i < 2; i++)
        {
            int numeroAleatorio = Random.Range(1, 11); // Genera un número aleatorio entre 1 y 10
            NumRifa.Add(numeroAleatorio);
        }
        bool algunNumeroEsIgualA10 = NumRifa.Any(num => num == 10);// IA2-LINQ
        if (algunNumeroEsIgualA10)
        {
            lentes.SetActive(true);
        }
    }
}
