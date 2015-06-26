using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using UnityStandardAssets.Characters.FirstPerson;

public class ParkourController : MonoBehaviour
{
    public float jumpForce = 20;
    public float movementVelocity = 2.5f;

    [SerializeField] private MouseLook mouseLook = new MouseLook();

    [SerializeField] private Dictionary<KeyCode, InputAction> keyMap = new Dictionary<KeyCode, InputAction>();
    [SerializeField] private Dictionary<InputAction, UnityAction> actionMap = new Dictionary<InputAction, UnityAction>();

    private bool canJump = false;
    private Rigidbody ownerRigidBody;

    void Start()
    {
        ownerRigidBody = GetComponent<Rigidbody>();
        MapKeys();
        MapActions();
        mouseLook.Init(this.transform, Camera.main.transform);
    }

    void FixedUpdate()
    {
        HandleInput();
        mouseLook.LookRotation(this.transform, Camera.main.transform);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Platform")
        {
            canJump = true;
        }
    }

    public void AddNewKeyMapping(KeyCode key, InputAction action)
    {
        keyMap.Add(key, action);
    }

    public void AddNewActionMapping(InputAction action, UnityAction executeOnAction)
    {
        actionMap.Add(action, executeOnAction);
    }

    private void HandleInput()
    {
        foreach (KeyCode key in keyMap.Keys)
        {
            if (Input.GetKey(key))
            {
                actionMap[keyMap[key]].Invoke();
            }
        }
    }

    private void MapActions()
    {
        UnityAction jumpAction = new UnityAction(() =>
        {
            if (canJump)
            {
                ownerRigidBody.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
                canJump = false;
            }
        });
        UnityAction moveForwardAction = new UnityAction(() =>
        {
            ownerRigidBody.AddForce(this.transform.forward * movementVelocity, ForceMode.Acceleration);
        });
        UnityAction moveBackAction = new UnityAction(() =>
        {
            ownerRigidBody.AddForce(-this.transform.forward * movementVelocity, ForceMode.Acceleration);
        });
        UnityAction moveLeftAction = new UnityAction(() =>
        {
            ownerRigidBody.AddForce(-this.transform.right * movementVelocity, ForceMode.Acceleration);
        });
        UnityAction moveRightAction = new UnityAction(() =>
        {
            ownerRigidBody.AddForce(this.transform.right * movementVelocity, ForceMode.Acceleration);
        });

        actionMap.Add(InputAction.Jump, jumpAction);
        actionMap.Add(InputAction.MoveBack, moveBackAction);
        actionMap.Add(InputAction.MoveForward, moveForwardAction);
        actionMap.Add(InputAction.MoveLeft, moveLeftAction);
        actionMap.Add(InputAction.MoveRight, moveRightAction);
    }

    private void MapKeys()
    {
        keyMap.Add(KeyCode.W, InputAction.MoveForward);
        keyMap.Add(KeyCode.A, InputAction.MoveLeft);
        keyMap.Add(KeyCode.S, InputAction.MoveBack);
        keyMap.Add(KeyCode.D, InputAction.MoveRight);
        keyMap.Add(KeyCode.Space, InputAction.Jump);
    }
}

public enum InputAction
{
    MoveForward,
    MoveLeft,
    MoveRight,
    MoveBack,
    Jump
}