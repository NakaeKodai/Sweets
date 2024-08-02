using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動のスクリプト
public class Move : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 

    private Vector2 playerDirection; //プレイヤーの向き
    public byte speed = 5; //移動の速さ
    public byte normalSpeed = 5;
    public byte maxSpeed = 20;

    private bool isWalk;
    public bool isDush;
    private bool canMove = true;

    public float timer = 0.0f;//タイマー

    public float rayDistance = 2;

    Animator animator;//アニメーションの変数
    
    [SerializeField]LayerMask WallLayer;

    public GameManager gameManager;
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!gameManager.pause && canMove)
        {
            // playerInputAction.Player.Dash.performed += ctx => speed = maxSpeed;
            // playerInputAction.Player.Dash.canceled += ctx => speed = normalSpeed;
            playerInputAction.Player.Dash.performed += ctx => {
                speed = maxSpeed;
                isDush = true;
            };
            
            playerInputAction.Player.Dash.canceled += ctx => {
                speed = normalSpeed;
                isDush = false;
            };

            playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();

            if(playerDirection.x != 0 || playerDirection.y != 0)
            {
                isWalk = true;
                animator.SetFloat("InputX",playerDirection.x);
                animator.SetFloat("InputY",playerDirection.y);
            }else
            {
                isWalk = false;
            }

            // 以下Raycastを利用した壁の判定
            // Vector3 playerPosition = transform.position;//プレイヤーの位置を取得
            // Vector3 rayDirection = new Vector3(playerDirection.x, 0, playerDirection.y);//プレイヤーの向きを取得
            // Ray ray = new Ray(playerPosition, rayDirection);//プライヤーの向きにあるものを判別するものを飛ばす
            RaycastHit hitForward;
            if (Physics.Raycast(transform.position, Vector3.forward, out hitForward, rayDistance, WallLayer))
            {
                // Debug.Log("上かべぇ");
                if(playerDirection.y > 0) playerDirection.y = 0;
            }
            RaycastHit hitBack;
            if (Physics.Raycast(transform.position, Vector3.back, out hitBack, rayDistance, WallLayer))
            {
                // Debug.Log("下かべぇ");
                if(playerDirection.y < 0) playerDirection.y = 0;
            }
            RaycastHit hitRight;
            if (Physics.Raycast(transform.position, Vector3.right, out hitRight, rayDistance, WallLayer))
            {
                // Debug.Log("右かべぇ");
                if(playerDirection.x > 0) playerDirection.x = 0;
            }
            RaycastHit hitLeft;
                if (Physics.Raycast(transform.position, Vector3.left, out hitLeft, rayDistance, WallLayer))
            {
                // Debug.Log("左かべぇ");
                if(playerDirection.x < 0) playerDirection.x = 0;
            }
            //if(!Physics.Raycast(ray, out wallHit, 1.0f, WallLayer)){//プレイヤーの方向の少し先にレイヤーの「Wall」がないかを判別する
            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            0.0f,
            playerDirection.y * speed * Time.deltaTime);

            //落下したときの救済
            if(transform.position.y <= -200){
                transform.position = new Vector3(63f,4.6f,15f);
            }
        }else if(!canMove)
        {
            if(timer <= 0.3f)
            {
                // Debug.Log("止まるぜ");
                timer += Time.deltaTime;
            }
            else
            {
                // Debug.Log("動くぜ");
                timer = 0.0f;
                canMove = true;
            }
        }

        animator.SetBool("IsDush",isDush);
        animator.SetBool("IsWalk",isWalk);
    }

    void FixedUpdate(){
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Teleport"))
        {
            // Debug.Log("動かないぜ!");
            isWalk = false;
            canMove = false;
        }
    }

}
