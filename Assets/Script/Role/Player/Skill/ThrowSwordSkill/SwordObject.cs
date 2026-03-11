using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class SwordObject : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    //技能信息
    private Skill_Info _skillInfo;

    public enum SwordState
    {
        //一开始的状态
        Normal,
        //以下三个不能共存
        //剑可以穿透怪物
        Penetrate,
        //剑可以在怪物之间弹射
        Catapult,
        //剑会停留在一个地方造成持续伤害
        Stop
    }

    //该对象所在的对象池
    public static string poolName = "SwordPool";
    //玩家
    private Player _player;
    //是否进行了碰撞
    private bool _isCrash;
    //是否判断销毁是否应为被捡起
    public bool _isBePickUp;
    //存在时，防止剑被扔出去的时候马上被玩家捡起
    private float _time;

    private Vector2 _direction;
    private float _force;

    //飞剑的状态
    private SwordState _state;
    //飞剑是否正在回归
    private bool _isBacking;
    //是否退出场景时的销毁
    private bool isApplicationQuitting = false;
    //是否进行自我销毁
    private bool _isPassTimeDestroy = false;

    //是否停止并造成持续伤害
    private bool _isStop = false;
    //停留时间
    private float _stopTime = 0;
    //伤害时间间隔
    private float _stopIntervalHit = 0f;
    //停留时储存敌人的列表
    private List<GameObject> stopEnemies = new List<GameObject>();

    //弹射次数
    private int _catapultNum;
    //弹射时储存敌人的数组
    private Collider2D[] enemies;
    //是否开始弹射
    private bool _isCatapult;
    //弹射的目标
    private int _catapultTarget;

    private Player player;


    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    public void InitSword(Vector2 direction, float force, SwordState State)
    {

        _state = State;


        if (_state == SwordState.Penetrate || _state == SwordState.Stop || _state == SwordState.Catapult)
        {
            if (_state == SwordState.Penetrate)
                SwitchState("Idle");
            else if (_state == SwordState.Stop || _state == SwordState.Catapult)
                SwitchState("Rotate");
            _rigidbody2D.velocity = direction * force;
            _rigidbody2D.gravityScale = 0;
            _isPassTimeDestroy = true;
        }
        else
            _rigidbody2D.velocity = new Vector2(direction.x, direction.y * 2) * force;
    }



    void Awake()
    {
        _skillInfo = GetComponent<Skill_Info>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _isCrash = false;
        _time = 0;
        _isBePickUp = false;
        _isBacking = false;
        _isPassTimeDestroy = false;
        _catapultNum = 4;
        _stopTime = 0;
        _stopIntervalHit = 0.35f;
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _state = SwordState.Normal;

    }


    void Update()
    {
        // 存在时间
        _time += Time.deltaTime;

        if (player != null && _time > .5f)
        {
            _isBePickUp = true;
            ReleaseSword();
        }
        //使剑不旋转的时候，剑头指向与运动方向一致
        if (!_isCrash && !_isBacking)
            _rigidbody2D.transform.right = _rigidbody2D.velocity;
        //剑回归运动
        if (_isBacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_player?.transform.position, 20 * Time.deltaTime);
            _rigidbody2D.transform.right = _player.transform.position;
        }


        if ((_state == SwordState.Penetrate && _time > 3) || (_isStop && _stopTime > 2f))
        {
            //DestroySword();
            ReleaseSword();
        }
        else if (_state == SwordState.Stop && _time > 1.5f)
        {
            Stop();
        }

        //飞剑停止时对范围内的怪物造成伤害
        if (_isStop)
        {
            _stopTime += Time.deltaTime;
            if (stopEnemies != null && _stopIntervalHit < _stopTime)
            {
                _stopIntervalHit += 0.35f;
                foreach (GameObject enemy in stopEnemies)
                {
                    BaseEnemy script = enemy.GetComponent<BaseEnemy>();
                    InfoManager.Instance.Damage(_skillInfo, enemy.GetComponent<IEnemyInfo>());
                }
            }

        }
        else if (_isCatapult)
        {
            if (_catapultNum <= 0 || enemies.Length <= 1)
            {
                _isCatapult = false;
                _isBePickUp = true;
                //DestroySword();
                ReleaseSword();
                return;
            }

            if (Vector2.Distance(transform.position, enemies[_catapultTarget].transform.position) < .1f)
            {
                _catapultNum--;
                if (_catapultTarget < enemies.Length - 1)
                    _catapultTarget++;
                else
                    _catapultTarget = 0;
            }
            transform.position = Vector2.MoveTowards(transform.position, enemies[_catapultTarget].transform.position, 10 * Time.deltaTime);

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (collision.gameObject.GetComponent<BaseEnemy>() != null && _state == SwordState.Stop)
            stopEnemies.Add(collision.gameObject);
        //防止刚扔出去就被玩家捡起
        if (collision.GetComponent<Player>() != null && _time < .5f)
        {
            player = collision.GetComponent<Player>();
            return;
        }
        //玩家捡起逻辑
        if (collision.GetComponent<Player>() != null && !_isCatapult)
        {
            _isBePickUp = true;
            //DestroySword()
            ReleaseSword();
            return;
        }

        //当飞剑处于这些状态时才可以造成伤害
        if ((_state == SwordState.Penetrate || _isBacking || !_isCrash || _isCatapult) && collision.gameObject.GetComponent<BaseEnemy>() != null)
            HitEnemy(collision);

        if (_isCrash)
        {
            return;
        }



        switch (_state)
        {
            case SwordState.Normal:
                Normal(collision);
                break;
            //穿透
            case SwordState.Penetrate:
                Penetrate();
                break;
            //停留
            case SwordState.Stop:
                Stop();
                break;
            //弹射
            case SwordState.Catapult:
                Catapult(collision);
                break;
        }



        _isCrash = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stopEnemies.Remove(collision.gameObject);
        if (collision.GetComponent<Player>() != null)
        {
            player = null;
        }
    }

    private void DestroySword()
    {
        if (_isBePickUp || _isPassTimeDestroy)
        {
            ThrowSwordSkill.AddSwordNum(1);
            Debug.Log("玩家捡起飞剑");
        }

        Destroy(gameObject);

    }

    private void ReleaseSword()
    {
        if (isApplicationQuitting || !gameObject.scene.isLoaded)
        {
            return;
        }

        if (_isBePickUp || _isPassTimeDestroy)
        {
            ThrowSwordSkill.AddSwordNum(1);
            Debug.Log("玩家捡起飞剑");
        }

        ObjectPoolManager.instance.ReleaseObject("SwordPool", gameObject);
    }


    private void HitEnemy(Collider2D collision)
    {
        if (collision.GetComponent<BaseEnemy>() != null)
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (_state != SwordState.Stop)
            {
                InfoManager.Instance.Damage(_skillInfo, enemy.GetComponent<IEnemyInfo>());
            }


        }
    }

    //销毁时运行
    private void OnDestroy()
    {
        if (isApplicationQuitting || !gameObject.scene.isLoaded)
        {
            return;
        }

        if (transform.parent != null && !_isBePickUp)
        {
            SwordObject sword = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<SwordObject>();
            sword.transform.localScale = new Vector3(8, 8, 8);
            sword.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    //变为活跃时调用
    private void OnEnable()
    {
        //_skillInfo = GetComponent<Skill_Info>();
        //_rigidbody2D = GetComponent<Rigidbody2D>();
        //_animator = GetComponentInChildren<Animator>();
        //_isCrash = false;
        //_time = 0;
        //_isBePickUp = false;
        //_isBacking = false;
        //_isPassTimeDestroy = false;
        //_catapultNum = 4;
        //_stopTime = 0;
        //_stopIntervalHit = 0.35f;
        //_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    //变为禁止时调用
    private void OnDisable()
    {
        if (isApplicationQuitting || !gameObject.scene.isLoaded)
        {
            return;
        }

        if (transform.parent.GetComponent<BaseEnemy>() != null && !_isBePickUp && ObjectPoolManager.instance?.GetPool("SwordPool") != null)
        {
            SwordObject sword = ObjectPoolManager.instance.GetObject("SwordPool").GetComponent<SwordObject>();
            sword.transform.position = transform.position;
            sword.transform.localScale = new Vector3(8, 8, 8);
            sword.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        _skillInfo.SetDamageTimeType(DamageTimeType.Hit);
        _isCrash = false;
        _time = 0;
        _isBePickUp = false;
        _isBacking = false;
        _isPassTimeDestroy = false;
        _catapultNum = 4;
        _stopTime = 0;
        _isStop = false;
        _stopIntervalHit = 0f;
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void SwitchState(string statename)
    {
        if (statename == "Idle")
        {
            _animator.SetBool("Rotate", false);
            _animator.SetBool("Idle", true);
        }
        else if (statename == "Rotate")
        {
            _animator.SetBool("Rotate", true);
            _animator.SetBool("Idle", false);
        }

    }


    #region 飞剑的行为逻辑
    //剑处于一般状态时运行
    private void Normal(Collider2D collision)
    {
        if (collision.transform.GetComponent<BaseEnemy>() != null || collision.transform.gameObject.tag == "Platform")
            transform.parent = collision.transform;
        _animator.SetBool("Idle", true);
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log("飞刀与：" + collision.name + "相撞");
        _rigidbody2D.velocity = Vector2.zero;
    }

    //剑被回收
    public void BackPlayer(Player player)
    {
        _isBacking = true;
        _player = player;
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    //剑的穿透能力
    public void Penetrate()
    {

    }

    //剑的停留能力
    public void Stop()
    {
        _skillInfo.SetDamageTimeType(DamageTimeType.IntervalHit);
        if (!_isStop)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _isStop = true;
            _stopTime = 0;

        }
    }

    //剑的弹射能力
    public void Catapult(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BaseEnemy>() == null)
        {
            _isBePickUp = true;
            //DestroySword();
            ReleaseSword();
        }
        enemies = Physics2D.OverlapCircleAll(collision.transform.position, 10, LayerMask.GetMask("Enemy"));
        _isCatapult = true;
        _catapultTarget = 1;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }
    #endregion
}
