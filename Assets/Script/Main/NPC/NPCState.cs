using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState : MonoBehaviour
{
    public string npcName;
    public byte state;
    public List<string> list;
    [SerializeField]
    public List<Topic> topicList = new List<Topic>();

    [System.Serializable]
    public class Topic
    {
        public List<string> topic = new List<string>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
