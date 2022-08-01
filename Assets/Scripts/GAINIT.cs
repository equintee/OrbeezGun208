using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAINIT : MonoBehaviour
{
    private void Awake()
    {
        GameAnalytics.Initialize();
        DontDestroyOnLoad(this.gameObject);
    }
}
