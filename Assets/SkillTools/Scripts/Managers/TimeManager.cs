using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {


    public delegate void WhileCountingDo();
    public delegate void AfterCountingDo();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static IEnumerator TimerCountDown_ThenReset(float TimeTracker, WhileCountingDo MeanWhileDo, AfterCountingDo Reset)
    {
        while (TimeTracker > 0)
        {
            TimeTracker -= Time.deltaTime;
            MeanWhileDo();
            yield return null;
        }
        Reset();
    }
}
