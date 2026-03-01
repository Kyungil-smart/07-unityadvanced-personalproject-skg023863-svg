using UnityEngine;

public class RunMonster : MonsterBase
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
