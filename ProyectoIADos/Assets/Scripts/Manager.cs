using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{
    public Agent[] agents;

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
    }
}
