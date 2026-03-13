using UnityEngine;
using UnityEngine.AddressableAssets;

public class RunMonster : MonsterBase
{
    protected override void Update()
    {
        base.Update();
        FlipSprite();
        SetMove(1f);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveToPlayer();
    }
    protected override void Die()
    {
        base.Die();
        ObjectPoolManager.Instance.Release(
            Addressables.LoadAssetAsync<GameObject>("RunMonster").WaitForCompletion(), 
            gameObject);
    }
}
