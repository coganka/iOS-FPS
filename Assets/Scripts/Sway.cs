using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public static Sway instance;

    public float intensity;
    public float smooth;

    private Quaternion originRotation;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originRotation = transform.localRotation;    
    }

    void Update()
    {
        //UpdateSway();
    }
    
    public void UpdateSway(float xMouse, float yMouse)
    {
        

        // calculate rotation
        Quaternion xAdjustment = Quaternion.AngleAxis(-intensity * xMouse, Vector3.up);
        Quaternion yAdjustment = Quaternion.AngleAxis(intensity * yMouse, Vector3.right);
        Quaternion targetRotation = originRotation * xAdjustment * yAdjustment;

        // rotate towards target
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
