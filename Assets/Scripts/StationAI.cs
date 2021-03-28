using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationAI : MonoBehaviour
{

  public int stationHealth;

  [SerializeField]
  private GameObject explosionEffect;

  private GameObject player;

  private GameObject state;
  private GameObject canvas;

  // Start is called before the first frame update
  void Start()
  {
      stationHealth = 200;
      player = GameObject.Find("Player Ship");
      state = GameObject.Find("StateManager");
      canvas = GameObject.Find("Canvas");
  }

  public void StationTakeDamage(string origin) {
    if (origin == "Laser") {
      stationHealth -= 1;
    } else if (origin == "Missile") {
      stationHealth -= 50;
    }
  }

  // Update is called once per frame
  void Update()
  {
      if (stationHealth <= 0) {
        PlayerController scriptPlayer = player.GetComponent<PlayerController>();
        scriptPlayer.AddStationScore();

        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(300, 300, 300);

        state.GetComponent<CurrentState>().remainingStations -= 1;
        if (state.GetComponent<CurrentState>().remainingStations == 0) {
          state.GetComponent<CurrentState>().gameFinished = true;
          canvas.GetComponent<APIController>().GoToScreen(1);
        }

        Destroy(gameObject);
      }
  }
}
