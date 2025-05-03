using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallWithGravity : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // x, z方向をランダムに設定
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        direction = new Vector3(x, 0, z).normalized;

        // 速度の初期値
        rb.linearVelocity = direction * speed;
    }
    
    void FixedUpdate()
    {
        // 一定速度を保つ
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突ときに反射ベクトル計算
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // 反発する
        direction = Vector3.Reflect(direction, normal);
    }
}
