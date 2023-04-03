using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameRoutine
{
    void Start();
    void End();
    void Update();
    void LateUpdate();
}
