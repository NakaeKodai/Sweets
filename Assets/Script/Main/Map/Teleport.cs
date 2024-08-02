using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private Animator animator;//アニメーションの変数
    public string targetAnimationName = "FadeOutAnimation"; // アニメーションの名前を設定
    private int targetAnimationHash;
    private bool isAnimation;
    private bool isFadeinAnimation;

    public float x,y,z;
    // Start is called before the first frame update
    void Start()
    {
        targetAnimationHash = Animator.StringToHash(targetAnimationName);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimation)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(isFadeinAnimation)
            {
                if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
                {
                    animator.SetTrigger("FadeOut");
                    player.position = new Vector3(x,y,z);
                    isFadeinAnimation = false;
                    // fadeImage.SetActive(false);
                }
            }
            if(stateInfo.shortNameHash == targetAnimationHash)
            {
                // Debug.Log("ふぇーどおうｔ");
                if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
                {
                    isAnimation = false;
                    fadeImage.SetActive(false);
                }
            }
        }

    }

    private void OnCollisionEnter(Collision player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            fadeImage.SetActive(true);
            animator.SetTrigger("FadeIn");
            isFadeinAnimation = true;
            isAnimation = true;
            // player.gameObject.transform.position = new Vector3(x,y,z);
        }
    }
}
