using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackCursor : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private GameObject nowCursor;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    int[,] menuList;
    int nowListNumber = 0;
    public Backpack backpackScript;

    // Start is called before the first frame update
    void Start()
    {
        menuList = new int[4,10];
    }

    // Update is called once per frame
    void Update()
    {
        if(backpackScript.opening){
            nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            
            if(playerInputAction.UI.CursorMoveUp.triggered){

            }
        }
    }

    public void setmenuSelect(){
        for(int i = 0; i < menuList.Length; i++){
            for(int j = 0; j < 10; i++){
                menuList[i,j] = j+10*i;
            }
        }
    }
}
