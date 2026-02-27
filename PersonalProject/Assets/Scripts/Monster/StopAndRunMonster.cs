using UnityEngine;

public class StopAndRunMonster : MonsterBase
{

    private void Update()
    {
        FlipSprite();
        SetMove(1f);
    }
    private void FixedUpdate()
    {
        MoveToPlayer();
    }
}
