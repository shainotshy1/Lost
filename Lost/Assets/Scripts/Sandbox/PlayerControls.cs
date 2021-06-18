using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction jump;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float slowDownMovementSpeed;
    [SerializeField] float slowDownRotationSpeed;

    Rigidbody rigidBody;

    bool isGrounded = false;
    float zMovement = 0f;
    float rotation = 0f;
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    private void OnEnable()
    {
        movement.Enable();
        jump.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
    }
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        float xValue = movement.ReadValue<Vector2>().x;
        float zValue = movement.ReadValue<Vector2>().y;
        float jumpValue = (jump.ReadValue<float>()>0.5&&isGrounded)?1:0;

        if (Mathf.Abs(xValue) >0.5)
        {
            xValue /= Mathf.Abs(xValue);
            rotation = xValue * Time.deltaTime * rotationSpeed;
        }
        else if (Mathf.Abs(rotation) > Mathf.Epsilon) { }
        {
            bool isPositive = rotation > 0;
            rotation -= rotation * Time.deltaTime * slowDownRotationSpeed / Mathf.Abs(rotation);
            if (rotation>0!=isPositive)
            {
                rotation = 0f;
            }
        }

        if (Mathf.Abs(zValue) >0.5)
        {
            zValue /= Mathf.Abs(zValue);
            zMovement = zValue * Time.deltaTime * movementSpeed;
        }
        else if (Mathf.Abs(zMovement) > Mathf.Epsilon)
        {
            zMovement -= zMovement * Time.deltaTime * slowDownMovementSpeed/ Mathf.Abs(zMovement);
            zMovement = Mathf.Clamp(zMovement, 0, Mathf.Infinity);
        }

        float yForce = jumpValue * Time.deltaTime*jumpForce;

        Vector3 force = new Vector3(0,yForce,0);

        if(isGrounded) rigidBody.AddRelativeForce(force);

        if(Mathf.Abs(zMovement)>Mathf.Epsilon) transform.Translate(0, 0, zMovement);
        
        if(Mathf.Abs(rotation) > Mathf.Epsilon) transform.Rotate(0, rotation, 0);
    }
}
