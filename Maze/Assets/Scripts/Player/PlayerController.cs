using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action OnExitTriggered;
    
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Rigidbody characterRigidbody;
    [SerializeField] private int movementSpeed = 5;
    [SerializeField] private string exitNodeTag = "ExitNode";

    private Vector3 moveDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(exitNodeTag))
        {
            OnExitTriggered?.Invoke();
        }
    }
    
    private void FixedUpdate()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterTransform.Translate(Time.deltaTime * moveDirection * movementSpeed);
    }

    private void LateUpdate()
    {
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
    }
}