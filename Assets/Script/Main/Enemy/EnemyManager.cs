using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("ステータス関連")]
    [SerializeField] private EnemyStatus enemyStatus;
    [SerializeField] private PlayerStatus playerStatus;
    public int id;
    private string name;
    private int hp;
    private int attack;
    private int defense;
    private float hostileDistance; //敵対距離
    private float lostDistance; //非敵対距離
    public float attackDistance;

    public float distance;
    private bool isBattle = false;
    private bool canAction = true;
    UnityEngine.AI.NavMeshAgent agent;

    public enum AttackType
    {
        shortType,
        longType
    }
    public AttackType attackType;

    public GameObject attackRange;

    private Coroutine hideCoroutine;

    public Transform player;

    [Header("percentは合計が100以下で設定してにゃ")]
    [Header("percentは小数点第一位まで")]
    public List<HarvestItem> HarvestList = new List<HarvestItem>();//採取できるアイテムのリスト
    public int maxAmount = 2;//最大採取量、これをプレイヤーの変数からとってスキルによる採取量変化とかやりたい
    [SerializeField] public IngredientsDB ingredientsDB;//データベースの取得
    public int harvestCount;//採取する回数
    public Backpack backpackScript;

    void Start()
    {
        this.name = enemyStatus.enemyList[id].name;
        this.hp = enemyStatus.enemyList[id].hp;
        this.attack = enemyStatus.enemyList[id].attack;
        this.defense = enemyStatus.enemyList[id].defense;
        this.hostileDistance = enemyStatus.enemyList[id].hostileDistance;
        this.lostDistance = enemyStatus.enemyList[id].lostDistance;
        this.attackDistance = enemyStatus.enemyList[id].attackDistance;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
        else if(canAction)
        {
            distance = Vector3.Distance(player.position, gameObject.transform.position);
            if (!isBattle)
            {
                if (distance <= hostileDistance)
                {
                    isBattle = true;
                    Debug.Log("見つけた");
                }
            }
            else
            {
                if (lostDistance <= distance)
                {
                    isBattle = false;
                    Debug.Log("どこじゃあ");
                    if(distance <= attackDistance) 
                    {
                        Attack();
                        canAction = false;
                    }
                }
                // if(navMeshAgent.pathStatus != navMeshPathStatus.PathInvalid)
                // {
                agent.SetDestination(player.position);
                // }
            }
        }
    }

    public void DropItem()
    {
        // 取得できるアイテムの乱数設定
        int selectItemId = HarvestList[0].Id;
        int[] itemPercent = new int[1000];
        int listNumber = 0;
        int listPercentNum = 0;
        // itemPercentにListのpercent*10の分の要素にIDを入れることで乱数をやりやすくしている
        for (int l = 0; l < 1000; l++)
        {
            if (listNumber < HarvestList.Count)
            {
                itemPercent[l] = HarvestList[listNumber].Id;
                if (l == (int)(HarvestList[listNumber].percent * 10) + listPercentNum - 1)
                {
                    listPercentNum += (int)(HarvestList[listNumber].percent * 10);
                    listNumber++;
                }
            }
            else itemPercent[l] = -1;
        }

        // アイテム採取の乱数決定
        for (int i = 0; i < harvestCount; i++)
        {
            int rand = Random.Range(1, 101);//1から100を乱数で指定
            // Debug.Log(rand);
            int amount;//採取量の指定
            // 採取量を乱数で変更
            if (rand >= 80)
            {
                amount = maxAmount;
            }
            else if (rand >= 50)
            {
                amount = maxAmount - 1;
                if (amount <= 0) amount = 1;
            }
            else if (rand >= 20)
            {
                amount = maxAmount - 2;
                if (amount <= 0) amount = 1;
            }
            else
            {
                amount = maxAmount - 3;
                if (amount <= 0) amount = 1;
            }

            // 乱数によって取得できるアイテムを調整
            do
            {
                int randItem = Random.Range(0, 1000);
                selectItemId = itemPercent[randItem];
            } while (selectItemId == -1);

            // 所持していないアイテムを取得したときに持ち物欄に加える
            if (ingredientsDB.ingredientsList[selectItemId].quantity == 0)
            {
                backpackScript.AddDataList(ingredientsDB.ingredientsList[selectItemId].ID, ingredientsDB.ingredientsList[selectItemId].name, amount);
            }

            ingredientsDB.ingredientsList[selectItemId].quantity += amount;//採取した個数分をアイテムの個数に追加

            if (!ingredientsDB.ingredientsList[selectItemId].got) ingredientsDB.ingredientsList[selectItemId].got = true;

            Debug.Log(ingredientsDB.ingredientsList[selectItemId].name + "を" + amount + "個手に入れた");

            int difference;//カンストしたときの入手できなかった分
            if (ingredientsDB.ingredientsList[selectItemId].quantity > 999)
            {
                difference = ingredientsDB.ingredientsList[selectItemId].quantity - 999;
                Debug.Log(ingredientsDB.ingredientsList[selectItemId].name + "を持ちすぎていたため、" + difference + "個入手できなかった。");
                ingredientsDB.ingredientsList[selectItemId].quantity = 999;
            }

        }
    }

    private void Attack()
    {
        Debug.Log(" 俺の攻撃!");
        if(attackType == AttackType.shortType)
        {
            Debug.Log("敵の近接攻撃");
            attackRange.SetActive(true);
            if(hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(HideattackRange(1.0f));
        }
        else if(attackType == AttackType.longType)
        {
            Debug.Log("敵の遠距離攻撃");
            if(hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(HideattackRange(3.0f));
        }
    }

    private IEnumerator HideattackRange(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        canAction = true;
        attackRange.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            this.hp -= (playerStatus.attack / 2) - (this.defense / 4);
            Debug.Log(this.hp);
        }
    }
}
