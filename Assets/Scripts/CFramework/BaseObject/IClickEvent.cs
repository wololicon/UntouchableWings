using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IClickEvent
{
    void MouseClickEvent(BaseEventData pd);
    void MouseEnterEvent(BaseEventData pd);
    void MouseExitEvent(BaseEventData pd);
}
