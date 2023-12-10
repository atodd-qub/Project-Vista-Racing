using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // variables
    private float raceTime;
    private float lapCount = 0;

    public Text raceTimeUI;
    public Text lapCountUI;
    public GameObject lineTrigger;
    public GameObject playerCar;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerCar == null)
        {
            playerCar = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update race time
        raceTime += Time.deltaTime;
        raceTimeUI.text = raceTime.ToString("F3");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lapCount += 1;
        }

        // update lap count text
        lapCountUI.text = "Lap " + lapCount.ToString();
    }
}
