using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentState : MonoBehaviour
{

  [SerializeField]
  private int score;

  [SerializeField]
  private string currentScenario;

  private string[] scenarios = {"Space", "Desert", "Ice"};

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

  void Start() {
    DefineScenario();
    DefineObjective();
  }

  void DefineScenario() {
    // Debug.Log(scenarios.Length);
    currentScenario = scenarios[Random.Range(0, scenarios.Length)];
    Debug.Log(currentScenario);
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

  }

  public void SetScore(int newScore) {
    score = newScore;
  }

  public int ReturnScore() {
    return score;
  }

}
