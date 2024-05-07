using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsManager : MonoBehaviour
{
    [SerializeField] public IngredientsDB ingredientsDB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P)){
            for(int i = 0; i < ingredientsDB.ingredientsList.Count; i++){
                ingredientsDB.ingredientsList[i].quantity = 0;
            }
            
        }
    }
}
