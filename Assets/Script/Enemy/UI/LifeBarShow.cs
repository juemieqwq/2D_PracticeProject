using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LifeBarShow : MonoBehaviour
{
    [SerializeField]
    private Image health;
    [SerializeField]
    private Image transition;
    private Coroutine TransitionCoroutine;
    [SerializeField]
    private TextMeshProUGUI text;

    private EnemyInfo enemyInfo;
    private PlayerInfo playerInfo;

    private float MaxHealth;
    private float TransitionHealth;
    private float CurrentHealth;




    void Start()
    {
        Assert.IsNotNull(health, "health为空");
        Assert.IsNotNull(transition, "transition为空");
        Assert.IsNotNull(text, "test为空");
        Init();


    }

    private void Update()
    {
        SetLifeBarShow();
        if (TransitionHealth <= 0)
            gameObject.SetActive(false);

    }


    private void Init()
    {
        enemyInfo = transform.parent.parent.GetComponentInChildren<EnemyInfo>();
        playerInfo = transform.parent.parent.GetComponentInChildren<PlayerInfo>();
        if (enemyInfo != null)
        {
            MaxHealth = enemyInfo.GetInfo(GetInfoType.MaxHealth);
            text.text = (enemyInfo.GetInfo(GetInfoType.Health) + "/" + enemyInfo.GetInfo(GetInfoType.MaxHealth));
        }
        else if (playerInfo != null)
        {
            MaxHealth = playerInfo.GetInfo(GetInfoType.MaxHealth);
            text.text = (playerInfo.GetInfo(GetInfoType.Health) + "/" + playerInfo.GetInfo(GetInfoType.MaxHealth));
        }
        CurrentHealth = MaxHealth;
        TransitionHealth = MaxHealth;
        if (playerInfo == null && enemyInfo == null)
            Debug.LogError("角色信息为空");

    }

    public void SetLifeBarShow()
    {
        if (enemyInfo != null && CurrentHealth != enemyInfo.GetInfo(GetInfoType.Health))
        {
            CurrentHealth = enemyInfo.GetInfo(GetInfoType.Health);
            text.text = (CurrentHealth + "/" + MaxHealth);
            health.fillAmount = (CurrentHealth / MaxHealth);

        }
        else if (playerInfo != null && CurrentHealth != playerInfo.GetInfo(GetInfoType.Health))
        {
            CurrentHealth = playerInfo.GetInfo(GetInfoType.Health);

            text.text = (CurrentHealth + "/" + MaxHealth);
            health.fillAmount = CurrentHealth / MaxHealth;
        }
        if (TransitionCoroutine != null)
        {
            StopCoroutine(TransitionCoroutine);
            TransitionCoroutine = null;
        }

        TransitionCoroutine = StartCoroutine(LifeBarTransient());

    }

    private IEnumerator LifeBarTransient()
    {
        //平滑时间
        float smoothTime = .4f;
        while (TransitionHealth > CurrentHealth)
        {

            TransitionHealth = Mathf.Lerp(TransitionHealth, CurrentHealth, Time.deltaTime / smoothTime);
            if (Mathf.Abs(TransitionHealth - CurrentHealth) < 0.1f)
                TransitionHealth = CurrentHealth;
            transition.fillAmount = TransitionHealth / MaxHealth;
            yield return null;
        }
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
