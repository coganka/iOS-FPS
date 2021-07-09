using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public bool isAuto;
    public float timeBetweenShots = .1f, heatPerShot = 1f;
    public Transform muzzlePos;
}
