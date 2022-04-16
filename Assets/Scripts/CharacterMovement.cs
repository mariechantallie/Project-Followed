using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerCharacter))]
public class CharacterMovement : MonoBehaviour {
    const int BrakeRate = 3;

    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [SerializeField] KeyCode strafeLeftKey = KeyCode.A;
    [SerializeField] KeyCode backwardKey = KeyCode.S;
    [SerializeField] KeyCode strafeRightKey = KeyCode.D;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float moveForce = 500f;
    [SerializeField] float maxMoveSpeed = 50f;
    [SerializeField] float lookRotationSpeed = 250f;
    [SerializeField] Transform characterModel;
    [SerializeField] float sprintForce = 1000f;
    [SerializeField] float maxSprintSpeed = 100f;
    [SerializeField] float magnitude;

    [field: SerializeField] public bool IsSprinting { get; private set; } = false;
    [field: SerializeField] public StateMachine MovementState { get; private set; } = new StateMachine();

    public IMonoState Walking { get; private set; } = new WalkingState("Walking");
    public IMonoState Sneaking { get; private set; } = new SneakingState("Sneaking");
    public IMonoState Idle { get; private set; } = new IdleState("Idle");
    public IMonoState SprintingState { get; private set; } = new SprintingState("Sprinting");

    Vector3 lookDirection;
    Rigidbody cachedRigidbody;
    Transform cachedTransform;
    PlayerCharacter player;

    void Start() {
        player = GetComponent<PlayerCharacter>();
        cachedRigidbody = GetComponent<Rigidbody>();
        cachedTransform = transform;
        MovementState.AddStates(Walking, Sneaking, Idle, SprintingState);
    }

    void Update() {
        lookDirection = Vector3.zero;
        IsSprinting = Input.GetKey(sprintKey);

        if(Input.GetKey(forwardKey)) { lookDirection += Forward; }
        if(Input.GetKey(strafeLeftKey)) { lookDirection += Left; }
        if(Input.GetKey(backwardKey)) { lookDirection += Back; }
        if(Input.GetKey(strafeRightKey)) { lookDirection += Right; }
        if(lookDirection != Vector3.zero) {
            Move(lookDirection, IsSprinting);
        }
        if(lookDirection == Vector3.zero
            && cachedRigidbody.velocity.magnitude > 0f
            && player.CharacterState.CurrentState == player.GroundedState) {
            Brake();
        }
    }

    void Brake() {
        cachedRigidbody.AddForce(-cachedRigidbody.velocity / BrakeRate);
    }

    Vector3 Forward => cachedTransform.forward;
    Vector3 Left => -cachedTransform.right;
    Vector3 Back => -cachedTransform.forward;
    Vector3 Right => cachedTransform.right;
    Quaternion NeededRotation(Vector3 dir) => Quaternion.LookRotation(dir, Vector3.up);
    float LookRotationStep => Time.deltaTime * lookRotationSpeed;
    Quaternion ModelRotation => characterModel.rotation;

    void Move(Vector3 dir, bool isSprinting = false) {
        var speed = isSprinting ? sprintForce : moveForce;
        dir = dir.normalized;
        characterModel.rotation = Quaternion.RotateTowards(ModelRotation, NeededRotation(dir), LookRotationStep);
        magnitude = cachedRigidbody.velocity.magnitude;

        if(isSprinting && cachedRigidbody.velocity.magnitude >= maxSprintSpeed) { return; }
        if(!isSprinting && cachedRigidbody.velocity.magnitude >= maxMoveSpeed) { return; }
        cachedRigidbody.AddForce(speed * dir * Time.deltaTime);
    }
}
