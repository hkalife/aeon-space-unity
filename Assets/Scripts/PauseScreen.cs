using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{

  [SerializeField]
  private GameObject pauseScreen;

  [SerializeField]
  private GameObject controlScreen;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      pauseScreen.SetActive(true);
    }
  }

  public void GoToControl() {
    pauseScreen.SetActive(false);
    controlScreen.SetActive(true);
  }

  public void GoBackToPauseMenu() {
    pauseScreen.SetActive(true);
    controlScreen.SetActive(false);
  }

  public void GoBackToGame() {
    pauseScreen.SetActive(false);
    controlScreen.SetActive(false);
  }

  public void GoToMainMenu() {
    SceneManager.LoadScene(0);
  }
}
