using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject popup;
    public Button confirmButton;
    public Button refreshButton;
    public TextMeshProUGUI text;
    private bool success;

    void Start()
    {
        EventManager.StartListening("gamesuccess", onGameSuccess);
        EventManager.StartListening("gamefail", onGameFail);
        confirmButton.onClick.AddListener(HandleConfirm);
        refreshButton.onClick.AddListener(HandleRefresh);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("gamesuccess", onGameSuccess);
        EventManager.StopListening("gamefail", onGameFail);
    }

    public void onGameSuccess()
    {
        this.ShowPopup("Success", true);
    }

    public void onGameFail() {
        this.ShowPopup("Fail", false);
    }

    public void ShowPopup(string msg,bool success)
    {
        this.text.text = msg;
        this.success = success;
        popup.SetActive(true);
    }

    public void HidePopup()
    {
        popup.SetActive(false);
    }

    void HandleConfirm()
    {
        // 处理确认按钮的点击事件
        Debug.Log("Confirmed!");
        HidePopup();

        if (success)
        {
        }
        else
        {
            this.ReloadScene();
            
        }
    }

    void HandleRefresh()
    {
        this.ReloadScene();
    }

    private void ReloadScene()
    {
        // 获取当前活动的场景
        Scene currentScene = SceneManager.GetActiveScene();
        // 重新加载当前场景
        SceneManager.LoadScene(currentScene.name);
    }
}
