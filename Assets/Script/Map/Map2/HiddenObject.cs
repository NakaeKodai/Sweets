using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    // publuc MiniMapManager miniMapManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        // MiniMapManager.RecordDestroyedObject
    }
}
