using UnityEngine;

public class BulletFire : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed= 13f; // 총알 속도
    [SerializeField] private float _bulletLifeTime = 1f; // 총알 생명주기
    private Vector2 _dir; // 총알 방향
    private float angle;  // 총알 각도 
    
    // 방향(dir)은 player한테 받아옴
    public void Init(Vector2 dir)
    {
        _dir = dir.normalized;
        
        // 총알이 총구가 바라보는 각도에 따라서 총알도 그에 맞게 회전
        transform.right = _dir;
        // angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angle);
        
        Destroy(gameObject, _bulletLifeTime);
    }

    void Update()
    {
        FireBullet();
    }

    void FireBullet()
    {
        transform.position += (Vector3)(_dir * _bulletSpeed * Time.deltaTime);
    }
    
    // Collider에 부딪히면 사라짐
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
