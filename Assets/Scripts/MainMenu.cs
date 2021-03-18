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


  public void PlayGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  void Update() {
    if (firstMenu.activeInHierarchy) {
      warningLoginText.text = "";
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
