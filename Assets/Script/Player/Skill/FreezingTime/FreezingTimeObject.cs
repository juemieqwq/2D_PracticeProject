using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezingTimeObject : MonoBehaviour
{

    //存在时间
    private float time;
    //存放怪物列表
    public List<BaseEnemy> enemies = new List<BaseEnemy>();
    //目标大小
    [SerializeField]
    private float targetScale;
    //变化数速度
    [SerializeField]
    private float scaleSpeed;
    //原本大小
    [SerializeField]
    private Vector3 originScale;
    //动画
    public Animator animator;
    //动画全部完成
    public bool isFinish;
    //是否进行缩放动画
    public bool isShrink;
    //是否进行爆炸动画
    public bool isDetonate;
    //状态
    private FreezingTimeSkill.FreezingTimeState state;
    //是否进行克隆攻击动画
    public bool isCloneAttack;
    //执行一次
    private bool once;
    private PlayerManager playerManager;
    //所在对象池名称
    public static string poolName = "FreezingTimeObjectPool";



    void Awake()
    {

        animator = GetComponentInChildren<Animator>();
        playerManager = PlayerManager.instance;
        state = FreezingTimeSkill.FreezingTimeState.Detonate;
    }

    private void OnEnable()
    {
        enemies.Clear();
        once = false;
        transform.localScale = new Vector3(1, 1, 1);
        targetScale = 8;
        scaleSpeed = 2;
        isFinish = false;
        isShrink = false;
        isDetonate = false;
        isCloneAttack = false;
        time = 0;
        originScale = transform.localScale;
    }

    private void OnDisable()
    {
        if (SceneStateManager.IsApplicationQuitting)
            return;
        foreach (var enemy in enemies)
        {
            enemy?.SetIsFreezingTime(false);
        }
    }



    void Update()
    {
        if (isFinish)
            ObjectPoolManager.instance.ReleaseObject(poolName, gameObject);
        //Destroy(gameObject);

        switch (state)
        {
            case FreezingTimeSkill.FreezingTimeState.Detonate:
                DetonateMode();
                break;
            case FreezingTimeSkill.FreezingTimeState.CloneAttack:
                CloneAttackMode();
                break;

        }

        time += Time.deltaTime;
    }


    private void CloneAttackMode()
    {
        //逐渐增加冻结范围
        if (transform.localScale.x < targetScale)
        {
            transform.localScale += new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
        }

        //如果范围能有敌人，才可以开启后续攻击
        if (!FreezingTimeSkill.isCanCloneAttack && enemies != null)
        {
            FreezingTimeSkill.isCanCloneAttack = true;
        }

        if (time <= 5 && isCloneAttack && !once)
        {
            animator.SetBool("CloneAttack", true);
            if (ObjectPoolManager.instance.GetPool(CloneObject.poolName) == null)
            {
                ObjectPoolManager.instance.CreateNewPool(CloneObject.poolName, playerManager.clonePrefab);
            }

            foreach (BaseEnemy enemy in enemies)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject gameObject;
                    gameObject = ObjectPoolManager.instance.GetObject(CloneObject.poolName);
                    float pos = Random.Range(1, 1.7f);
                    pos = pos >= -Random.Range(-1.7f, -1) ? pos : Random.Range(-1.7f, -1);
                    gameObject.transform.position = enemy.transform.position + new Vector3(pos * -enemy._direction, 0);
                    gameObject.transform.rotation = enemy.transform.rotation;
                    if (pos < 0)
                    {
                        gameObject.transform.Rotate(0, 180, 0);
                    }
                    gameObject.SetActive(true);
                }

            }
            once = true;
            time = 0;
        }
        else if (time > 5 && !isCloneAttack)
        {
            ObjectPoolManager.instance.ReleaseObject(poolName, gameObject);
            //Destroy(gameObject);
        }

        if (once && time > 3)
        {
            ObjectPoolManager.instance.ReleaseObject(poolName, gameObject);
            //Destroy(gameObject);
        }
    }

    private void DetonateMode()
    {

        if (time >= 5 && !isShrink && !isDetonate)
        {
            animator.SetBool("Shrink", true);
            isShrink = true;
        }
        else if (!isShrink && !isDetonate)
        {
            if (transform.localScale.x < targetScale)
            {
                transform.localScale += new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
            }
        }

        if (isShrink)
        {
            scaleSpeed = 16;
            if (animator.transform.localScale.x > originScale.x)
                animator.transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
        }
        else if (isDetonate)
        {
            scaleSpeed = 16;
            if (animator.transform.localScale.x < targetScale)
                animator.transform.localScale += new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
        }
    }

    public void SetState(FreezingTimeSkill.FreezingTimeState _state)
    {
        state = _state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy enemy;
        if (collision.GetComponent<BaseEnemy>() != null)
        {
            enemy = collision.GetComponent<BaseEnemy>();
            enemies.Add(enemy);
            enemy?.SetIsFreezingTime(true);
        }

    }

    private void OnDestroy()
    {
        //foreach (var enemy in enemies)
        //{
        //    enemy?.SetIsFreezingTime(false);
        //}
    }

}
