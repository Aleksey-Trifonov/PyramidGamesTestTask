using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class AimButton : Button
{
    private bool isPressed = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        isPressed = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        isPressed = false;
        GameplayManager.Instance.LaunchBall();
    }

    private void Update()
    {
        if (isPressed)
        {
            GameplayManager.Instance.ChargeBallVelocity();
        }
    }
}
