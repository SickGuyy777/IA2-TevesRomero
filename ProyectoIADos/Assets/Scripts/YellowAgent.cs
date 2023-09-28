using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowAgent : Agent
{
    private void Update()
    {
        CheckTeam();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}
