using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MianUI : MonoBehaviour
{
    private Vector2 relativePosition;
    //物体刚体
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = transform.parent.GetComponentInChildren<Rigidbody2D>();
        relativePosition = transform.position;
        Assert.IsNotNull(rigidbody2D, "主UI获取不到属主位置");
    }


    void Update()
    {
        if ((Vector2)transform.position != rigidbody2D.position)
            transform.position = rigidbody2D.position;

    }
}
