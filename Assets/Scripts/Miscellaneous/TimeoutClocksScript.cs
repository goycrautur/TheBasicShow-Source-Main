using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeoutClocksScript : MonoBehaviour
{
    public TMP_Text TimeTXT;
    public void Update()
    {
        int hours = Mathf.FloorToInt(Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk / 3600);
        int num = Mathf.FloorToInt(Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk / 60f);
        int num2 = Mathf.FloorToInt(Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk % 60f);
        
        if (Singleton<TimeOutManagerFUCKYEA>.Instance.countItDown)
        {
		TimeTXT.text = string.Format("{0:00}:{1:00}:{2:00}", hours,num, num2);
        }
        if (Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk <= 60f)
        {
            TimeTXT.color = Color.red;
        }
        if (!Singleton<TimeOutManagerFUCKYEA>.Instance.countItDown && Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk <= 0f)
        {
		TimeTXT.text = "ur cooked miaw:3";
        }
    }
}
