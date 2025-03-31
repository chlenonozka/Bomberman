using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{
    public GameObject explosionPrefab; // ������ ������ (���� �� �����)
    public float activationDelay = 0.5f; // �������� ����� ���������� ���� (� ��������)
    private Player owner; // �����, ������� �������� ����
    private bool isTriggered = false; // ����, ����� ���� ����������� ������ ���� ���
    private bool isActive = false; // ����, �����������, ������� �� ����

    // ����� ��� ��������� ��������� ����
    public void SetOwner(Player player)
    {
        owner = player;
    }

    private void Start()
    {
        // ��������� �������� ��� ��������� ���� � ���������
        StartCoroutine(ActivateMine());
    }

    private IEnumerator ActivateMine()
    {
        // ��� ��������� ����� ����� ����������
        yield return new WaitForSeconds(activationDelay);
        isActive = true; // ���� ���������� ��������
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ���� �������, �� �� �������� �����, � ��� ��� �� ���������
        if (isActive && collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                isTriggered = true; // ��������, ��� ���� ���������

                // ������ ������ ������
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }

                // ������� ������, ���� ��� ��������
                if (player != owner) // ���� �� ������ ������� ����, ��� � ��������
                {
                    player.deadPlayer(); // ����������� ������� ������, ��������� ��������
                }

                // ���������� ���������, ��� ���� ����������
                if (owner != null)
                {
                    owner.MineExploded();
                }

                // ���������� ����
                Destroy(gameObject);
            }
        }
    }
}