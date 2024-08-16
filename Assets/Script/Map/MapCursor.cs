using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCursor : MonoBehaviour
{
    public MapManager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // other.gameObject.CompareTag("Checkpoint")
        if (other.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            mapManager.KARI();
            Debug.Log("あんこ");
        }
    }
}
