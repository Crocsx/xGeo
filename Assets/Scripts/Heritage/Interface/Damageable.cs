using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
    void GetDamage(Vector2 dir, float power, PlayerManager dealer);
}