using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    GameObject settingsPanel;
    Button pauseBtn,continueBtn,saveBtn,LoadBtn,quitBtn,closeBtn;
    private void Awake() 
    {
        DontDestroyOnLoad(this);
        settingsPanel = this.transform.GetChild(0).gameObject;
        pauseBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = settingsPanel.transform.GetChild(0).GetComponent<Button>();
        saveBtn = settingsPanel.transform.GetChild(1).GetComponent<Button>();
        LoadBtn = settingsPanel.transform.GetChild(2).GetComponent<Button>();
        quitBtn = settingsPanel.transform.GetChild(3).GetComponent<Button>();

        pauseBtn.onClick.AddListener(PauseGame);
        continueBtn.onClick.AddListener(ContinueGame);
        saveBtn.onClick.AddListener(SaveGame);
        LoadBtn.onClick.AddListener(LoadGame);
        quitBtn.onClick.AddListener(QuitGame);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
    }
    void SaveGame()
    {
        SaveManager.Instance.SavePlayerData();
    }
    void LoadGame()
    {
        SaveManager.Instance.LoadPlayerData();
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }    
}
