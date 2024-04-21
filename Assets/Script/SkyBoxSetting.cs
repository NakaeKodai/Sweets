using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSetting : MonoBehaviour
{

    [SerializeField] Material skybox;
    void Start()
    {
        if (RenderSettings.skybox != skybox) {
            RenderSettings.skybox = skybox;
        }
    }
}
