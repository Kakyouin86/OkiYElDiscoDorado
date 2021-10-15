using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class HideWalkCursor : MonoBehaviour
{
    private void OnDisable()
    {
        AC.KickStarter.cursorManager.allowWalkCursor = false;
    }

    private void OnEnable()
    {
        AC.KickStarter.cursorManager.allowWalkCursor = false;
    }
}