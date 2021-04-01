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

  [SerializeField]
  private GameObject deathScreen;

  [SerializeField]
  private GameObject player;

  public bool isGamePaused;

  private Vector2 screenCenter;

  [SerializeField]
  private GameObject currentState;

  [SerializeField]
  private GameObject endScreenPt1;

  [SerializeField]
  private GameObject endScreenPt2;

  // Start is called before the first frame update
  void Start()
  {
    isGamePaused = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (player.GetComponent<PlayerController>().playerHealth <= 0) {
      Time.timeScale = 0f;
      isGamePaused = true;
      deathScreen.SetActive(true);
    }
    if (Input.GetKeyDown(KeyCode.Escape) && !currentState.GetComponent<CurrentState>().gameFinished) {
      isGamePaused = !isGamePaused;
      Time.timeScale = 0f;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
      if (!isGamePaused) {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
      }
      pauseScreen.SetActive(isGamePaused);
      if (controlScreen.activeInHierarchy) {
        controlScreen.SetActive(isGamePaused);
      }
    }
    if (pauseScreen.activeInHierarchy || controlScreen.activeInHierarchy || endScreenPt1.activeInHierarchy || endScreenPt2.activeInHierarchy) {
      Time.timeScale = 0f;
    } else {
      Time.timeScale = 1f;
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

  public void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }


  public void GoBackToGame() {
    pauseScreen.SetActive(false);
    controlScreen.SetActive(false);
    gameObject.GetComponent<PauseScreen>().isGamePaused = false;

    screenCenter.x = Screen.width * .5f;
    screenCenter.y = Screen.height * .5f;

    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = false;
  }

  public void GoToMainMenu() {
    SceneManager.LoadScene(0);
  }
}
