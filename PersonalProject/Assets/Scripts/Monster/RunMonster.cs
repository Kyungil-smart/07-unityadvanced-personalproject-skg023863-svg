using UnityEngine;

public class RunMonster : MonsterBase
{
    private void Update()
    {
        base.Update();
        FlipSprite();
        SetMove(1f);
    }
    private void FixedUpdate()
    {
        base.FixedUpdate();
        MoveToPlayer();
    }
}
