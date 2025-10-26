using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(CheckpointSystem))]
/// <summary>
/// Deals with inputs for player characters
/// </summary>
public class Player2Controller : MonoBehaviour {

    public float softRespawnDelay = 0.5f;
    public float softRespawnDuration = 0.5f;
    public InputMaster controls;
    public float attackCooldown = 1.0f;
    public SpriteRenderer _renderer;

    bool attacking = false;
    bool attackOnCooldown = false;

    // Other components
    private P2Attack attackManager;
    private CharacterController2D character;
    private CameraController cameraController;
    private CheckpointSystem checkpoint;
    private InteractSystem interact;
    private Vector2 axis;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        character = GetComponent<CharacterController2D>();
        checkpoint = GetComponent<CheckpointSystem>();
        interact = GetComponent<InteractSystem>();
        attackManager = GetComponentInChildren<P2Attack>();
        cameraController = FindFirstObjectByType<CameraController>();
        if (!cameraController) {
            Debug.LogError("The scene is missing a camera controller! The player script needs it to work properly!");
        }
        controls = new InputMaster();
        controls.Player2.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player2.Movement.canceled += ctx => Move(Vector2.zero);
        controls.Player2.Jump.started += Jump;
        controls.Player2.Jump.canceled += EndJump;
        controls.Player2.Dash.started += Dash;
        controls.Player2.Interact.started += Interact;
        controls.Player2.AttackA.started += Attack;
        controls.Player2.AttackA.canceled += AttackCancelled;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate() {
        character.Walk(axis.x);
        character.ClimbLadder(axis.y);
    }

    private void Move(Vector2 _axis) {
        axis = _axis;
    }

    private void Jump(InputAction.CallbackContext context) {
        if (axis.y < 0) {
            character.JumpDown();
        } else {
            character.Jump();
        }
    }

    private void EndJump(InputAction.CallbackContext context) {
        character.EndJump();
    }

    private void Dash(InputAction.CallbackContext context) {
        character.Dash(axis);
    }

    private void Interact(InputAction.CallbackContext context) {
        if (interact) {
            interact.Interact();
        }
    }

    
    private void Attack(InputAction.CallbackContext context) {
        if (interact && interact.PickedUpObject)
        {
            interact.Throw();
        }
        else
        {
            attacking = true;
            if (!character.Immobile)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }
    void AttackCancelled(InputAction.CallbackContext context)
    {
        attacking = false;
    }
    IEnumerator AttackRoutine()
    {
        while (attacking)
        {
            if (!attackOnCooldown)
            {
                SwordAttack();
            }
            yield return null;
        }
    }
    void SwordAttack()
    {

        //GameManager.Instance.ParticleSpawner.SpawnSlash(transform.position + Vector3.up, axis);
        attackManager.StartAttack(axis, character.FacingRight);
        attackOnCooldown = true;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    /// <summary>
    /// Respawns the player at the last soft checkpoint while keeping their current stats
    /// </summary>
    public void SoftRespawn() {
        _renderer.flipY = true;
        character.Immobile = true;
        Invoke("StartSoftRespawn", softRespawnDelay);
    }

    /// <summary>
    /// Starts the soft respwan after a delay and fades out the screen
    /// </summary>
    private void StartSoftRespawn() {
        cameraController.FadeOut();
        Invoke("EndSoftRespawn", softRespawnDuration);
    }

    /// <summary>
    /// Ends the soft respwan after the duration ended, repositions the player and fades in the screen
    /// </summary>
    private void EndSoftRespawn() {
        _renderer.flipY = false;
        checkpoint.ReturnToSoftCheckpoint();
        cameraController.FadeIn();
        character.Immobile = false;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable() {
        controls.Player2.Enable();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable() {
        controls.Player2.Disable();
    }
}