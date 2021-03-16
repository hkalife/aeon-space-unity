using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private enum State {
        Stop,
        ChasePlayer,
        AttackPlayer,
        ChaseAndAttack
    }

    [SerializeField]
    private State state;

    private GameObject player;

    [SerializeField]
    private GameObject leftLaser;

    [SerializeField]
    private GameObject rightLaser;

    [SerializeField]
    private GameObject leftLaserPosition;

    [SerializeField]
    private GameObject rightLaserPosition;

    private bool allowAttack;

    [SerializeField]
    private int enemyHealth;

    [SerializeField]
    private GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Stop;
        player = GameObject.Find("Player Ship");
        allowAttack = true;
        enemyHealth = 100;
    }

    void Update() {
        CheckLife();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleStateMachine();
        ActionsStateMachine();
    }

    void ChasePlayer() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 50f * Time.deltaTime);

        Quaternion lookOnLook = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 5);
    }

    void AttackPlayer() {
        Quaternion lookOnLook = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 5);
        if (allowAttack) {
            GameObject newLeftLaser = Instantiate(leftLaser, leftLaserPosition.transform.position, leftLaserPosition.transform.rotation);
            GameObject newRightLaser = Instantiate(rightLaser, rightLaserPosition.transform.position, rightLaserPosition.transform.rotation);
            newLeftLaser.SetActive(true);
            newRightLaser.SetActive(true);
            allowAttack = false;
            StartCoroutine(WaitSeconds(5));
        }
    }

    void ChaseAndAttack() {
        AttackPlayer();
        ChasePlayer();
    }

    void HandleStateMachine() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < 150.0f) {
            state = State.AttackPlayer;
        } else if (distanceToPlayer > 150.0f && distanceToPlayer < 300.0f) {
            state = State.ChaseAndAttack;
        } else if (distanceToPlayer > 300.0f && distanceToPlayer < 450.0f) {
            state = State.ChasePlayer;
        } else if (distanceToPlayer > 450.0f) {
            state = State.Stop;
        }
    }

    void ActionsStateMachine() {
        switch(state) {
            default:
            case State.Stop:
                break;
            case State.ChasePlayer:
                ChasePlayer();
                break;
            case State.AttackPlayer:
                AttackPlayer();
                break;
            case State.ChaseAndAttack:
                ChaseAndAttack();
                break;
        }
    }

    IEnumerator WaitSeconds(int seconds) {
        yield return new WaitForSeconds(2);
        allowAttack = true;
    }

    public void DamageForEnemy() {
        enemyHealth -= 50;
    }

    public void CheckLife() {
        if (enemyHealth <= 0) {
            PlayerController scriptPlayer = player.GetComponent<PlayerController>();
            scriptPlayer.AddScore();

            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(20, 20, 20);

            Destroy(gameObject);
        }
    }

}
