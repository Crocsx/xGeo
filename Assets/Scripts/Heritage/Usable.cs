using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    public event onUsed OnUsed;
    public delegate void onUsed();

    public Sprite icon;
    public bool AUTOMATIC_USE = false;

    [HideInInspector]
    public xPlayer launcher;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void Use(Transform fireTurret)
    {
    }

    public  virtual void Used()
    {
        if (OnUsed != null)
            OnUsed();

        Destroy(gameObject);
    }
}
