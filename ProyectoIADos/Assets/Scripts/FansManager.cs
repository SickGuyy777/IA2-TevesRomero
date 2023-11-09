using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FansManager : MonoBehaviour
{
    [SerializeField] Manager _manager;

    Renderer _rend;
    float _num;

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
    }
}
