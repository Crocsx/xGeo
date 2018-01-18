using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    public event onUsed OnUsed;
    public delegate void onUsed();

    [HideInInspector]
    public Player launcher;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void Use(Vector3 pos)
    {
    }

    protected virtual void Used()
    {
        if (OnUsed != null)
            OnUsed();
    }
}
