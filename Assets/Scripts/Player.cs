using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

public class Player : MonoBehaviour
{
    [Header("Mine Settings")]
    public GameObject minePrefab; 
    public int maxMines = 2; 
    private int currentMines = 0; 
    [Header("Slow Bomb")]
    public GameObject slowBombPrefab;
    private int lifes;
    public GlobalStateManager globalManager;
    [Range(1, 2)] 
    public int playerNumber = 1;
    public float moveSpeed = 4f;
    public float rotateSpeed = 1.0f;
    public bool canDropBombs = true;
    public bool canMove = true;
    public int bombsCount;
    public int healthCount;
    public Text bombsText;
    public int maxHealthCount;
    public bool haveHealth = false;
    private bool isRunning = false;

    private bool playerOnePressed;
    private bool playerTwoPressed;

    [Header("SuperBomb")]
    public GameObject superBombPrefab;
    public bool haveSuperBomb;

    [Header("Shield")]
    public bool haveShield;
    public GameObject shieldEffect;

    private bool dead = false;

    private bool gameEnd = false;

    public bool onTheBomb = false;

    private int bombs = 2;

    [Header("StepSystem")]
    public AudioClip[] footstepSounds;
    public AudioSource audioSource;

    public float minDelay = 0.1f;
    public float maxDelay = 0.5f;

    private float timeSinceLastFootstep;

    public GameObject bombPrefab;

    private Rigidbody rigidBody;
    private Transform myTransform;
    public Animator animator;
    public GameObject effectShell;
    private Transform playerModelTransform;
    public Text healthText;
    public GameObject healthImage;
    public GameObject cameraPlayer;

    public GameObject zipImage;
    public GameObject shieldImage;
    public GameObject superBombImage;

    public UnityEvent deadEvent;
    public UnityEvent pickup;

    private bool isShielded = false;
    private bool isBoosted = false;

    public UnityEvent minusHealth;
    public UnityEvent plusHealth;
    
   
    void Start ()
    {
        loadLifesSettings();
        if (lifes == 0)
        {
            haveHealth = false;
        }
        else { haveHealth = true; }
        
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        playerModelTransform = myTransform.Find("PlayerModel").GetComponent<Transform>();
        healthCount = maxHealthCount;
        if(haveHealth == false)
        {
            healthText.gameObject.SetActive(false);
            healthImage.gameObject.SetActive(false);
        }
    }

    
    void Update ()
    {
        if (gameEnd == false)
        {
            UpdateMovement();
        }
        else { animator.SetBool("Walking", false); animator.SetBool("Running", false); }
        if (dead == false && gameEnd == true)
        {
            cameraPlayer.SetActive(true);
            animator.SetTrigger("Victory");
        }
        healthText.text = healthCount.ToString ();
        bombsText.text = bombsCount.ToString();

        if(rigidBody.velocity.magnitude > 2f)
        {
            if (Time.time - timeSinceLastFootstep >= UnityEngine.Random.Range(0.3f, 0.6f))
            {
                PlayFootstepSound();

                timeSinceLastFootstep = Time.time;
            }
        }
    }

    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false);
        animator.SetBool("Running", false);

        if (!canMove)
        { 
            return;
        }

        
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[randomIndex];
            audioSource.Play();
        }
    }

   
    private void UpdatePlayer1Movement ()
    {
        if (Input.GetKey (KeyCode.W) && playerOnePressed == false)
        { //Up movement
            playerOnePressed = true;
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 360, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerOnePressed = false; }

        if (Input.GetKey (KeyCode.A) && playerOnePressed == false)
        { 
            playerOnePressed = true;
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerOnePressed = false; }

        if (Input.GetKey (KeyCode.S) && playerOnePressed == false)
        { 
            playerOnePressed = true;
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerOnePressed = false; }

        if (Input.GetKey (KeyCode.D) && playerOnePressed == false)
        { 
            playerOnePressed = true;
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerOnePressed = false; }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
        { 
            DropBomb ();
        }

        if (canDropBombs && Input.GetKeyDown(KeyCode.R) && haveSuperBomb)
        { 
            DropSuperBomb ();
        }
        if (canDropBombs && Input.GetKeyDown(KeyCode.Q)) // Замедленная бомба на Q
        {
            DropSlowBomb();
        }
        if (Input.GetKeyDown(KeyCode.M) && currentMines < maxMines)
        {
            PlaceMine();
        }

    }

    
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow) && playerTwoPressed == false)
        { 
            playerTwoPressed = true;
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerTwoPressed = false; }

        if (Input.GetKey (KeyCode.LeftArrow) && playerTwoPressed == false)
        { 
            playerTwoPressed = true;
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerTwoPressed = false; }

        if (Input.GetKey (KeyCode.DownArrow) && playerTwoPressed == false)
        { 
            playerTwoPressed = true;
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerTwoPressed = false; }

        if (Input.GetKey (KeyCode.RightArrow) && playerTwoPressed == false)
        { 
            playerTwoPressed = true;
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }
        else { playerTwoPressed = false; }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { 
            DropBomb ();
        }

        if (canDropBombs && haveSuperBomb && (Input.GetKeyDown(KeyCode.RightShift))) //Backslash
        { 
            DropSuperBomb();
        }
        if (canDropBombs && Input.GetKeyDown(KeyCode.L)) // Замедленная бомба на L
        {
            DropSlowBomb();
        }
        if (Input.GetKeyDown(KeyCode.P) && currentMines < maxMines)
        {
            PlaceMine();
        }

    }

    
    private void DropSlowBomb()
    {
        if (slowBombPrefab && onTheBomb == false && bombsCount != 0)
        {
            bombsCount--;
            Vector3 bombPosition = new Vector3(Mathf.RoundToInt(myTransform.position.x), 0, Mathf.RoundToInt(myTransform.position.z));
            Instantiate(slowBombPrefab, bombPosition, slowBombPrefab.transform.rotation);
        }
    }

    private void DropBomb ()
    {
        if (bombPrefab && onTheBomb == false && bombsCount != 0)
        {
            bombsCount--;
            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
            bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
            bombPrefab.transform.rotation);
        }
    }

    private void DropSuperBomb()
    {
        if (superBombPrefab && onTheBomb == false && haveSuperBomb == true)
        {
            haveSuperBomb = false;
            superBombImage.SetActive(false);
            Instantiate(superBombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x),
            superBombPrefab.transform.position.y + 0.5f, Mathf.RoundToInt(myTransform.position.z)),
            superBombPrefab.transform.rotation);
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            onTheBomb = false;
            if (haveShield == false) {
                if (haveHealth == false)
                {
                    deadEvent.Invoke();
                }
                else { minusHealth.Invoke(); }
            }
        }

        if (other.CompareTag("Explosion_super"))
        {
            onTheBomb = false;
            if (haveShield == false)
            {
                if (haveHealth == false)
                {
                    deadEvent.Invoke();
                }
                else { minusHealth.Invoke(); }
            }
        }

        if (other.CompareTag("Bomb"))
        {
            onTheBomb = true;
        }

        if(other.CompareTag("PickUp_Bomb"))
        {
            if (bombsCount < 99)
            {
                pickup.Invoke();
            }
        }

        if(other.CompareTag("SpeedBooster"))
        {
            if (isBoosted == false)
            {
                StartCoroutine(speedBoosted());
            }
            else { return; }
        }

        if (other.CompareTag("PickUp_SuperBomb"))
        {
            haveSuperBomb = true;
            superBombImage.SetActive(true);
        }

        if(other.CompareTag("PickUp_Shield"))
        {
            if (isShielded == false)
            {
                StartCoroutine(shieldTime());
            }
            else { return; }
        }

        if(other.CompareTag("Spear"))
        {
            if (haveShield == false)
            {
                if (haveHealth == false)
                {
                    deadEvent.Invoke();
                }
                else { minusHealth.Invoke(); }
            }
        }

        if (other.CompareTag("FallTrigger"))
        {
            if (haveHealth == false)
            {
                deadEvent.Invoke();
            }
            else { deadEvent.Invoke(); healthCount = 0; }
        }

        if (other.CompareTag("PickUp_Health"))
        {
            if (healthCount != 3 && healthCount > 0)
            {
                plusHealth.Invoke();
            }
            else { return; }
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            onTheBomb = false;
        }
    }

    public void healthOn()
    {
        haveHealth = true;
    }

    public void healthUpdate()
    {
        if (healthCount > 0)
        {
            animator.SetTrigger("Hit");
            healthCount--;
        }

        if (healthCount == 0)
        {
            deadEvent.Invoke();
        }
    }
    private void PlaceMine()
    {
        if (minePrefab && onTheBomb == false) 
        {
            currentMines++; // Увеличиваем счётчик установленных мин
            Vector3 minePosition = new Vector3(Mathf.RoundToInt(myTransform.position.x), 0, Mathf.RoundToInt(myTransform.position.z));
            GameObject mine = Instantiate(minePrefab, minePosition, Quaternion.identity);
            Mine mineScript = mine.GetComponent<Mine>(); // Получаем скрипт мины
            if (mineScript != null)
            {
                mineScript.SetOwner(this); 
            }
        }
    }
    public void MineExploded()
    {
        if (currentMines > 0)
        {
            currentMines--;
        }
    }
    public void deadPlayer()
    {
        if(dead == false) {
            animator.SetBool("Dead", true);
            Debug.Log("P" + playerNumber + " hit by explosion!");
            dead = true;
            globalManager.PlayerDied(playerNumber);
        }
    }

    public void endGame()
    {
        gameEnd = true;
    }

    public void bombsPlus()
    {
        bombsCount++;
    }

    private IEnumerator speedBoosted()
    {
        if(moveSpeed != 5f) {
            isBoosted = true;
            animator.SetBool("Running", true);
            float currentSpeed = moveSpeed;
            zipImage.SetActive(true);
            moveSpeed = 5f;
            yield return new WaitForSeconds(5f);
            animator.SetBool("Running", false);
            isBoosted = false;
            zipImage.SetActive(false);
            moveSpeed = currentSpeed;
        }
    }

    private IEnumerator shieldTime()
    {
        if (haveShield == false)
        {
            isShielded = true;
            haveShield = true;
            effectShell.SetActive(true);
            shieldImage.SetActive(true);
            shieldEffect.SetActive(true);
            yield return new WaitForSeconds(8f);
            isShielded = false;
            shieldEffect.SetActive(false);
            effectShell.SetActive(false);
            shieldImage.SetActive(false);
            haveShield = false;
        }
    }

    public bool getShielded()
    {
        return isShielded;
    }

    public bool getBoosted()
    {
        return isBoosted;
    }

    public int getHealth()
    {
        return healthCount;
    }

    public void healthOnePlus()
    {
        healthCount++;
    }

    public void loadLifesSettings()
    {
        if (PlayerPrefs.HasKey("lifes"))
        {
            lifes = PlayerPrefs.GetInt("lifes");
        }
        else
        {
            lifes = 0;
            Debug.LogError("There is no save data!");
        }
    }
}
