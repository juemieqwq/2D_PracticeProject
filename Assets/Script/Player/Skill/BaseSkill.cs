using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseSkill : MonoBehaviour
{

    [SerializeField]
    protected float _coolDown;
    protected float _coolDownTime;

    protected PlayerManager _playerManager;
    protected Player _player;

    protected Dictionary<string, BaseSkill> dirSkill = new Dictionary<string, BaseSkill>();

    public GameObject currentObject = null;

    public TransferObject objectClass = null;

    public BaseSkill(float coolDown, PlayerManager playerManager, Player player)
    {
        _coolDown = coolDown;
        _playerManager = playerManager;
        _player = player;
    }



    public virtual void SkillUpdate()
    {
        _coolDownTime -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (_coolDownTime < 0)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Init   _coolDownTime
    /// </summary>
    public virtual void UseSkill()
    {

        this._coolDownTime = _coolDown;
    }




    //public BaseSkill GetSkillClass<T>(string skillName) where T : BaseSkill, new()
    //{
    //    BaseSkill skill = null;
    //    if (dirSkill.TryGetValue(skillName, out skill))
    //    {
    //        return skill;
    //    }
    //    skill = new T();
    //    dirSkill.Add(skillName, skill);
    //    return skill;
    //}
}
