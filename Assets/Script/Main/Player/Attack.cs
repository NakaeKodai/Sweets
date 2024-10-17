using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    public Vector2 playerDirection; //プレイヤーの向き
    public bool isEnter;
    public byte attackPoint = 2;

    public GameObject attackRange;
    public float distance = 1.0f;
    public float attackTime = 1.0f;

    private Coroutine hideCoroutine;
    Enemy enemy;
    
    public GameManager gameManager;
    
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();
        if(playerDirection.magnitude < 0.5f) playerDirection = Vector2.zero;
        if(playerDirection != Vector2.zero)
        {
            Vector3 direction = new Vector3(playerDirection.x, 0, playerDirection.y).normalized;
            Vector3 attackRangePosition = gameObject.transform.position + direction * distance;
            attackRange.transform.position = attackRangePosition;
        }
        if(playerInputAction.Player.Fire.triggered && !gameManager.pause)
        {
            Debug.Log("こうげき！");
            attackRange.SetActive(true);
            if(hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(HideattackRange());
        }
    }

    private IEnumerator HideattackRange()
    {
        yield return new WaitForSeconds(attackTime);
        attackRange.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("E_Attack"))
        {
            playerStatus.hp -= (playerStatus.attack / 2) - (playerStatus.defense / 4); //仮
            Debug.Log(playerStatus.hp);
        }
    }
}
