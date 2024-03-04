using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private byte hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Damage(byte attackPoint)
    {
        hp -= attackPoint;
        Debug.Log(hp);
    }
}
