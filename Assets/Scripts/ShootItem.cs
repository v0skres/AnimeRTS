using UnityEngine;

public class ShootItem : MonoBehaviour
{
    [Header("Настройки")]
    public Transform graphics;
    public int damage;
    public float flySpeed;
    public float rotateSpeed;

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
            Debug.Log("Shot the enemy");
            enemy.TakeDamage(damage);
            DestroyProjectile();
        }
        if (collision.CompareTag("Out"))
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

    //Handle rotation and flying
    void Update()
    {
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