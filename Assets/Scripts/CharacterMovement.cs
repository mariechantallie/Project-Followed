using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {
    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [SerializeField] KeyCode strafeLeftKey = KeyCode.A;
    [SerializeField] KeyCode backwardKey = KeyCode.S;
    [SerializeField] KeyCode strafeRightKey = KeyCode.D;
    [SerializeField] float moveSpeed = 500f;
    [SerializeField] float lookRotationSpeed = 250f;
    [SerializeField] Transform characterModel;

    Vector3 lookDirection;
    Rigidbody cachedRigidbody;
    Transform cachedTransform;

    private void Start() {
        cachedRigidbody = GetComponent<Rigidbody>();
        cachedTransform = transform;
    }

    private void Update() {
        lookDirection = Vector3.zero;
        if(Input.GetKey(forwardKey)) { lookDirection += Forward; }
        if(Input.GetKey(strafeLeftKey)) { lookDirection += Left; }
        if(Input.GetKey(backwardKey)) { lookDirection += Back; }
        if(Input.GetKey(strafeRightKey)) { lookDirection += Right; }
        if(lookDirection != Vector3.zero) { Move(lookDirection); }
    }

    private Vector3 Forward => cachedTransform.forward;
    private Vector3 Left => -cachedTransform.right;
    private Vector3 Back => -cachedTransform.forward;
    private Vector3 Right => cachedTransform.right;
    private Quaternion NeededRotation(Vector3 dir) => Quaternion.LookRotation(dir, Vector3.up);
    private float LookRotationStep => Time.deltaTime * lookRotationSpeed;
    private Quaternion ModelRotation => characterModel.rotation;

    private void Move(Vector3 dir) {
        dir = dir.normalized;
        characterModel.rotation = Quaternion.RotateTowards(ModelRotation, NeededRotation(dir), LookRotationStep); ;
        cachedRigidbody.AddForce(moveSpeed * dir * Time.deltaTime);
    }
}
