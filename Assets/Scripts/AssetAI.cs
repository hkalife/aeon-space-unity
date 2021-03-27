using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetAI : MonoBehaviour
{

  private GameObject stateManager;

  // Start is called before the first frame update
  void Start()
  {
    stateManager = GameObject.Find("StateManager");
  }

  void OnCollisionEnter(Collision col) {
    Debug.Log(col.gameObject.name);
    if (col.gameObject.name == "Player Ship") {
      stateManager.GetComponent<CurrentState>().remainingAssets -= 1;
      if (stateManager.GetComponent<CurrentState>().remainingAssets == 0) {
        stateManager.GetComponent<CurrentState>().gameFinished = true;
        stateManager.GetComponent<CurrentState>().scoreScreen.SetActive(true);
      }
      Destroy(gameObject);
    }
  }
}
