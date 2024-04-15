using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : Character
{
    protected override void Move() {

    }

    protected override void CheckForCharacters() {

    }

    public override void Die()
    {
        GameManager.Instance.GameOver();
    }
}
