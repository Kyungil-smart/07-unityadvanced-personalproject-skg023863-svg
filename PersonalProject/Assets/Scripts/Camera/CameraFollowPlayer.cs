using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playertranform;

    void LateUpdate()
    {
        // 카메라가 위에서 바라보고 있기 때문에 z는 transform으로
        transform.position = new Vector3(
            _playertranform.position.x,
            _playertranform.position.y,
            transform.position.z);
    }
}
