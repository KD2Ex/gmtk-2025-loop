using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform circleCenter;
    
    private Rigidbody2D rb;
    private float radius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        radius = (transform.position - circleCenter.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        var dir = (target.position - circleCenter.position).normalized;
        transform.position = circleCenter.position + dir * radius;
        return;
        
        var dist = (transform.position - circleCenter.position).magnitude;
        var targetDistance = radius - dist;

        var targetPos = dir * targetDistance;
        transform.position = target.position + targetPos;
        return;
        
        var targetCircle = (target.position - circleCenter.position).normalized;
        var circleLine = (transform.position - circleCenter.position).normalized;
        var angle = Vector2.Angle(targetCircle, circleLine);
        print("Angle: " + angle);
        var x = transform.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        var y = transform.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        
        transform.position = new Vector3(x, y, 0.0f);
    }
}
