using System.Collections;
using UnityEngine;

public class SlowBomb : MonoBehaviour
{
    public GameObject explosionPrefab; // Префаб эффекта взрыва
    public LayerMask levelMask; // Маска для уровня (стены, препятствия)
    public LayerMask levelMask2; // Дополнительная маска (если нужно)
    public float explosionTime = 5f; // Время задержки перед взрывом (увеличено)
    public float explosionRadius = 6f; // Радиус взрыва (увеличен)
    public float explosionForce = 1500f; // Сила взрыва (увеличена)
    private bool exploded = false;
    private Collider bombCollider;

    void Start()
    {
        // Кэшируем коллайдер бомбы
        bombCollider = GetComponent<Collider>();

        // Отключаем коллайдер бомбы, чтобы персонаж не застревал
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // Фиксируем позицию бомбы
        FixPosition();

        // Устанавливаем задержку перед взрывом
        Invoke("PrepareExplosion", explosionTime - 0.1f); // Включаем коллайдер перед взрывом
        Invoke("Explode", explosionTime);
    }

    private void FixPosition()
    {
        // Отключаем Rigidbody, если он есть
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Делаем Rigidbody кинематическим
        }

        // Фиксируем позицию бомбы
        transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            0, // Убедитесь, что это значение соответствует уровню земли
            Mathf.RoundToInt(transform.position.z)
        );
    }

    private void PrepareExplosion()
    {
        // Включаем коллайдер перед взрывом
        if (bombCollider != null)
        {
            bombCollider.enabled = true;
        }
    }

    void Explode()
    {
        // Создаем эффект взрыва
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Создаем взрывы в четырех направлениях
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        // Отключаем визуальное отображение бомбы
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;

        // Отключаем коллайдер бомбы
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // Применяем силу взрыва к объектам в радиусе
        ApplyExplosionForce();

        // Уничтожаем бомбу через 0.3 секунды
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        // Увеличиваем количество итераций до 3, чтобы взрывать 3 блока
        for (int i = 1; i <= 3; i++) // Теперь взрывает 3 блока
        {
            RaycastHit hit;
            RaycastHit hit2;

            // Проверяем, есть ли препятствия на пути взрыва
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit2, i, levelMask2);

            if (!hit.collider && !hit2.collider)
            {
                // Создаем эффект взрыва в текущей позиции
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            {
                break; // Останавливаем взрыв, если наткнулись на препятствие
            }

            yield return new WaitForSeconds(.05f);
        }
    }

    private void ApplyExplosionForce()
    {
        // Получаем все объекты в радиусе взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Применяем силу к объектам с Rigidbody
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Наносим урон игрокам
            Player player = nearbyObject.GetComponent<Player>();
            if (player != null)
            {
                player.healthUpdate(); // Уменьшаем здоровье игрока
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode(); // Взрываем бомбу, если она попадает под другой взрыв
        }
    }
}