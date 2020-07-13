using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startBtn;

    private void Start()
    {
        SoundManager.instance.PlayBgm(SoundKey.MAIN);

        startBtn.onClick.AddListener(OnClickStartBtn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene("Play");
    }

    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("Play");
    }
}
