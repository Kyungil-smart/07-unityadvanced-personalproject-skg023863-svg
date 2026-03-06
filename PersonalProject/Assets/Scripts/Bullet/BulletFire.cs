using UnityEngine;

public class BulletFire : MonoBehaviour
{
    private float _bulletSpeed; // 총알 속도
    private float _damage; // 총알 공격력
    private Vector2 _dir; // 총알 방향
    private float _angle;  // 총알 각도 
    private Vector2 _startPosition;
    private float _distance;
    
    // 방향(dir)과 damage, bulletSpeed, bulletDistance를 player한테 받아옴
    public void Init(Vector2 dir, float damage, float bulletSpeed, float bulletDistance)
    {
        _damage = damage;
        _bulletSpeed = bulletSpeed;
        _dir = dir.normalized;
        _distance = bulletDistance;
        
        // 총알이 총구가 바라보는 각도에 따라서 총알도 그에 맞게 회전
        transform.right = _dir;
        
        _startPosition = transform.position;
    }

    void Update()
    {
        FireBullet();
        BulletDitance();
    }

    // 총 발사 함수
    void FireBullet()
    {
        transform.position += (Vector3)(_dir * _bulletSpeed * Time.deltaTime);
    }

    // 총알이 _distance만큼 날아가면 총알 제거
    void BulletDitance()
    {
        float distance = Vector2.Distance(_startPosition, transform.position);

        if (distance > _distance)
        {
            Destroy(gameObject);
        }
    }
    
    // Collider에 부딪히면 사라짐
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        
        if (other.TryGetComponent<IDamagable>(out IDamagable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }
}
