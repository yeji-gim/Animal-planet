using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Internal Clock")]
    [SerializeField]
    GameTimeStamp timestamp;
    public float timeScale = 1.0f; 

    [Header("Day and Night cycle")]
    public Transform sunTransform;

    List<ITimeTracker> listeners = new List<ITimeTracker>();
    public static TimeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        timestamp = new GameTimeStamp(0, GameTimeStamp.Season.Spring,1,6,0);
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(GameTimeStamp timestamp)
    {
        this.timestamp = new GameTimeStamp(timestamp);
    }

    IEnumerator TimeUpdate()
    {
        while(true)
        {
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }   
    }
    public void Tick()
    {
        timestamp.UpdateClock();

        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();
    }
    public void SkipTime(GameTimeStamp timetoSkip)
    {
        int timeToSkipToMinute = GameTimeStamp.TimeStampToMinutes(timetoSkip);
        Debug.Log(timeToSkipToMinute);
        int timeNowInMinutes = GameTimeStamp.TimeStampToMinutes(timestamp);
        Debug.Log("" + timeNowInMinutes);


        int differenceInMinutes = timeToSkipToMinute - timeNowInMinutes;
        Debug.Log(differenceInMinutes + "minutes will be adcvanced");
        if (differenceInMinutes <= 0) return;

        for(int i = 0; i<differenceInMinutes; i++)
        {
            Tick();
        }
    }

    void UpdateSunMovement()
    {
        int timeInMinutes = GameTimeStamp.HoursToMinute(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimeStamp GetGameTimeStamp()
    {
        return new GameTimeStamp(timestamp);
    }

    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }

    

}
