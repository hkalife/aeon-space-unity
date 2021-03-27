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

  public bool isGamePaused;

  private Vector2 screenCenter;

  // Start is called before the first frame update
  void Start()
  {
    isGamePaused = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      isGamePaused = !isGamePaused;
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
    if (pauseScreen.activeInHierarchy || controlScreen.activeInHierarchy) {
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

  public void GoBackToGame() {
    pauseScreen.SetActive(false);
    controlScreen.SetActive(false);

    screenCenter.x = Screen.width * .5f;
    screenCenter.y = Screen.height * .5f;

    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = false;
  }

  public void GoToMainMenu() {
    SceneManager.LoadScene(0);
  }
}
