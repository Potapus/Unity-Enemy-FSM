using UnityEngine;
using EnemyFSM;

[RequireComponent(typeof(Rigidbody))]
public class EnemyKnockback : MonoBehaviour, IKnockbackHandler
{
    [SerializeField] private float knockbackDuration = 0.3f;

    private Rigidbody rb;
    private float knockbackTimer;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsActive) return;

        knockbackTimer -= Time.deltaTime;
        if (knockbackTimer <= 0f)
            IsActive = false;
    }

    public void ApplyKnockback(Vector3 force)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
        knockbackTimer = knockbackDuration;
        IsActive = true;
    }
}
