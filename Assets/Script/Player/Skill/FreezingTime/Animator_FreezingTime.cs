using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_FreezingTime : MonoBehaviour
{
    FreezingTimeObject object_FreezingTime;


    void Awake()
    {
        object_FreezingTime = GetComponentInParent<FreezingTimeObject>();
    }


    public void IsFinish(int Is)
    {
        if (Is > 0)
            object_FreezingTime.isFinish = true;
        else
            object_FreezingTime.isFinish = false;
    }

    public void HitEnemies()
    {
        foreach (BaseEnemy enemy in object_FreezingTime.enemies)
        {
            enemy.SetIsFreezingTime(false);
            InfoManager.Instance.Damage(gameObject.GetComponentInParent<Skill_Info>(), enemy.gameObject.GetComponent<EnemyInfo>());

        }
    }

    public void EndShrink()
    {

        object_FreezingTime.animator.SetBool("Shrink", false);
        object_FreezingTime.animator.SetBool("Detonate", true);
        object_FreezingTime.isShrink = false;
        object_FreezingTime.isDetonate = true;
    }
}
