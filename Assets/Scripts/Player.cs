using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Player : MonoBehaviour
{
    public float Speed = 1f;
    public float RotationSpeed = 90f;
}
