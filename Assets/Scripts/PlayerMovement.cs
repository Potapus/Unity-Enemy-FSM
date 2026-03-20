using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 720f;

    private Rigidbody rb;
    private Camera cam;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight   = cam.transform.right;
        camForward.y = 0f;
        camRight.y   = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime));
        }
    }
}
