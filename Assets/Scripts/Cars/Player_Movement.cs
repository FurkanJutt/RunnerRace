using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private TimerController timer;

    private bool isSprintMode = false;

    public float speed = 2f;
    public float rotationSpeed = 3f;
    private float turnSpeed = 2f;
    public float burst = 1.25f;

    public int currentLife;
    private int maxLife = 3;
    private int maxRespawn = 3;
    private int maxTime = 0;

    [Header("Sprites")]
    public Sprite normalSprite;
    public GameObject damagedSprite;

    [Header("Trails")]
    public GameObject trails;

    private int respawnCount = 0;
    //private bool isGameOver = false;

    private Rigidbody2D rb;

    public Transform respawnPosition;
    private Transform bottomBarrier;
    private Transform topBarrier;

    public float distance;
    public int crash;
    public int coin;

    [SerializeField] Sprite[] carSprites;
    public static Player_Movement instance;

    [Header("Sounds")]
    public AudioClip engineStart;
    public AudioClip hitClip;
    public AudioClip coinCollectClip;
    public AudioClip waterClip;
    public AudioClip grassClip;

    [HideInInspector]
    public FixedJoystick joystick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.GetComponent<SpriteRenderer>().sprite = carSprites[PlayerPrefs.GetInt("SelectCar", 0)];
    }

    private void Start()
    {
        if(UIManager.instance != null)
        {
            if (PlayerPrefs.GetInt("StickToggle", 0) == 0)
            {
                joystick = UIManager.instance.leftJoystick.GetComponent<FixedJoystick>();
            }
            else
            {
                joystick = UIManager.instance.rightJoystick.GetComponent<FixedJoystick>();
            }
        }
        rb = GetComponent<Rigidbody2D>();

        topBarrier = UIManager.instance.topBarrier;
        bottomBarrier = UIManager.instance.bottomBarrier;

        isSprintMode = UIManager.instance.sprintMode;
        maxLife = UIManager.instance.GetMaxLife();
        maxRespawn = UIManager.instance.GetMaxRespawn();
        maxTime = GetMaxTime();
        currentLife = maxLife;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        timer = GetComponent<TimerController>();

        UpdateRespawnChancesText();

        damagedSprite.SetActive(false);
        trails.SetActive(false);

        SoundManager.instance.PlaySoundFX(engineStart, 0.9f);
    }

    public int GetMaxTime()
    {
        if (PlayerPrefs.GetInt("Time3", 0) == 1)
            return 3;
        else if (PlayerPrefs.GetInt("Time2", 0) == 1)
            return 2;
        else if (PlayerPrefs.GetInt("Time1", 0) == 1)
            return 1;
        else
            return 0;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Clamp();
    }

    private void Movement()
    {
        if (!isSprintMode)
        {
            distance += Time.deltaTime * speed;
            //distance += Time.deltaTime * speed;
            UIManager.instance.UpdateScore((int)distance);
        }

        // Auto Movement
        if (!Input.GetKey(KeyCode.UpArrow) && joystick.Vertical != 1)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            trails.SetActive(false);
        }

        //// Burst Movement
        //if (Input.GetKey(KeyCode.UpArrow) || joystick.Vertical == 1 && !isSprintMode)
        //{
        //    transform.position += new Vector3(0, burst * Time.deltaTime, 0);
        //}

        // Burst Movement
        if (Input.GetKey(KeyCode.UpArrow) || joystick.Vertical == 1)
        {
            transform.position += new Vector3(0, burst  * Time.deltaTime, 0);
            trails.SetActive(true);
        }

        if (Input.GetKey(KeyCode.RightArrow) || joystick.Horizontal == 1)
        {
            transform.position += new Vector3(speed / rotationSpeed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -47), turnSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || joystick.Horizontal == -1)
        {
            transform.position -= new Vector3(speed / rotationSpeed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 47), turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || joystick.Vertical == -1)
        {
            transform.position -= new Vector3(0, speed / 3 * Time.deltaTime, 0);
        }

        if (transform.rotation.z != 90)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 5f * Time.deltaTime);
        }
    }

    private void Clamp()
    {
        //Unity Inbuilt Feature
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.20f, 2.20f);
        pos.y = Mathf.Clamp(pos.y, bottomBarrier.position.y, topBarrier.position.y);
        // pos.y = Mathf.Clamp(pos.y, -3.8f, 3.8f);
        transform.position = pos;
    }

    float bounceForce = 5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.CompareTag("Opponent") || other.gameObject.CompareTag("OtherCar"))
        //{
        //    Vector2 playerCenter = transform.position;
        //    Vector2 opponentCenter = other.transform.position;
        //    Vector2 collisionDirection = (playerCenter - opponentCenter).normalized;

        //    if (!isSprintMode)
        //    {
        //        currentLife--;
        //        crash++;
        //        GameDataManager.Instance.sprintRank.crash = crash;
        //        UIManager.instance.UpdateLives(currentLife);
        //        damagedSprite.SetActive(true);

        //        if (currentLife < 0 )
        //        {
        //            if (respawnCount < maxRespawn)
        //            {
        //                StartCoroutine(RespawnAfterDelay(1f));
        //            }
        //            else
        //            {
        //                GameDataManager.Instance.EndGame();
        //            }
        //        }
        //    }
        //    SoundManager.instance.PlaySoundFX(hitClip, 0.9f);

        //    Rigidbody2D opponentRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
        //    if (opponentRigidbody != null)
        //    {
        //        // Adjust the bounce force based on collision direction
        //        float bounceForce = 1f; // You can adjust this value as needed

        //        // Apply a force to the player in the collision direction
        //        opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);


        //        // Apply a force to the opponent in the opposite direction
        //        opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);
        //    }
        //}
        if (other.gameObject.CompareTag("Opponent"))
        {
            UIManager.instance.gameOverPanel.SetActive(true);
            SoundManager.instance.PlaySoundFX(hitClip, 0.9f);
            GameDataManager.Instance.EndGame();
        }
        else if (other.gameObject.CompareTag("OtherCar"))
        {
            StartCoroutine(PushCar(other));
        }
        //else if (other.CompareTag("CheckPoint"))
        //{
        //    respawnPosition.position = transform.position;
        //}

    }

    private IEnumerator PushCar(Collision2D other)
    {
        Vector2 collisionDirection = (other.transform.position - transform.position).normalized;
        Rigidbody2D opponentRB = other.gameObject.GetComponent<Rigidbody2D>();

        SoundManager.instance.PlaySoundFX(hitClip, 0.9f);

        rb.AddForce(-collisionDirection * bounceForce / 2, ForceMode2D.Impulse);

        opponentRB.AddForce(collisionDirection * bounceForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        opponentRB.velocity = Vector2.zero;
        opponentRB.angularVelocity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            UIManager.instance.gameOverPanel.SetActive(true);
            SoundManager.instance.PlaySoundFX(waterClip, 0.9f);
            GameDataManager.Instance.EndGame();
        }
        else if (other.gameObject.CompareTag("Grass"))
        {
            SoundManager.instance.PlaySoundFX(grassClip, 0.9f);
            StartCoroutine(RespawnAfterDelay(1f));
        }
        else if (other.CompareTag("Coin"))
        {
            SoundManager.instance.PlaySoundFX(coinCollectClip, 0.9f);
            coin++;
            GameDataManager.Instance.playerCoins++;
            PlayerPrefs.SetInt("TotalScore", GameDataManager.Instance.playerCoins);
            UIManager.instance.UpdateCoinText(coin.ToString());
            //int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            //Debug.Log("Total Coins: " + GameDataManager.Instance.playerCoins);
            other.transform.parent.gameObject.SetActive(false);
        }
    }


    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(0.3f);

        float tempCamSpeed = CameraMovement.instance.cameraSpeed;
        float tempSpeed = speed;
        speed = 0f;
        CameraMovement.instance.cameraSpeed = 0f;

        yield return new WaitForSeconds(delay);

        UIManager.instance.gameOverPanel.SetActive(true);
        GameDataManager.Instance.EndGame();

        //if (!isSprintMode)
        //{
        //if (respawnCount < maxRespawn)
        //{
        //    SoundManager.instance.PlaySoundFX(engineStart, 0.9f);
        //    transform.position = respawnPosition.position;
        //    //currentLife = maxLife;
        //    //UIManager.instance.UpdateLives(currentLife);
        //    respawnCount++;

        //    speed = tempSpeed;
        //    CameraMovement.instance.cameraSpeed = tempCamSpeed;

        //    UpdateRespawnChancesText();
        //}
        //else
        //{
        //    EndGame();
        //}
        //}
        //else
        //{
        //    SoundManager.instance.PlaySoundFX(engineStart, 0.9f);
        //    transform.position = respawnPosition.position;
        //    speed = tempSpeed;
        //    CameraMovement.instance.cameraSpeed = tempCamSpeed;
        //}
    }

    private void UpdateRespawnChancesText()
    {
        int remainingChances = maxRespawn - respawnCount;
        UIManager.instance.respawnChancesText.text = remainingChances.ToString();
    }

    //private void EndGame()
    //{
    //    Time.timeScale = 0f;
    //    if (!isSprintMode)
    //    {
    //        GameDataManager.Instance.enduranceRank.time = TimerController.instance.totalTimePlayed;
    //        GameDataManager.Instance.enduranceRank.mile = UIManager.instance.Distance;
    //        GameDataManager.Instance.AddEnduranceRank();
    //        timer.DisplayTotalTimePlayed();
    //        UIManager.instance.UpdateTotalDistance();
    //    }
    //    else
    //        FinishLane.instance.GameOver();

    //    UIManager.instance.gameOverPanel.SetActive(true);
    //    PlayerPrefs.SetInt($"Respawn{maxRespawn}", 0);
    //    PlayerPrefs.SetInt($"Life{maxLife}", 0);
    //    PlayerPrefs.SetInt($"Time{maxTime}", 0);
    //}
}
