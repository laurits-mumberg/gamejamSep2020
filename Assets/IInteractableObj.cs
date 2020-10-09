using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObj
{
    void onClick();

    void onRelease();

    void onHover();

    void onHoverStop();
}
