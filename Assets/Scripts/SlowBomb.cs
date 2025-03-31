using System.Collections;
using UnityEngine;

public class SlowBomb : MonoBehaviour
{
    public GameObject explosionPrefab; // ������ ������� ������
    public LayerMask levelMask; // ����� ��� ������ (�����, �����������)
    public LayerMask levelMask2; // �������������� ����� (���� �����)
    public float explosionTime = 5f; // ����� �������� ����� ������� (���������)
    public float explosionRadius = 6f; // ������ ������ (��������)
    public float explosionForce = 1500f; // ���� ������ (���������)
    private bool exploded = false;
    private Collider bombCollider;

    void Start()
    {
        // �������� ��������� �����
        bombCollider = GetComponent<Collider>();

        // ��������� ��������� �����, ����� �������� �� ���������
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // ��������� ������� �����
        FixPosition();

        // ������������� �������� ����� �������
        Invoke("PrepareExplosion", explosionTime - 0.1f); // �������� ��������� ����� �������
        Invoke("Explode", explosionTime);
    }

    private void FixPosition()
    {
        // ��������� Rigidbody, ���� �� ����
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ������ Rigidbody ��������������
        }

        // ��������� ������� �����
        transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            0, // ���������, ��� ��� �������� ������������� ������ �����
            Mathf.RoundToInt(transform.position.z)
        );
    }

    private void PrepareExplosion()
    {
        // �������� ��������� ����� �������
        if (bombCollider != null)
        {
            bombCollider.enabled = true;
        }
    }

    void Explode()
    {
        // ������� ������ ������
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // ������� ������ � ������� ������������
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        // ��������� ���������� ����������� �����
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;

        // ��������� ��������� �����
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // ��������� ���� ������ � �������� � �������
        ApplyExplosionForce();

        // ���������� ����� ����� 0.3 �������
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        // ����������� ���������� �������� �� 3, ����� �������� 3 �����
        for (int i = 1; i <= 3; i++) // ������ �������� 3 �����
        {
            RaycastHit hit;
            RaycastHit hit2;

            // ���������, ���� �� ����������� �� ���� ������
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit2, i, levelMask2);

            if (!hit.collider && !hit2.collider)
            {
                // ������� ������ ������ � ������� �������
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            {
                break; // ������������� �����, ���� ���������� �� �����������
            }

            yield return new WaitForSeconds(.05f);
        }
    }

    private void ApplyExplosionForce()
    {
        // �������� ��� ������� � ������� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // ��������� ���� � �������� � Rigidbody
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // ������� ���� �������
            Player player = nearbyObject.GetComponent<Player>();
            if (player != null)
            {
                player.healthUpdate(); // ��������� �������� ������
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode(); // �������� �����, ���� ��� �������� ��� ������ �����
        }
    }
}