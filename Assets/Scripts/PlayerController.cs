using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
  public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
  private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
  private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;

  public float lookRateSpeed = 90f;
  private Vector2 lookInput, screenCenter, mouseDistance;

  private float rollInput;
  public float rollSpeed = 90f, rollAcceleration = 3.5f;

  private bool isTurboAvailable;

  [SerializeField]
  private GameObject leftLaser;

  [SerializeField]
  private GameObject rightLaser;

  [SerializeField]
  private GameObject leftLaserPosition;

  [SerializeField]
  private GameObject rightLaserPosition;

  [SerializeField]
  private GameObject missile;

  [SerializeField]
  private GameObject missilePosition;

  public int playerHealth;

  public HealthBar healthBar;

  public int playerScore;

  public int missileQuantity;
  
  [SerializeField]
  private TextMeshProUGUI scoreText;

  [SerializeField]
  private GameObject stateManager;

  [SerializeField]
  private GameObject explosionEffect;

  [SerializeField]
  private GameObject canvas;

  [SerializeField]
  private TMP_Text missileQuantityText;

  [SerializeField]
  private TMP_Text textTurboAvailable;

  // Start is called before the first frame update
  void Start()
  {
    screenCenter.x = Screen.width * .5f;
    screenCenter.y = Screen.height * .5f;

    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = false;

    playerHealth = 100;
    missileQuantity = 2;
    missileQuantityText.text = "x" + missileQuantity.ToString();
    healthBar.SetMaxHealth(100);
    playerScore = 0;
		stateManager.GetComponent<CurrentState>().SetScore(playerScore);
    scoreText.text = playerScore.ToString();
    isTurboAvailable = true;
    textTurboAvailable.text = "Turbo disponível";
  }

  void Update() {
    CheckLife();
    //call laser control
    Debug.Log(!canvas.GetComponent<PauseScreen>().isGamePaused && !stateManager.GetComponent<CurrentState>().gameFinished);
    if (Input.GetMouseButtonDown(0) && !canvas.GetComponent<PauseScreen>().isGamePaused && !stateManager.GetComponent<CurrentState>().gameFinished) {
      GameObject newLeftLaser = Instantiate(leftLaser, leftLaserPosition.transform.position, leftLaserPosition.transform.rotation);
      GameObject newRightLaser = Instantiate(rightLaser, rightLaserPosition.transform.position, rightLaserPosition.transform.rotation);
      newLeftLaser.SetActive(true);
      newRightLaser.SetActive(true);
    } else if (Input.GetMouseButtonDown(1) && (!canvas.GetComponent<PauseScreen>().isGamePaused || !stateManager.GetComponent<CurrentState>().gameFinished) && missileQuantity > 0) {
      GameObject newMissile = Instantiate(missile, missilePosition.transform.position, missilePosition.transform.rotation);
      newMissile.SetActive(true);
      missileQuantity--;
      missileQuantityText.text = "x" + missileQuantity.ToString();
    }
    if (Input.GetKeyDown(KeyCode.LeftShift) && isTurboAvailable) {
      StartCoroutine(ActivateTurbo());
    }
  }

  IEnumerator ActivateTurbo()
  {
    isTurboAvailable = false;
    textTurboAvailable.text = "";
    forwardSpeed = 300f;
    yield return new WaitForSeconds(2f);
    forwardSpeed = 100f;
    yield return new WaitForSeconds(10f);
    isTurboAvailable = true;
    textTurboAvailable.text = "Turbo disponível";
  }

  void OnCollisionEnter(Collision col) {
    if (col.gameObject.tag == "Terrain" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "Station") {
      playerHealth -= 100;
      healthBar.SetHealth(playerHealth);
    }
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    lookInput.x = Input.mousePosition.x;
    lookInput.y = Input.mousePosition.y;

    mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
    mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

    mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

    rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

    transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

    activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
    activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
    activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

    transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
    transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
  }

  public void DamageForPlayer() {
    playerHealth = playerHealth - 5;
    healthBar.SetHealth(playerHealth);
  }

  public void AddScore() {
    playerScore += 10;
		stateManager.GetComponent<CurrentState>().SetScore(playerScore);
    scoreText.text = playerScore.ToString();
  }

  public void AddStationScore() {
    playerScore += 100;
		stateManager.GetComponent<CurrentState>().SetScore(playerScore);
    scoreText.text = playerScore.ToString();
  }

  public void CheckLife() {
    if (playerHealth <= 0) {
      GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
      explosion.transform.localScale = new Vector3(20, 20, 20);
      gameObject.SetActive(false);
      Time.timeScale = 0f;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

}
