using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentState : MonoBehaviour
{

  [SerializeField]
  private int score;

  [SerializeField]
  private string currentScenario;

  private string[] scenarios = {"Space", "Desert", "Ocean"};

  [SerializeField]
  private Material spaceSkyboxMaterial;
  
  [SerializeField]
  private Material desertSkyboxMaterial;
  
  [SerializeField]
  private Material oceanSkyboxMaterial;

  void Start() {
    DefineScenario();
    DefineObjective();
  }

  void DefineScenario() {
    currentScenario = scenarios[Random.Range(0, scenarios.Length)];
    MountScene(currentScenario);
  }

  void MountScene(string scenario) {
    if (scenario == "Space") {
      RenderSettings.skybox = spaceSkyboxMaterial;
    } else if (scenario == "Desert") {
      RenderSettings.skybox = desertSkyboxMaterial;
    } else if (scenario == "Ocean") {
      RenderSettings.skybox = oceanSkyboxMaterial;
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
