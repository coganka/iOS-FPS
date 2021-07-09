using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    public FixedJoystick moveJoystick;
    public FixedButton jumpButton;
    public FixedTouchField touchField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.instance.runAxis = moveJoystick.Direction;
        PlayerController.instance.jumpAxis = jumpButton.Pressed;
        PlayerController.instance.lookAxis = touchField.TouchDist;
    }
}
