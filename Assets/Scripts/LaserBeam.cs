using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Combat Settings")]
    public int damage;
    public float flySpeed;
    public float rotateSpeed;

    private bool _isPaused = false;
    private Vector2 _savedVelocity;

    [Header("Visuals")]
    public Transform graphics;

    //METHODS
    //Init
    public void Init(int damageAmount)
    {
        damage = damageAmount;
    }

    public void SetPaused(bool paused)
    {
        _isPaused = paused;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (paused)
            {
                _savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
                rb.velocity = _savedVelocity;
            }
        }

        // Для аниматоров
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = !paused;
        }

        // Для частиц
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            if (paused) ps.Pause();
            else ps.Play();
        }
    }

    void Update()
    {
        if (_isPaused) return;

        Rotate();
        FlyForward();
    }

    void Rotate() => graphics.Rotate(0, 0, -rotateSpeed * Time.deltaTime);

    void FlyForward() => transform.Translate(transform.right * flySpeed * Time.deltaTime, Space.World);

    //Trigger with enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Out"))
        {
            Destroy(gameObject);
        }
    }
    //Handle rotation and flying
}
