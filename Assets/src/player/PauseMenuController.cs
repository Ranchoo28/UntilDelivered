using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject paths;
    [SerializeField] private Text speedText;
    private CameraController cameraController;

    void Start() {
        cameraController = GetComponentInChildren<CameraController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }

    }

    public void PauseGame()
    {
        GetComponentInChildren<BuildingsPlacement>().CancelBuildingPlacement();
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        speedText.text = "1x";
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        cameraController.enabled = !pauseMenu.activeSelf;
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        cameraController.enabled = true;
    }

    public void BackToMainMenu() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        PlayerInfo.GetInstance().ResetGold();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void RestartGame() {
        PlayerInfo.GetInstance().ResetGold();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ManagePaths(){
        paths.SetActive(!paths.active);
    }
}
