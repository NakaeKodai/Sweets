using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;

    public bool isMenu;//メニューウィンドウが開いているか
    private PlayerInputAction playerInputAction;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private Button[] menuList;//メニューウィンドウのボタンを格納しているリスト
    private int selectBottuonNum;//選択中のボタンのリスト番号
    private int beforeCursorNum = -1;
    private Button selectBottuon;//今選択中のボタン
    private Image buttonImage;
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

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.pause && playerInputAction.UI.OpenMenu.triggered && !isMenu)
        {
            menuWindow.SetActive(true);
            Time.timeScale = 0;
            gameManager.pause = true;
            isMenu = true;
            // selectBottuonNum = 0;
            Debug.Log("ザ・ワールド！");
        }
        else if (((gameManager.pause && playerInputAction.UI.OpenMenu.triggered) || (gameManager.pause && playerInputAction.UI.Cancel.triggered && !selectMenuNow)) && isMenu)
        {
            menuWindow.SetActive(false);
            Time.timeScale = 1;
            gameManager.pause = false;
            isMenu = false;
            // selectBottuon = menuList[selectBottuonNum];
            // Image buttonImage = selectBottuon.GetComponent<Image>();
            // buttonImage.color = nomalColor;
            selectMenuNow = false;
            Debug.Log("そして時は動き出す");
        }


        if (isMenu && !selectMenuNow)
        {
            if (beforeCursorNum != selectBottuonNum)
            {
                selectBottuon = menuList[selectBottuonNum];
                buttonImage = selectBottuon.GetComponent<Image>();
                beforeCursorNum = selectBottuonNum;
                buttonImage.color = selectColor;
            }



            //上ボタン
            if (playerInputAction.UI.CursorMoveUp.triggered)
            {
                downTime = Time.realtimeSinceStartup;
                isLongPushUp = true;
                selectBottuon = menuList[selectBottuonNum];
                buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                selectBottuonNum--;
                if (selectBottuonNum < 0)
                {
                    selectBottuonNum = menuList.Length - 1;
                }
            }
            if (isLongPushUp)
            {
                if (Time.realtimeSinceStartup - downTime >= pushDuration)
                {
                    selectBottuon = menuList[selectBottuonNum];
                    buttonImage = selectBottuon.GetComponent<Image>();
                    buttonImage.color = nomalColor;
                    selectBottuonNum--;
                    if (selectBottuonNum < 0)
                    {
                        selectBottuonNum = menuList.Length - 1;
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
                selectBottuon = menuList[selectBottuonNum];
                buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                selectBottuonNum++;
                if (selectBottuonNum >= menuList.Length)
                {
                    selectBottuonNum = 0;
                }
            }
            if (isLongPushDown)
            {
                if (Time.realtimeSinceStartup - downTime >= pushDuration)
                {
                    selectBottuon = menuList[selectBottuonNum];
                    buttonImage = selectBottuon.GetComponent<Image>();
                    buttonImage.color = nomalColor;
                    selectBottuonNum++;
                    if (selectBottuonNum >= menuList.Length)
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
                        Debug.Log("アイテム開く");
                        backpackManager.OpenBackpack();
                        // backpackScript.OpenBackpack();
                        break;
                    case 1:
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        recipeMAnager.OpenRecipe(true);
                        // recipeScript.OpenRecipe();
                        break;
                    case 2:
                        Debug.Log("ウィッシュリスト");
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        wishListManager.OpenWishList();
                        break;
                    case 3:
                        Debug.Log("図鑑");
                        selectMenuNow = true;
                        menuWindow.SetActive(false);
                        libraryManager.OpenLibrary();
                        break;
                    case 4:
                        Debug.Log("依頼一覧");
                        break;
                    case 5:
                        Debug.Log("設定");
                        selectMenuNow = true;
                        setting.SetActive(true);
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
            }
        }
    }
}
