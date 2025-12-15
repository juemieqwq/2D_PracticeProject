using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove;
    protected new string name;
    protected CanvasGroup canvasGroup;

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            Time.timeScale = 0;
        }
        else
            Debug.LogError("canvasGroup为空" + this.name + "面板打开错误");
        isRemove = false;
    }

    public virtual void ClosePanel()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

        }
        else
            Debug.LogError("canvasGroup为空" + this.name + "面板关闭错误");
        if (UIManager.Instance.runningPanels.ContainsKey(name))
        {
            UIManager.Instance.runningPanels.Remove(name);
        }
        if (!UIManager.Instance.cachePanels.ContainsKey(name))
        {
            UIManager.Instance.cachePanels.Add(name, this);
        }
        //else
        //{
        //    if (GetComponentInParent<Canvas>() != null)
        //        Destroy(gameObject.transform.parent.gameObject);
        //    else
        //        Destroy(gameObject);
        //    Debug.LogError("错误销毁");
        //}
        isRemove = true;
    }
}
