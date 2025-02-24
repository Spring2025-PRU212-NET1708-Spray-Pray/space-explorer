using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    private Transform enemy;
    public Vector3 offset = new Vector3(0, 1f, 0); // Điều chỉnh vị trí HP Bar

    public void SetTarget(Transform target)
    {
        enemy = target;
        transform.SetParent(target); // Đảm bảo HP Bar đi theo Enemy luôn
        transform.localPosition = offset; // Giữ khoảng cách cố định
    }

    void LateUpdate()
    {
        if (enemy != null)
        {
            transform.position = enemy.position + offset;
        }
    }
}
