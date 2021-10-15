using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class ShowWalkCursor : MonoBehaviour
{
    private void OnEnable()
    {
        AC.KickStarter.cursorManager.allowWalkCursor = true;
    }
}

