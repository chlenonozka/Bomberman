using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{
    public GameObject explosionPrefab; // Префаб взрыва (взят от бомбы)
    public float activationDelay = 0.5f; // Задержка перед активацией мины (в секундах)
    private Player owner; // Игрок, который поставил мину
    private bool isTriggered = false; // Флаг, чтобы мина срабатывала только один раз
    private bool isActive = false; // Флаг, указывающий, активна ли мина

    // Метод для установки владельца мины
    public void SetOwner(Player player)
    {
        owner = player;
    }

    private void Start()
    {
        // Запускаем корутину для активации мины с задержкой
        StartCoroutine(ActivateMine());
    }

    private IEnumerator ActivateMine()
    {
        // Ждём указанное время перед активацией
        yield return new WaitForSeconds(activationDelay);
        isActive = true; // Мина становится активной
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что мина активна, на неё наступил игрок, и она ещё не сработала
        if (isActive && collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                isTriggered = true; // Отмечаем, что мина сработала

                // Создаём эффект взрыва
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }

                // Убиваем игрока, если это соперник
                if (player != owner) // Мина не должна убивать того, кто её поставил
                {
                    player.deadPlayer(); // Моментально убиваем игрока, игнорируя здоровье
                }

                // Уведомляем владельца, что мина взорвалась
                if (owner != null)
                {
                    owner.MineExploded();
                }

                // Уничтожаем мину
                Destroy(gameObject);
            }
        }
    }
}