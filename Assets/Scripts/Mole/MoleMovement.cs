using UnityEngine;

public class MovingBillboard : MonoBehaviour
{
    [SerializeField] private Vector3 positionA;
    [SerializeField] private Vector3 positionB;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private Transform player;

    private Vector3 currentTarget;

    private void Start()
    {
        currentTarget = positionB;
        transform.position=positionA;
    }

    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        FacePlayer();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f){
            if (currentTarget == positionA){
                currentTarget = positionB;
            }
            else{
                currentTarget = positionA;
            }
        }
    }

    private void FacePlayer()
    {
        if (player == null)
            return;
        
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}