using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRope : MonoBehaviour
{
    private Collider2D collider2D;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        lineRenderer = transform.parent.GetComponent<LineRenderer>();
        springJoint = transform.parent.GetComponent<SpringJoint2D>();
        Debug.Log("lineRenderer:" + lineRenderer + "springJoint:" + springJoint);
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.parent.position);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            Destroy(lineRenderer);
            Destroy(springJoint);
            Destroy(gameObject);
            Debug.Log("Éþ×Ó¶Ï¿ª");
        }
    }
}
