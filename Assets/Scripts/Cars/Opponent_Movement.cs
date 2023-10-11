using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent_Movement : MonoBehaviour
{
    private float speed = 2f;
    public float minSpeed = 2f;
    public float maxSpeed = 8f;
    private float turnSpeed = 2f;

    private Transform respawnPosition;
    private Transform bottomBarrier;
    private Transform topBarrier;

    public Transform[] waypoints;
    public int targetPoint;

    private Vector3 tempPos;

    [Header("Sounds")]
    public AudioClip engineStart;
    public AudioClip hitClip;
    public AudioClip waterClip;
    public AudioClip grassClip;



    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);

        waypoints = new Transform[WaypointsSelector.Instance.waypoints.Length];
        waypoints = WaypointsSelector.Instance.waypoints;
        //respawnPosition = GameObject.FindWithTag("SpawnPoint").GetComponent<Transform>();
        topBarrier = UIManager.instance.topBarrier;
        bottomBarrier = UIManager.instance.bottomBarrier;
    }

    private void Update()
    {
        VerticalMovement();
        Clamp();
    }

    private void VerticalMovement()
    {
        //// Move the opponent vertically
        //if (transform.position.y == waypoints[targetPoint].position.y)
        //{
        //    IncreaseTargetPoint();
        //}
        //transform.position = Vector2.MoveTowards(transform.position, waypoints[targetPoint].position, speed * Time.deltaTime);

        //if (transform.position.x <= waypoints[targetPoint].position.x)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -47), turnSpeed * Time.deltaTime);
        //}
        //if (transform.position.x >= waypoints[targetPoint].position.x)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 47), turnSpeed * Time.deltaTime);
        //}

        //if (transform.rotation.z != 90)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 10f * Time.deltaTime);
        //}

        Vector2 direction = (waypoints[targetPoint].position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // Rotate towards the current waypoint.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (transform.position.y >= waypoints[targetPoint].position.y - 1f)
        {
            IncreaseTargetPoint();
        }

    }

    private void IncreaseTargetPoint()
    {
        targetPoint++;
        if(targetPoint >= waypoints.Length) 
            targetPoint = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Opponent"))
        {
            //GameDataManager.Instance.sprintRank.crash++;
            UIManager.instance.DisplayRaceResult();
            UIManager.instance.gameWinPanel.SetActive(true);
            Time.timeScale = 0f;
            SoundManager.instance.PlaySoundFX(hitClip, 0.9f);
        }

        //if (other.gameObject.CompareTag("Opponent") || other.gameObject.CompareTag("OtherCar"))
        //{
        //    Vector2 playerCenter = transform.position;
        //    Vector2 opponentCenter = other.transform.position;
        //    Vector2 collisionDirection = (playerCenter - opponentCenter).normalized;

        //    Rigidbody2D opponentRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
        //    if (opponentRigidbody != null)
        //    {
        //        float bounceForce = 1f;
        //        opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);
        //        opponentRigidbody.AddForce(-collisionDirection * bounceForce, ForceMode2D.Impulse);
        //    }
        //}

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Timer"))
        //{
        //    other.transform.parent.gameObject.SetActive(false);
        //}
        /*else*/ if (other.CompareTag("Coin"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Water"))
        {
            //GameDataManager.Instance.sprintRank.crash++;
            //tempPos = respawnPosition.position;
            SoundManager.instance.PlaySoundFX(waterClip, 0.2f);
            //StartCoroutine(RespawnAfterDelay(1f));
            UIManager.instance.DisplayRaceResult();
            UIManager.instance.gameWinPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (other.gameObject.CompareTag("Grass"))
        {
            //GameDataManager.Instance.sprintRank.crash++;
            //tempPos = respawnPosition.position;
            SoundManager.instance.PlaySoundFX(grassClip, 0.2f);
            //StartCoroutine(RespawnAfterDelay(1f));
            UIManager.instance.DisplayRaceResult();
            UIManager.instance.gameWinPanel.SetActive(true);
            Time.timeScale = 0f;
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

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(0.3f);

        float tempSpeed = speed;
        speed = 0f;

        yield return new WaitForSeconds(delay);

        SoundManager.instance.PlaySoundFX(engineStart, 0.2f);

        transform.position = tempPos;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        speed = tempSpeed;
    }
}
