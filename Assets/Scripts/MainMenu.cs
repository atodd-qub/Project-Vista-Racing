using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button btnPlay;
    public Button btnHowToPlay;
    public Button btnHowToPlayClose;

    public GameObject objHowToPlay;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonPress(string buttonValue)
    {
        switch (buttonValue)
        {
            case "Play":
                {
                    // load scene...too scared to rename the scene at this point so its still called samplescene lol
                    SceneManager.LoadScene("SampleScene");
                    break;
                }
            case "HowToPlay":
                {
                    // activate how to play game object
                    objHowToPlay.SetActive(true);
                    break;
                }
            case "HowToPlayClose":
                {
                    // close
                    objHowToPlay.SetActive(false);
                    break;
                }
            case "Retry":
                {
                    // reload current scene
                    
                    break;
                }
            case "MainMenu":
                {
                    // load mainmenu
                    SceneManager.LoadScene("MainMenu");
                    break;
                }
            default:
                {
                    print("Button error");
                    break;
                }
        }
    }
}
