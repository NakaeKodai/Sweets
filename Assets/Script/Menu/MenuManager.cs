using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private PlayerStatus playerstatus;

    public bool isMenu;//メニューウィンドウが開いているか
    private PlayerInputAction playerInputAction;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject menuList;
    [SerializeField] private GameObject money;
    [SerializeField] private GameObject popularity;
    private int selectBottuonNum;//選択中のボタンのリスト番号
    private int beforeCursorNum = -1;
    private GameObject selectBottuon;//今選択中のボタン
    private GameObject buttonImage;
    public bool selectMenuNow;//メニュー項目が選択されているかを判別
    private bool isLongPushUp;
    private bool isLongPushDown;
    private float pushDuration = 0.3f;
    private float downTime = 0f;

    public Color nomalColor;
    public Color selectColor;

    public Backpack backpackScript;
    public BackpackManager backpackManager;
    public Recipe recipeScript;
    public RecipeManager recipeMAnager;
    public WishListManager wishListManager;
    public LibraryManager libraryManager;
    public GameObject setting;

    public Talk talkScript;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.pause && playerInputAction.UI.OpenMenu.triggered && !isMenu && !talkScript.isTalking)
        {
            menuWindow.SetActive(true);
            money.SetActive(true);
            popularity.SetActive(true);
            setStatus();
            Time.timeScale = 0;
            gameManager.pause = true;
            isMenu = true;
            // selectBottuonNum = 0;
            Debug.Log("ザ・ワールド！");
        }
        else if (((gameManager.pause && playerInputAction.UI.OpenMenu.triggered) || (gameManager.pause && playerInputAction.UI.Cancel.triggered && !selectMenuNow)) && isMenu)
        {
            menuWindow.SetActive(false);
            money.SetActive(false);
            popularity.SetActive(false);
            Time.timeScale = 1;
            gameManager.pause = false;
            isMenu = false;
            selectMenuNow = false;
            Debug.Log("そして時は動き出す");
        }


        if (isMenu && !selectMenuNow)
        {
            if (beforeCursorNum != selectBottuonNum)
            {
                selectBottuon = menuWindow.transform.GetChild(selectBottuonNum).gameObject;
                buttonImage = selectBottuon.transform.GetChild(1).gameObject;
                beforeCursorNum = selectBottuonNum;
                buttonImage.SetActive(true);
            }



            //上ボタン
            if (playerInputAction.UI.CursorMoveUp.triggered)
            {
                downTime = Time.realtimeSinceStartup;
                isLongPushUp = true;
                buttonImage.SetActive(false);
                selectBottuonNum--;
                if (selectBottuonNum < 0)
                {
                    selectBottuonNum = menuWindow.transform.childCount - 1;
                }
            }
            if (isLongPushUp)
            {
                if (Time.realtimeSinceStartup - downTime >= pushDuration)
                {
                    buttonImage.SetActive(false);
                    selectBottuonNum--;
                    if (selectBottuonNum < 0)
                    {
                        selectBottuonNum = menuWindow.transform.childCount - 1;
                    }
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveUp.canceled += ctx =>
            {
                isLongPushUp = false;
                pushDuration = 0.3f;
            };

            //下ボタン
            if (playerInputAction.UI.CursorMoveDown.triggered)
            {
                downTime = Time.realtimeSinceStartup;
                isLongPushDown = true;
                buttonImage.SetActive(false);
                selectBottuonNum++;
                if (selectBottuonNum >= menuWindow.transform.childCount)
                {
                    selectBottuonNum = 0;
                }
            }
            if (isLongPushDown)
            {
                if (Time.realtimeSinceStartup - downTime >= pushDuration)
                {
                    buttonImage.SetActive(false);
                    selectBottuonNum++;
                    if (selectBottuonNum >= menuWindow.transform.childCount)
                    {
                        selectBottuonNum = 0;
                    }
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveDown.canceled += ctx =>
            {
                isLongPushDown = false;
                pushDuration = 0.3f;
            };


            // アイテムを開く項目
            if (playerInputAction.UI.MenuSelect.triggered)
            {
                switch (selectBottuonNum)
                {
                    case 0:
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        money.SetActive(false);
                        popularity.SetActive(false);
                        Debug.Log("アイテム開く");
                        backpackManager.OpenBackpack();
                        // backpackScript.OpenBackpack();
                        break;
                    case 1:
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        money.SetActive(false);
                        popularity.SetActive(false);
                        recipeMAnager.OpenRecipe(true);
                        // recipeScript.OpenRecipe();
                        break;
                    case 2:
                        Debug.Log("ウィッシュリスト");
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        money.SetActive(false);
                        popularity.SetActive(false);
                        wishListManager.OpenWishList();
                        break;
                    case 3:
                        Debug.Log("図鑑");
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        money.SetActive(false);
                        popularity.SetActive(false);
                        libraryManager.OpenLibrary();
                        break;
                    case 4:
                        Debug.Log("依頼一覧");
                        break;
                    case 5:
                        Debug.Log("設定");
                        selectMenuNow = true;
                        setting.SetActive(true);
                        money.SetActive(false);
                        popularity.SetActive(false);
                        menuWindow.SetActive(false);
                        break;
                    case 6:
                        Debug.Log("セーブ");
                        gameManager.Save();
                        break;
                }
            }
            // if (selectBottuonNum == 0 && playerInputAction.UI.MenuSelect.triggered)
            // {
            //     selectMenuNow = true;
            //     menuWindow.SetActive(false);
            //     Debug.Log("アイテム開く");
            //     backpackScript.OpenBackpack();
            // }
            // else if (selectBottuonNum == 1 && playerInputAction.UI.MenuSelect.triggered)
            // {
            //     selectMenuNow = true;
            //     menuWindow.SetActive(false);
            //     recipeScript.OpenRecipe();
            // }
        }
        else if (isMenu && selectMenuNow)
        {//メニュー項目を開いてるときになんか使えそうだから現在放置
            if (playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered)
            {
                selectMenuNow = false;
                menuWindow.SetActive(true);
                money.SetActive(true);
                popularity.SetActive(true);
            }
        }
    }

    public void setStatus(){
        GameObject moneyT = money.transform.GetChild(0).gameObject;
        TextMeshProUGUI moneyText = moneyT.GetComponent<TextMeshProUGUI>();
        moneyText.text = playerstatus.money + "G";
        GameObject popularityT = popularity.transform.GetChild(0).gameObject;
        TextMeshProUGUI popularityText = popularityT.GetComponent<TextMeshProUGUI>();
        popularityText.text = ""+playerstatus.popularity;
    } 
}
