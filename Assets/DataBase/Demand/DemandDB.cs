using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DemandDB : ScriptableObject
{
    public List<Demands> demandList = new List<Demands>();
}
