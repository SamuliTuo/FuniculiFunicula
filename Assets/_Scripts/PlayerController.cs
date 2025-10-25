using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(CheckpointSystem))]
/// <summary>
/// Deals with inputs for player characters
/// </summary>
public class PlayerController : MonoBehaviour {

    public float softRespawnDelay = 0.5f;
    public float softRespawnDuration = 0.5f;
    public InputMaster controls;
    public SpriteRenderer renderer;

    public GameObject bullet;
    public float shootInterval = 3.0f;
    private bool shootOnCooldown = false;
    private bool shooting = false;
    private bool dead = false;

    // Other components
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
        cameraController = FindFirstObjectByType<CameraController>();
        if (!cameraController) {
            Debug.LogError("The scene is missing a camera controller! The player script needs it to work properly!");
        }
        controls = new InputMaster();
        controls.Player1.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player1.Movement.canceled += ctx => Move(Vector2.zero);
        controls.Player1.Jump.started += Jump;
        controls.Player1.Jump.canceled += EndJump;
        controls.Player1.Dash.started += Dash;
        controls.Player1.Interact.started += Interact;
        controls.Player1.AttackA.performed += Attack;
        controls.Player1.AttackA.canceled += AttackCanceled;
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

    public void Attack(InputAction.CallbackContext context) {
        shooting = true;
        if (!character.Immobile)
        {
            StartCoroutine(ShootRoutine());
        }
    }
    void AttackCanceled(InputAction.CallbackContext context)
    {
        shooting = false;
    }
    IEnumerator ShootRoutine()
    {
        while (shooting)
        {
            if (!shootOnCooldown)
            {
                Shoot();
            }
            yield return null;
        }
    }
    void Shoot()
    {
        Vector3 shootDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var clone = Instantiate(bullet, transform.position + shootDir, Quaternion.identity).GetComponent<BulletController>();
        clone.Init(shootDir, false);
        shootOnCooldown = true;
        StartCoroutine(ShootCooldown());
    }
    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootInterval);
        shootOnCooldown = false;
    }

    /// <summary>
    /// Respawns the player at the last soft checkpoint while keeping their current stats
    /// </summary>
    public void SoftRespawn() {
        renderer.flipY = true;
        character.Immobile = true;
        Invoke("StartSoftRespawn", softRespawnDelay);
    }

    /// <summary>
    /// Starts the soft respwan after a delay and fades out the screen
    /// </summary>
    private void StartSoftRespawn() {
        StopAllCoroutines();
        cameraController.FadeOut();
        Invoke("EndSoftRespawn", softRespawnDuration);
    }

    /// <summary>
    /// Ends the soft respwan after the duration ended, repositions the player and fades in the screen
    /// </summary>
    private void EndSoftRespawn() {
        renderer.flipY = false;
        checkpoint.ReturnToSoftCheckpoint();
        cameraController.FadeIn();  
        character.Immobile = false;
        shootOnCooldown = false;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable() {
        controls.Player1.Enable();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable() {
        controls.Player1.Disable();
    }
}