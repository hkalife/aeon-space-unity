using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentState : MonoBehaviour
{

  public int score;

  public bool gameFinished;

  public int remainingStations;

  [SerializeField]
  private string currentScenario;

  private string[] scenarios = {"Space", "Desert", "Ice"};

  [SerializeField]
  private string currentObjective = "Deathmatch";

  private string[] objectives = {"Deathmatch", "Assets", "Base"};

  [SerializeField]
  private Material spaceSkyboxMaterial;
  
  [SerializeField]
  private Material desertSkyboxMaterial;
  
  [SerializeField]
  private Material iceSkyboxMaterial;

  [SerializeField]
  private GameObject spaceScenario;
  
  [SerializeField]
  private GameObject desertScenario;
  
  [SerializeField]
  private GameObject iceScenario;

  [SerializeField]
  private GameObject enemy;

  [SerializeField]
  private TMP_Text clockText;

  [SerializeField]
  private TMP_Text objectiveTextTitle;

  [SerializeField]
  private TMP_Text objectiveTextSubTitle;

  [SerializeField]
  private TMP_Text scoreText;

  [SerializeField]
  private GameObject scoreScreen;

  [SerializeField]
  private GameObject station;

  private int minutes = 2;

  private int seconds = 59;

  void Start() {
    gameFinished = false;
    DefineScenario();
    DefineObjective();
  }

  void Update() {
    /*if (currentObjective == "Base" && remainingStations == 0) {
      gameFinished = true;
      // scoreScreen.SetActive(true);
    }*/
  }

  void DefineScenario() {
    currentScenario = scenarios[Random.Range(0, scenarios.Length)];
    MountScene(currentScenario);
  }

  void MountScene(string scenario) {
    if (scenario == "Space") {
      RenderSettings.skybox = spaceSkyboxMaterial;
      spaceScenario.SetActive(true);
      desertScenario.SetActive(false);
      iceScenario.SetActive(false);
    } else if (scenario == "Desert") {
      RenderSettings.skybox = desertSkyboxMaterial;
      spaceScenario.SetActive(false);
      desertScenario.SetActive(true);
      iceScenario.SetActive(false);
    } else if (scenario == "Ice") {
      RenderSettings.skybox = iceSkyboxMaterial;
      spaceScenario.SetActive(false);
      desertScenario.SetActive(false);
      iceScenario.SetActive(true);
    }
  }

  void DefineObjective() {
    currentObjective = objectives[Random.Range(0, objectives.Length)];
    StartCoroutine(DefineTextForScreen(currentObjective));
    MountRandomObjectives(currentObjective);
  }

  IEnumerator DefineTextForScreen(string objective) {
    if (objective == "Deathmatch") {
      objectiveTextTitle.text = "Modo: Batalha";
      objectiveTextSubTitle.text = "Elimine o máximo de naves inimigas até o tempo acabar";
    } else if (objective == "Assets") {
      objectiveTextTitle.text = "Modo: Recursos";
      objectiveTextSubTitle.text = "Colete todos os recursos sem morrer";
    } else if (objective == "Base") {
      objectiveTextTitle.text = "Modo: Base Inimiga";
      objectiveTextSubTitle.text = "Elimine as bases inimigas";
    }

    yield return new WaitForSeconds(3f);

    objectiveTextTitle.text = "";
    objectiveTextSubTitle.text = "";
  }

  void MountRandomObjectives(string currentObjective) {
    if (currentObjective == "Deathmatch") {
      CreateDeathmatchObjective();
    } /*else if (currentObjective == "Assets") {
      CreateAssetObjective();
    } */else if (currentObjective == "Base") {
      CreateBaseObjective();
    }
    //CreateDeathmatchObjective();
    //CreateBaseObjective();
    //CreateAssetObjective();
  }

  public void SetScore(int newScore) {
    score = newScore;
    scoreText.text = newScore.ToString();
  }

  public int ReturnScore() {
    return score;
  }

  void CreateAssetObjective() {
    clockText.text = "";
  }

  void CreateBaseObjective() {
    clockText.text = "";

    remainingStations = 3;
    for (int i = 0; i <= 2; i++) {
      Quaternion stationRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

      Vector3 stationPosition = new Vector3(0.0f, 0.0f, 0.0f);
      if (i == 0) {
        stationPosition = new Vector3(Random.Range(1200.0f, 3800.0f), 340.0f, 4400.0f);
      } else if (i == 1) {
        stationPosition = new Vector3(4200.0f, 340.0f, Random.Range(900.0f, 2600.0f));
      } else if (i == 2) {
        stationPosition = new Vector3(800.0f, 340.0f, Random.Range(900.0f, 2600.0f));
      }

      GameObject newStation = Instantiate(station, stationPosition, stationRotation);
      newStation.SetActive(true);
    }

    //GenerateEnemies(25);
  }

  void GenerateEnemies(int amountOfEnemies) {
    for (int i = 0; i < amountOfEnemies ; i++) {
      Vector3 newEnemyPosition = new Vector3(Random.Range(-680.0f, 4700.0f), Random.Range(250.0f, 600.0f), Random.Range(124.0f, 4200.0f));
      Quaternion newEnemyRotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), Random.Range(-30.0f, 30.0f));
      GameObject newEnemy = Instantiate(enemy, newEnemyPosition, newEnemyRotation);
      newEnemy.SetActive(true);
    }
  }

  void CreateDeathmatchObjective() {
    clockText.text = "";
    GenerateEnemies(50);
    StartCoroutine(Countdown());
  }

  IEnumerator Countdown()
  {
    while (seconds > 0 || minutes > 0) {
      if (seconds > 0) {
        seconds--;
      } else {
        seconds = 59;
        minutes--;
      }
      string secondsToShow = seconds.ToString();
      if (seconds < 10) {
        secondsToShow = "0" + seconds.ToString();
      }
      clockText.text = "0" + minutes.ToString() + ":" + secondsToShow;
      yield return new WaitForSeconds(1f);
    }
    gameFinished = true;
    clockText.text = "";
    scoreScreen.SetActive(true);
  }
}
