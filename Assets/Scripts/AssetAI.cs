using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetAI : MonoBehaviour
{

  private GameObject stateManager;

  private GameObject player;

  // Start is called before the first frame update
  void Start()
  {
    stateManager = GameObject.Find("StateManager");
    player = GameObject.Find("Player Ship");
  }

  void OnCollisionEnter(Collision col) {
    if (col.gameObject.name == "Player Ship") {
      player.GetComponent<PlayerController>().AddAssetScore();
      stateManager.GetComponent<CurrentState>().remainingAssets -= 1;
      if (stateManager.GetComponent<CurrentState>().remainingAssets == 0) {
        stateManager.GetComponent<CurrentState>().gameFinished = true;
        stateManager.GetComponent<CurrentState>().scoreScreen.SetActive(true);
      }
      Destroy(gameObject);
    }
  }
}
