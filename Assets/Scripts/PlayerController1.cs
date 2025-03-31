using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController1 : MonoBehaviour
{

    public float speed = 2.0f;
    public GameObject character;

    public GameObject bombPrefab;
    public int bombsCount;
    private bool onTheBomb = false;

    private Rigidbody rigidBody;
    private Transform myTransform;

    private int healthCount;
    public int maxHealthCount;
    private bool dead = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        healthCount = maxHealthCount;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        { //Drop bomb
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (bombPrefab && onTheBomb == false && bombsCount != 0)
        {
            bombsCount--;
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
            bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
            bombPrefab.transform.rotation);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            deadPlayer();
        }

        if (other.CompareTag("Bomb"))
        {
            onTheBomb = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            onTheBomb = false;
        }
    }

    public void deadPlayer()
    {
        if (dead == false)
        {
            Debug.Log("hit by explosion!");
            dead = true;
        }
    }
}
