using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WMIForUnity;

public class StopListenerOnDestory : MonoBehaviour
{
    private void OnDestroy() => ListenerProgramm.StopListenningPorcessChange();
    private void OnBecameInvisible() => ListenerProgramm.StopListenningPorcessChange();
}
