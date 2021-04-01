using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    private GameObject player;
    private GameObject enemy;
    private GameObject station;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
        player = GameObject.Find("Player Ship");
        enemy = GameObject.Find("Enemy");
        station = GameObject.Find("Station");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isActiveAndEnabled) {
            transform.position += transform.forward * Time.deltaTime * 750;
        }
    }

    void OnCollisionEnter(Collision col) {
        SendDamage(col.gameObject.tag, col);
        Destroy(gameObject);
    }

    void SendDamage(string tagObject, Collision col) {
        if (tagObject == "Enemy") {
            GameObject parent = col.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            EnemyAI scriptEnemy = parent.gameObject.GetComponent<EnemyAI>();
            scriptEnemy.DamageForEnemy("Laser");
        } else if (player != null && tagObject == "Player" && gameObject.name != "GroupLeftLaserPlayer(Clone)" && gameObject.name != "GroupRightLaserPlayer(Clone)") {
            PlayerController scriptPlayer = player.GetComponent<PlayerController>();
            scriptPlayer.DamageForPlayer();
        } else if (tagObject == "Station") {
            GameObject parent = col.gameObject.transform.parent.gameObject;
            StationAI scriptStation = parent.gameObject.GetComponent<StationAI>();
            scriptStation.StationTakeDamage("Laser");
        }
    }

}
