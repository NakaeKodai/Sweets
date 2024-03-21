using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState : MonoBehaviour
{
    public string name;
    public byte state;
    public List<string> list;
    [SerializeField] public List<List<string>> talk = new List<List<string>>();
    // public List<TalkTopic> talk = new List<TalkTopic>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
