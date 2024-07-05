using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] GameObject menuWindow;
    public Light directionalLight;
    [SerializeField]private Image lightImage;
    public List<Scrollbar> scrollbar;
    
    public Color normalColor,selectColor;
    Image scrollbarImage;
    private int selectBarNum,beforeSelectBarNum = -1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.playerInputAction.UI.CursorMoveRight.triggered)
        {
            if(scrollbar[selectBarNum].value <= 1.0f) scrollbar[selectBarNum].value += 0.1f;
        }
        if(gameManager.playerInputAction.UI.CursorMoveLeft.triggered)
        {
            if(scrollbar[selectBarNum].value >= 0.01f) scrollbar[selectBarNum].value -= 0.1f;
        }

        switch(selectBarNum)
        {
            case 0:
                Color color = lightImage.color;
                color.a = scrollbar[selectBarNum].value;
                if(color.a >= 0.96f) color.a = 0.95f;
                lightImage.color = color;
                // directionalLight.intensity = 0.08f + 0.08f * (scrollbar[selectBarNum].value * 10);
                //RenderSettings.ambientIntensity = scrollbar[selectBarNum].value;
            break;
        }

        if(gameManager.playerInputAction.UI.OpenMenu.triggered || gameManager.playerInputAction.UI.Cancel.triggered)
        {
            menuManager.selectMenuNow = false;
            menuWindow.SetActive(true);
            gameObject.SetActive(false);
        }

        SelectControl();
    }

    void SelectControl()
    {
        if(beforeSelectBarNum != selectBarNum)
        {
            scrollbarImage = scrollbar[selectBarNum].GetComponent<Image>();
            scrollbarImage.color = selectColor;
            beforeSelectBarNum = selectBarNum;
        }
        if(gameManager.playerInputAction.UI.CursorMoveUp.triggered)
        {
            scrollbarImage.color = normalColor;
            selectBarNum --;
            if(selectBarNum < 0) selectBarNum = scrollbar.Count - 1;
        }

        if(gameManager.playerInputAction.UI.CursorMoveDown.triggered)
        {
            scrollbarImage.color = normalColor;
            selectBarNum ++;
            if(selectBarNum >= scrollbar.Count) selectBarNum = 0;
        }
    }
}
