using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXHit : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Material hitMat;
    private Material originalMat;
    private float coutineTime;

    // Start is called before the first frame update
    void Start()
    {
        //获取本身的图片
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;


    }


    //协程要用StartCoroutine(函数);启动不然调用不了- -
    public IEnumerator ChangeFxHit(float ContinueTime = .1f)
    {

        coutineTime = 1;
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.material = originalMat;

    }
}
