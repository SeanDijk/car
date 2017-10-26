﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class AbstractSensorBehaviour
{
    public abstract CarAdvice GiveAdvice(Car car); //TODO Refactor to give advice
    public abstract void Initialize();
}
