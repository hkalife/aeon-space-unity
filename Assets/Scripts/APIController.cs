using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class APIController : MonoBehaviour
{

  private readonly string baseURL = "https://aeon-space-backend.herokuapp.com";

  private Championship championship;

  private Championship[] listChampionships;
  
  [SerializeField]
  private TMPro.TMP_Dropdown dropdownChampionships;

  [SerializeField]
  private GameObject scoreScreen;

  //[SerializeField]
  //private TMP_Text scoreText;

  [SerializeField]
  private GameObject sendScreen;

  //[SerializeField]
  //private GameObject stateManager;

  private string userId;

  public string username;

  public int globalScore;

  IEnumerator GetCurrentUser(string email) {
    string userURL = baseURL + "/users/getByEmail/" + email;

    // Debug.Log(userURL);

    UnityWebRequest userInfoRequest = UnityWebRequest.Get(userURL);

    yield return userInfoRequest.SendWebRequest();

    if (userInfoRequest.isNetworkError || userInfoRequest.isHttpError) {
      Debug.LogError(userInfoRequest.error);
      yield break;
    }

    JSONNode userInfo = JSON.Parse(userInfoRequest.downloadHandler.text);
    userId = userInfo[0]["id"];
    username = userInfo[0]["username"];
    globalScore = userInfo[0]["global_score"];
    Championship[] listPlayerChampionships = new Championship[userInfo[0]["current_championships"].Count];

    // Debug.Log(userInfo[0]["current_championships"].Count);

    for (int i = 0; i < userInfo[0]["current_championships"].Count; i++) {
      string championshipURL = baseURL + "/championships/" + userInfo[0]["current_championships"][i]["championship_id"];
      UnityWebRequest championshipInfoRequest = UnityWebRequest.Get(championshipURL);
      yield return championshipInfoRequest.SendWebRequest();

      if (championshipInfoRequest.isNetworkError || championshipInfoRequest.isHttpError) {
        Debug.LogError(championshipInfoRequest.error);
        yield break;
      }

      JSONNode championshipInfo = JSON.Parse(championshipInfoRequest.downloadHandler.text);
      // Debug.Log(championshipInfo);

      // Debug.Log(userInfo[0]["current_championships"][i]["championship_id"]);
      // Debug.Log(userInfo[0]["current_championships"][i]["score"]);
      listPlayerChampionships[i] = new Championship(
        userInfo[0]["current_championships"][i]["championship_id"],
        userInfo[0]["current_championships"][i]["score"],
        championshipInfo["championship_name"],
        championshipInfo["state"]
      );

      listChampionships = listPlayerChampionships;

      /* Debug.Log("ID: " + listPlayerChampionships[i].Id);
      Debug.Log("SCORE: " + listPlayerChampionships[i].Score);
      Debug.Log("NAME: " + listPlayerChampionships[i].ChampionshipName);
      Debug.Log("STATE: " + listPlayerChampionships[i].State); */
    }

    if (dropdownChampionships != null) {
      dropdownChampionships.ClearOptions();
    }
    List<TMPro.TMP_Dropdown.OptionData> championshipItems = new List<TMPro.TMP_Dropdown.OptionData>();
    foreach (var championship in listPlayerChampionships) {
      var champOption = new TMPro.TMP_Dropdown.OptionData(championship.ChampionshipName);
      championshipItems.Add(champOption);
    }

    if (dropdownChampionships != null) {
      dropdownChampionships.AddOptions(championshipItems);
    }

    //Debug.Log(listPlayerChampionships);

    // Debug.Log(userInfo[0]["current_championships"][1]["championship_id"]);
    
    // Debug.Log(userInfo);
  }

  void Start() {
    StartCoroutine(GetCurrentUser(CurrentState.usermail));
    //scoreScreen.SetActive(false);
  }

  // Update is called once per frame
  void Update()
  {
    /* if (Input.GetKeyDown(KeyCode.Escape)) {
      StartCoroutine(GetCurrentUser("henriquekalife@gmail.com"));
    } */
    // Debug.Log(dropdownChampionships.value);
    // Debug.Log(dropdownChampionships.options[dropdownChampionships.value].text);

    /*if (stateManager.GetComponent<CurrentState>().gameFinished) {
      GoToScreen(1);
      scoreText.text = stateManager.GetComponent<CurrentState>().score.ToString();
    }*/
  }

  public void HandleInputData(int val) {
    Debug.Log(val);
  }

  public void GoToScreen(int screen) {
    if (screen == 1) {
      scoreScreen.SetActive(true);
      sendScreen.SetActive(false);
    } else if (screen == 2) {
      scoreScreen.SetActive(false);
      sendScreen.SetActive(true);
    }
  }

  public void SendGameInfo() {
    StartCoroutine(UpdateScoreInChampionship());
  }

  IEnumerator UpdateScoreInChampionship() {    
    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

    string championshipId = "";
    foreach (var championship in listChampionships) {
      if (championship.ChampionshipName == dropdownChampionships.options[dropdownChampionships.value].text) {
        championshipId = championship.Id;
      }
    }

    GameObject player = GameObject.Find("Player Ship");
    string url = "https://aeon-space-backend.herokuapp.com/championships/updatechampionshipscore/"+ championshipId + "&" + userId + "&" + player.GetComponent<PlayerController>().playerScore.ToString();

    UnityWebRequest www = UnityWebRequest.Post(url, formData);
    yield return www.SendWebRequest();

    if (www.isNetworkError || www.isHttpError) {
      Debug.LogError(www.error);
      yield break;
    }

    StartCoroutine(UpdateScoreInUser());
  }

  IEnumerator UpdateScoreInUser() {
    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

    string championshipId = "";
    foreach (var championship in listChampionships) {
      if (championship.ChampionshipName == dropdownChampionships.options[dropdownChampionships.value].text) {
        championshipId = championship.Id;
      }
    }

    GameObject player = GameObject.Find("Player Ship");
    GameObject state = GameObject.Find("StateManager");
    string url = "https://aeon-space-backend.herokuapp.com/users/updateplayerscore/" + championshipId + "&" + userId
    + "&" + player.GetComponent<PlayerController>().playerScore.ToString()
    + "&" + state.GetComponent<CurrentState>().currentObjective
    + "&" + state.GetComponent<CurrentState>().currentScenario;

    UnityWebRequest www = UnityWebRequest.Post(url, formData);
    yield return www.SendWebRequest();

    if (www.isNetworkError || www.isHttpError) {
      Debug.LogError(www.error);
      yield break;
    }

    MainMenu.isNextGame = true;
    SceneManager.LoadScene(0);
  }
}
