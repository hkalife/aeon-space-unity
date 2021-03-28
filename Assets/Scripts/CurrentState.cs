using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentState : MonoBehaviour
{

  public int score;

  public bool gameFinished;

  public int remainingStations;

  public int remainingAssets;

  public GameObject scoreScreen;

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
  private TMP_Text remainingNumber;

  [SerializeField]
  private TMP_Text titleRemainingNumber;

  [SerializeField]
  private TMP_Text messageCrate;

  [SerializeField]
  private GameObject station;

  [SerializeField]
  private GameObject assetItem;

  [SerializeField]
  private GameObject collectable;

  [SerializeField]
  private GameObject endScreenPt1;

  [SerializeField]
  private GameObject endScreenPt2;

  private int minutes = 2;

  private int seconds = 59;

  void Start() {
    gameFinished = false;
    DefineScenario();
    DefineObjective();
  }

  void Update() {
    if (currentObjective == "Base") {
      titleRemainingNumber.text = "Restando";
      remainingNumber.text = remainingStations.ToString();
    }
    if (currentObjective == "Assets") {
      titleRemainingNumber.text = "Restando";
      remainingNumber.text = remainingAssets.ToString();
    }
    if (endScreenPt1.activeInHierarchy || endScreenPt2.activeInHierarchy) {
      gameFinished = true;
    }
  }

  void DefineScenario() {
    currentScenario = scenarios[Random.Range(0, scenarios.Length)];
    MountScene(currentScenario);
  }

  public void CallClean() {
    StartCoroutine(CleanAssetTextAfterSomeTime());
  }

  public IEnumerator CleanAssetTextAfterSomeTime() {
    yield return new WaitForSeconds(3f);
    messageCrate.text = "";
  }

  void MountScene(string scenario) {
    CreateCollectables();
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

  void CreateCollectables() {
    for (int i = 0; i < 30 ; i++) {
      Vector3 newCollectablePosition = new Vector3(Random.Range(870.0f, 4300.0f), Random.Range(200.0f, 400.0f), Random.Range(745.0f, 4150.0f));
      GameObject newCollectable = Instantiate(collectable, newCollectablePosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
      newCollectable.SetActive(true);
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
    } else if (currentObjective == "Assets") {
      CreateAssetObjective();
    } else if (currentObjective == "Base") {
      CreateBaseObjective();
    }
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
    remainingAssets = 10;
    for (int i = 0; i <= 9; i++) {
      Quaternion stationRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      Vector3 stationPosition = new Vector3(Random.Range(870.0f, 4300.0f), Random.Range(400.0f, 500.0f), Random.Range(745.0f, 4150.0f));
      GameObject newAsset = Instantiate(assetItem, stationPosition, stationRotation);
    }
    GenerateEnemies(50);
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

    GenerateEnemies(50);
  }

  void GenerateEnemies(int amountOfEnemies) {
    for (int i = 0; i < amountOfEnemies ; i++) {
      Vector3 newEnemyPosition = new Vector3(Random.Range(680.0f, 4700.0f), Random.Range(250.0f, 600.0f), Random.Range(500.0f, 4200.0f));
      Quaternion newEnemyRotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), Random.Range(-30.0f, 30.0f));
      GameObject newEnemy = Instantiate(enemy, newEnemyPosition, newEnemyRotation);
      newEnemy.SetActive(true);
    }
  }

  void CreateDeathmatchObjective() {
    clockText.text = "";
    titleRemainingNumber.text = "";
    remainingNumber.text = "";
    GenerateEnemies(75);
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
