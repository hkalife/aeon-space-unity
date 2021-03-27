using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

  [SerializeField]
  private GameObject firstMenu;

  [SerializeField]
  private GameObject secondMenu;

  [SerializeField]
  private GameObject thirdMenu;

  public TMP_Text warningLoginText;

  public TMP_Text userMessage;

  public TMP_Text scoreMessage;

  private string username = "";

  private int globalScore;

  public void PlayGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  void Update() {
    if (firstMenu.activeInHierarchy) {
      warningLoginText.text = "";
    }
    if (gameObject.name == "ThirdMenu") {
      username = gameObject.GetComponent<APIController>().username;
      globalScore = gameObject.GetComponent<APIController>().globalScore;
      if (username != "") {
        userMessage.text = "Olá, " + username + "!";
        scoreMessage.text = "Sua pontuação global: " + globalScore.ToString() + " Aeons";
      }
    }
  }

  public void QuitGame() {
    Application.Quit();
  }

  public void GoToFirstMenu() {
    firstMenu.SetActive(true);
    secondMenu.SetActive(false);
    thirdMenu.SetActive(false);
  }

  public void GoToSecondMenu() {
    firstMenu.SetActive(false);
    secondMenu.SetActive(true);
    thirdMenu.SetActive(false);
  }

  public void GoToThirdMenu() {
    firstMenu.SetActive(false);
    secondMenu.SetActive(false);
    thirdMenu.SetActive(true);
  }

  public void OpenAeonSpaceSystem() {
    Application.OpenURL("http://unity3d.com/");
  }
}
