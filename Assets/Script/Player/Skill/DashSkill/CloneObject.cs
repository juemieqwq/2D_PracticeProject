using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloneObject : MonoBehaviour
{
    public static string poolName = "CloneAttackPool";
    private CloneAttackSkill cloneAttackSkill;
    public static bool isCanAttack;
    private bool isAttacking;

    private PlayerAnimator playerAnimator;

    private Material cloneMaterial;
    //完成动画后的时间
    private float finishTime;
    //当前时间大于此时间时运行
    private float runTime;
    //存在时间
    private float waitTime;


    void Start()
    {
        isCanAttack = true;
        cloneAttackSkill = new CloneAttackSkill();
        cloneAttackSkill.Init(this);
        cloneMaterial = GetComponentInChildren<SpriteRenderer>().material;
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.time;
        if (!isAttacking && isCanAttack)
        {
            isAttacking = true;
            cloneAttackSkill.Attack();
        }

        //如果克隆存在超过5秒或者克隆体的动画播放完毕
        if (playerAnimator.isFinish || waitTime > 5)
        {
            finishTime += Time.deltaTime;

        }
        //使克隆体逐渐淡化
        if (finishTime > runTime + .5f && runTime < 1)
        {
            runTime += .1f;
            cloneMaterial.color = new Color(1, 1, 1, 1 - runTime);

        }
        //当克隆体完全淡化时释放
        else if (runTime > 1)
        {
            ObjectPoolManager.instance.ReleaseObject(poolName, gameObject);
        }


    }

    private void OnEnable()
    {
        finishTime = 0;
        runTime = 0;
        waitTime = 0;
        isAttacking = false;
        if (cloneMaterial != null)
            cloneMaterial.color = new Color(1, 1, 1, 1);

    }

    private void OnDisable()
    {

    }
}
