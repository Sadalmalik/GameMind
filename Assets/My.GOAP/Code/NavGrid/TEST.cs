using System.Collections;
using System.Collections.Generic;
using Sadalmalik.GridNavigation;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public NavGridAgent agent;
    
    public Transform target;
    
    [Sirenix.OdinInspector.Button]
    private void SetDestination()
    {
        agent.SetDestination(target.position);
    }
}
