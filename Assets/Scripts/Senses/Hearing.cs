using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : Sense
{
    public float HearingPower; //the higher the number, the further the owner can hear

    [Header("Debug")]
    public bool isAlerted;
    public Vector3 lastHeard;

    private Transform playerTrans;

    private void Awake()
    {
    }

    private void Start()
    {
        NoiseManager.Instance.AddHearer(this);

    }

    protected override void Initialize()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Hear(Vector3 pos)
    {
        lastHeard = pos;
        isAlerted = true;
    }

    public Vector3 GetLastHeard()
    {
        isAlerted = false;
        return lastHeard;
    }
}
