using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectable : MonoBehaviour
{

  private string[] possibleTypes = {"Health", "Missile"};

  [SerializeField]
  private string type;

  [SerializeField]
  private GameObject player;

  [SerializeField]
  private TMP_Text missileQuantityText;

  [SerializeField]
  private TMP_Text messageCrate;

  [SerializeField]
  private GameObject currentState;

  void Start() {
    type = possibleTypes[Random.Range(0, possibleTypes.Length)];
  }

  void OnCollisionEnter(Collision col) {
    if (col.gameObject.tag == "Player") {
      if (type == "Health") {
        messageCrate.text = "+10 vida";
        player.GetComponent<PlayerController>().playerHealth += 10;
      } else if (type == "Missile") {
        int newMissiles = Random.Range(1, 5);
        messageCrate.text = "+" + newMissiles.ToString() + " mísseis";
        player.GetComponent<PlayerController>().missileQuantity += newMissiles;
        missileQuantityText.text = "x" + player.GetComponent<PlayerController>().missileQuantity.ToString();
      }
      currentState.GetComponent<CurrentState>().CallClean();
      Destroy(gameObject);
    }
  }
}
