using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damager
{
    string damagerName { get; }

    void DealDamage(Player reciever, Vector2 dir, float power);
}
