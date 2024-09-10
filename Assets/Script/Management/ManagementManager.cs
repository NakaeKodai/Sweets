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
        while (Customer > 0){
            if(playerInputAction.UI.MenuSelect.triggered){
                testText.text = "残りの客の数："+Customer;
            }
        }
        testText.text = "客数０、終わり";
    }
}
