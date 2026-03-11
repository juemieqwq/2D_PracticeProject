using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class CloneAttackSkill
{
    //攻击类型
    public static int attackNum = 1;
    //自身动画组件
    public Animator animator;
    //属主
    public CloneObject cloneSkill;
    //当前播放动画的名称
    public string attackName;



    public void Init(CloneObject host)
    {
        cloneSkill = host;
        animator = cloneSkill.GetComponentInChildren<Animator>();
        if (animator == null)
            animator = cloneSkill.GetComponentInChildren<Animator>();
    }


    public void Attack()
    {
        Assert.IsNotNull(animator, "克隆体为初始化");
        if (attackNum > 3)
            attackNum = 1;

        switch (attackNum)
        {
            case 3:
                attackName = "Attack3";
                animator.SetBool(attackName, true);
                attackNum++;
                break;
            case 2:
                attackName = "Attack2";
                animator.SetBool(attackName, true);
                attackNum++;
                break;
            case 1:
                attackName = "Attack1";
                animator.SetBool(attackName, true);
                attackNum++;
                break;
        }

    }
}
