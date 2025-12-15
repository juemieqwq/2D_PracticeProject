using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferObject : MonoBehaviour
{
    public static string poolName = "Transfer";

    private Player player;

    public TransferSkill host;

    [SerializeField]
    private float continueTime = 1.5f;
    //当前时间
    private float currentTime;
    //是否转移角色
    public bool isTransferPlayer;
    //是否活跃
    public bool isActive;
    //追踪的对象
    public GameObject target;
    //自己的实现类
    public TransferRealize skillRealize;

    public enum TransferState
    {
        Normal = 1,
        Detonate = 2,
    }

    public HashSet<TransferState> states;


    private void Start()
    {
        skillRealize = RealizeManager.instance.GetRealizeClass<TransferRealize>(TransferRealize.realizeName);
        Debug.Log("skillRealize:" + skillRealize);
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;
        if (currentTime > continueTime && isActive)
        {
            ObjectPoolManager.instance.ReleaseObject(poolName, gameObject);
        }


    }

    public void Init(Player _player)
    {
        player = _player;
    }

    private void OnEnable()
    {
        isActive = true;
        currentTime = 0f;
        isTransferPlayer = false;
    }

    private void OnDisable()
    {

        isActive = false;
        if (isTransferPlayer && player != null)
        {
            player.transform.position = transform.position;
        }
    }






    //public void AddState(TransferState state)
    //{
    //    states.Add(state);

    //}

    //public void RemoveState(TransferState state)
    //{
    //    states.Remove(state);

    //}

}
