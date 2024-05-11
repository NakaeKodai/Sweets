using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    private bool opening = false;
    // private List<int> BackpackList = new List<int>();
    public IngredientsDB ingredientsDB;
    public GameObject BackpackObjekt;
    public BackpackItemIcon backpackItemIcon;
    private PlayerInputAction playerInputAction;
    public MenuManager menuManager;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(opening){
            if(playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered){
                BackpackObjekt.SetActive(false);
                opening = false;
                menuManager.selectMenuNow = false;
            }
        }
    }

    public void OpenBackpack(){
        if(!opening){
            opening = true;
            List<int> BackpackList = new List<int>();
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < ingredientsDB.ingredientsList.Count; i++){
                if(ingredientsDB.ingredientsList[i].quantity != 0){
                    BackpackList.Add(ingredientsDB.ingredientsList[i].ID);
                    Debug.Log(ingredientsDB.ingredientsList[BackpackList[j]].name);
                    j++;
                }
            }
            backpackItemIcon.ItemIconSetting(BackpackList);
            BackpackObjekt.SetActive(true);
        }
    }
}
