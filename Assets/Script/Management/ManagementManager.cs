using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagementManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    public GameManager gameManager;

    public int Customer;
    public int managementFlug = 0;
    public TextMeshProUGUI testText;


    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        testText.text = "残りの客の数："+Customer;
        if(managementFlug == 0){
            if(playerInputAction.UI.MenuSelect.triggered){
                // testText.text = "残りの客の数："+Customer;
                Customer--;
            }
            if(Customer == 0){
                managementFlug = 1;
            }
        }
        else if(managementFlug == 1){
            testText.text = "客数０、終わり";
        }
        
    }
}
