using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 只控制角色移动
/// NPC和Player可共用
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3 currentInput;
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position+currentInput*maxMoveSpeed*Time.fixedDeltaTime);
    }

    public void setMovementInput(Vector3 input)
    {
        Vector3.ClampMagnitude(input, 1);
        currentInput=input;
    }
}
