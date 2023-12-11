using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // variables
    private float raceTime;
    private float lapCount = 0;
    public float countdown = 4.0f;

    public Text raceTimeUI;
    public Text lapCountUI;
    public Text raceCountDownUI;
    public GameObject lineTrigger;
    public GameObject playerCar;

    private bool raceStarted;
    private bool raceFinished;

    public AudioSource cameraAudioSrc;
    public AudioClip sfx3;
    public AudioClip sfx2;
    public AudioClip sfx1;
    public AudioClip sfxGo;
    public AudioClip sfxLap2;
    public AudioClip sfxLap3;
    public AudioClip sfxFinish;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerCar == null)
        {
            playerCar = GameObject.FindWithTag("Player");
        }

        raceFinished = false; 

        // disable car for race countdown
        ToggleCar(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!raceStarted)
        {
            RaceCountdown();
        }

        if (raceStarted && !raceFinished)
        {
            if (raceTime >= 2f && raceTime <= 4f)
            {
                raceCountDownUI.text = "";
            }
            
            // update race time
            raceTime += Time.deltaTime;
            raceTimeUI.text = raceTime.ToString("F3");
        }

        if (raceFinished)
        {
            raceCountDownUI.text = "FINISHED!";
        }
    }

    private void RaceCountdown()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 3f && countdown >2.98f)
        {
            raceCountDownUI.text = "3";

            // play sfx
            cameraAudioSrc.PlayOneShot(sfx3, 0.8f);
        }
        if (countdown <= 2f && countdown > 1.98f)
        {
            raceCountDownUI.text = "2";

            // play sfx
            cameraAudioSrc.PlayOneShot(sfx2, 0.8f);
        }
        if (countdown <= 1f && countdown > 0.98f)
        {
            raceCountDownUI.text = "1";

            // play sfx
            cameraAudioSrc.PlayOneShot(sfx1, 0.8f);
        }
        if (countdown <= 0f)
        {
            Debug.Log("go");
            raceCountDownUI.text = "GO!";

            // play sfx
            cameraAudioSrc.PlayOneShot(sfxGo, 0.8f);
            // start race
            ToggleCar(true);
            raceStarted = true;
            
        }

    }

    private void ToggleCar(bool toggle)
    {
        (playerCar.GetComponent("ControllerCar") as MonoBehaviour).enabled = toggle;
        Debug.Log("toggle: " + toggle.ToString());
    }

    // check colisions 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lapCount += 1;
        }

        if (lapCount == 2)
        {
            // play sfx
            cameraAudioSrc.PlayOneShot(sfxLap2, 2f);
        }
        else if (lapCount == 3)
        {
            // play sfx
            cameraAudioSrc.PlayOneShot(sfxLap3, 1.6f);
        }

        // update lap count text
        if (lapCount < 4)
        {
            lapCountUI.text = "Lap " + lapCount.ToString() + "/3";
        }
        else
        {
            // Race must be finished
            raceFinished = true;
            ToggleCar(false);
            // Todo : stop car audio

            cameraAudioSrc.PlayOneShot(sfxFinish, 1.5f);
        }
    }

}
