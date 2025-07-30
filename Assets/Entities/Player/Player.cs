using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    private Rigidbody2D rb;
    private PlayerInput input;

    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");


        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }

    private void OnDisable()
    {
        var moveAction = input.currentActionMap.FindAction("Move");
        
        
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        Move();
        //print(rb.velocity);
    }


    public void Move()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}