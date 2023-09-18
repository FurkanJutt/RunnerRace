using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private TimerController timer;

    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float burst = 1.25f;

    public int currentLife;
    private int maxLife = 3;

    public Sprite normalSprite;
    public Sprite damagedSprite;

    private int respawnCount = 0;
    private bool isGameOver = false;

    public AudioClip hitClip;

    public Transform respawnPosition;

    public float distance;
    public int crash;

    [SerializeField] Sprite[] carSprites;
    public static Player_Movement instance;

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
        if (PlayerPrefs.GetInt("StickToggle", 0) == 0)
        {
            joystick = UIManager.instance.leftJoystick.GetComponent<FixedJoystick>();
        }
        else
        {
            joystick = UIManager.instance.rightJoystick.GetComponent<FixedJoystick>();
        }

        currentLife = maxLife;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        timer = GetComponent<TimerController>();

        //respawnPosition = transform.position;

        UpdateRespawnChancesText();

        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Clamp();
    }

    private void Movement()
    {
        distance += Time.deltaTime * speed;
        //distance += Time.deltaTime * speed;
        UIManager.instance.UpdateScore((int)distance);

        // Auto Movement
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        // Burst Movement
        if (Input.GetKey(KeyCode.UpArrow) || joystick.Vertical > 0)
        {
            transform.position += new Vector3(0, speed * burst * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || joystick.Horizontal > 0)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -47), rotationSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || joystick.Horizontal < 0)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 47), rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || joystick.Vertical < 0)
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
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
        // pos.y = Mathf.Clamp(pos.y, -3.8f, 3.8f);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isGameOver)
        {
            if (other.CompareTag("Opponent") || other.CompareTag("OtherCar"))
            {
                Vector2 playerCenter = transform.position;
                Vector2 opponentCenter = other.transform.position;
                Vector2 collisionDirection = (playerCenter - opponentCenter).normalized;

                currentLife--;
                crash++;
                GameDataManager.Instance.sprintRank.crash = crash;
                UIManager.instance.UpdateLives(currentLife);
                SoundManager.instance.PlaySoundFX(hitClip, 0.9f);

                if (currentLife <= 0 )
                {
                    if (respawnCount > 1)
                    {
                        StartCoroutine(RespawnAfterDelay(1f));
                    }
                    else
                    {
                        isGameOver = true;
                        Time.timeScale = 0f;
                        GameDataManager.Instance.enduranceRank.time = TimerController.instance.totalTimePlayed;
                        GameDataManager.Instance.enduranceRank.mile = UIManager.instance.Distance;
                        GameDataManager.Instance.AddEnduranceRank();
                        UIManager.instance.gameOverPanel.SetActive(true);
                        //FinishLane.fL.GameOver();
                        timer.DisplayTotalTimePlayed();
                        UIManager.instance.UpdateTotalDistance();
                    }
                    
                }

                Rigidbody2D opponentRigidbody = other.GetComponent<Rigidbody2D>();
                if (opponentRigidbody != null)
                {
                    // Adjust the bounce force based on collision direction
                    float bounceForce = 1f; // You can adjust this value as needed

                    // Apply a force to the player in the collision direction
                    opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);


                    // Apply a force to the opponent in the opposite direction
                    opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);
                }
            }
            else if (other.CompareTag("Water"))
            {
                Time.timeScale = 0f;
                UIManager.instance.gameOverPanel.SetActive(true);
                FinishLane.fL.GameOver();
                timer.DisplayTotalTimePlayed();
            }
            else if (other.CompareTag("Grass"))
            {
                StartCoroutine(RespawnAfterDelay(1f));
            }
            //else if (other.CompareTag("CheckPoint"))
            //{
            //    respawnPosition.position = transform.position;
            //}
        }
    }


    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (respawnCount < 1)
        {
            transform.position = respawnPosition.position;
            //currentLife = maxLife;
            //UIManager.instance.UpdateLives(currentLife);
            respawnCount++;

            UpdateRespawnChancesText();
        }
        else
        {
            isGameOver = true;
            Time.timeScale = 0f;
            GameDataManager.Instance.enduranceRank.time = TimerController.instance.totalTimePlayed;
            GameDataManager.Instance.enduranceRank.mile = UIManager.instance.Distance;
            GameDataManager.Instance.AddEnduranceRank();
            UIManager.instance.gameOverPanel.SetActive(true);
            FinishLane.fL.GameOver();
            timer.DisplayTotalTimePlayed();
            UIManager.instance.UpdateTotalDistance();
        }
    }

    private void UpdateRespawnChancesText()
    {
        int remainingChances = 3 - respawnCount;
        UIManager.instance.respawnChancesText.text = remainingChances.ToString();
    }


}
