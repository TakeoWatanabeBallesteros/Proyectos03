using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KidType
{
    public KidGender gender { get => _gender;}
    public KidDiff diff { get => _diff; }

    [SerializeField] private KidGender _gender;
    [SerializeField] private KidDiff _diff;
}

public enum KidGender {
    Male,
    Female
}
public enum KidDiff {
    Diff1,
    Diff2
}
