using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // variables
    private float raceTime;

    public Text raceTimeUI;
    public GameObject checkeredLine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update race time
        raceTime += Time.deltaTime;
        raceTimeUI.text = raceTime.ToString("F3");
    }
}
