using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTopic : MonoBehaviour
{
    // public List<string> talk;
    [SerializeField]
    List<Topic> topicList = new List<Topic>();

    [System.Serializable]
    public class Topic
    {
        public List<string> topic = new List<string>();
    }
}
