using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour {
  //Firebase variables
  [Header("Firebase")]
  public DependencyStatus dependencyStatus;
  public FirebaseAuth auth;    
  public FirebaseUser User;

  //Login variables
  [Header("Login")]
  public TMP_InputField emailLoginField;
  public TMP_InputField passwordLoginField;
  public TMP_Text warningLoginText;
  // public TMP_Text confirmLoginText;

  void Awake() {
    //checando dependências
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    {
      dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
        //iniciar firebase
        InitializeFirebase();
      }
      else {
        Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });
  }

  private void InitializeFirebase() {
    Debug.Log("Setting up Firebase Auth");
    //Set the authentication instance object
    auth = FirebaseAuth.DefaultInstance;
  }

  //Function for the login button
  public void LoginButton() {
    //Call the login coroutine passing the email and password
    StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
  }

  private IEnumerator Login(string _email, string _password) {
    //Call the Firebase auth signin function passing the email and password
    var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
    //Wait until the task completes
    yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

    if (LoginTask.Exception != null) {
      //If there are errors handle them
      Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
      FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
      AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

      string message = "Erro de login, tente novamente.";
      switch (errorCode) {
        case AuthError.MissingEmail:
          message = "Verifique o E-mail";
          break;
        case AuthError.MissingPassword:
          message = "Verifique a senha";
          break;
        case AuthError.WrongPassword:
          message = "Senha errada";
          break;
        case AuthError.InvalidEmail:
          message = "Email fora de formato";
          break;
        case AuthError.UserNotFound:
          message = "Conta inexistente";
          break;
      }
      warningLoginText.text = message;
    }
    else {
      //User is now logged in
      //Now get the result
      User = LoginTask.Result;
      Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);

      MainMenu menuController = gameObject.GetComponent<MainMenu>();
      menuController.GoToThirdMenu();

      warningLoginText.text = "";
    }
  }
}
