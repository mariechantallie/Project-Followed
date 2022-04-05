using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour {
    private const float FallingVelocityTrigger = -1f;

    [SerializeField] LayerMask jumpableLayers;
    [SerializeField] float jumpForce = 50f;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [field: SerializeField]
    public StateMachine CharacterState { get; private set; } = new StateMachine();
    [field: SerializeField]
    public IMonoState JumpState { get; private set; } = new JumpingState("Jumping");
    [field: SerializeField]
    public IMonoState GroundedState { get; private set; } = new GroundedState("Grounded");
    [field: SerializeField]
    public IMonoState FallingState { get; private set; } = new FallingState("Falling");

    Rigidbody characterRigidbody;

    public bool IsGrounded => (CharacterState.CurrentState is GroundedState);

    private void Start() {
        characterRigidbody = GetComponent<Rigidbody>();
        JumpState.OnEnter += Jump;
        CharacterState.AddState(GroundedState);
        CharacterState.AddState(JumpState);
        CharacterState.AddState(FallingState);
    }

    private void Update() {
        MonitorFalling();
        if(IsGrounded) { CharacterState.ChangeState(GroundedState); }
        if(Input.GetKeyDown(jumpKey) && IsGrounded) {
            CharacterState.ChangeState(JumpState);
        }
    }

    private void MonitorFalling() {
        if(IsFalling) {
            CharacterState.ChangeState(FallingState);
        }
    }

    private bool IsFalling => characterRigidbody.velocity.y < FallingVelocityTrigger;

    private void OnCollisionEnter(Collision collision) {
        if(jumpableLayers.Contains(collision.gameObject.layer)) {
            CharacterState.ChangeState(GroundedState);
        }
    }

    private void Jump() {
        characterRigidbody.AddForce(jumpForce * Vector3.up);
    }
}
