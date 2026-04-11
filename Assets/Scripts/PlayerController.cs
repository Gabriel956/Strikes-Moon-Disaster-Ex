using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;

    private Vector2 currentMovementInput;
    private float elevationInput;

    private bool isAscending = false;
    private bool isDescending = false;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float elevationSpeed = 5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float projectileSpawnOffset = 0.2f;

    void OnMove(InputValue inputValue)
    {
        currentMovementInput = inputValue.Get<Vector2>();
        Debug.Log("currentMovementInput = " + currentMovementInput);
    }

    void OnChangeElevation(InputValue inputValue)
    {
        elevationInput = inputValue.Get<float>();
        Debug.Log("changeElevation = " + elevationInput);
    }

    void OnFire(InputValue inputValue)
    {
        if (!inputValue.isPressed)
        {
            return;
        }

        FireProjectile();
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(currentMovementInput.x, 0f, currentMovementInput.y);

        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            moveDirection = (cameraForward * moveDirection.z) + (cameraRight * moveDirection.x);
        }

        Vector3 movement = moveDirection * moveSpeed;
        movement.y = elevationInput * elevationSpeed;

        characterController.Move(movement * Time.deltaTime);
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is not assigned.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("Fire point is not assigned.");
            return;
        }

        Vector3 spawnPosition = firePoint.position + (firePoint.forward * projectileSpawnOffset);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, firePoint.rotation);

        Collider[] projectileColliders = projectile.GetComponentsInChildren<Collider>();
        Collider[] playerColliders = GetComponentsInChildren<Collider>();
        foreach (Collider projectileCollider in projectileColliders)
        {
            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(projectileCollider, playerCollider);
            }
        }

        ProjectileHit hit = projectile.GetComponent<ProjectileHit>();
        if (hit == null)
        {
            hit = projectile.AddComponent<ProjectileHit>();
        }

        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.linearVelocity = firePoint.forward * projectileSpeed;
        }
        else
        {
            SimpleProjectile mover = projectile.GetComponent<SimpleProjectile>();
            if (mover == null)
            {
                mover = projectile.AddComponent<SimpleProjectile>();
            }

            mover.Initialize(firePoint.forward, projectileSpeed);
        }

        if (projectileLifetime > 0f)
        {
            Destroy(projectile, projectileLifetime);
        }
    }
}
