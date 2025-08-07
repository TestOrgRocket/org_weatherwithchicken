using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLock : MonoBehaviour
{
    public bool lockScreen;
    public ScreenOrientation wantedLockedOrientation;

    public void Start()
    {
        if (lockScreen)
        {
            Screen.orientation = wantedLockedOrientation;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }
    }
}
