using UnityEngine;

public class ShootItem : MonoBehaviour
{
    [Header("Настройки")]
    public Transform graphics;
    public int damage;
    public float flySpeed;
    public float rotateSpeed;

    private bool _isPaused = false;
    private Vector2 _savedVelocity;

    [Header("Физика")]
    public bool usePhysics = false; // Если нужно физическое движение
    private Rigidbody2D rb;

    //METHODS
    //Init
    private void Start()
    {
        if (usePhysics)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
            rb.velocity = transform.right * flySpeed;
        }
    }

    public void Init(int damageAmount)
    {
        damage = damageAmount;
    }

    //Trigger with enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(damage);
            DestroyProjectile();
        }
        else if (collision.CompareTag("Out"))
        {
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(damage);
            DestroyProjectile();
        }
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

    //Handle rotation and flying
    void Update()
    {
        if (_isPaused) return;

        if (!usePhysics)
        {
            Rotate();
            FlyForward();
        }
    }
    void Rotate()
    {
        graphics.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
    }
    void FlyForward()
    {
        transform.Translate(transform.right * flySpeed * Time.deltaTime, Space.World);
    }

    void DestroyProjectile()
    {
        // Отключаем коллайдер перед уничтожением
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        Destroy(gameObject);
    }
}