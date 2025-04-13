using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float returnSpeed = 3f;
    private Vector3 startPosition;
    private bool isAttacking = false;
    private bool isReturning = false;
    private bool attackFinished = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackPoint.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, attackPoint.position) < 0.1f)
            {
                isAttacking = false;
                isReturning = true;
                attackFinished = true; 
            }
        }

        if (isReturning)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition) < 0.1f)
            {
                isReturning = false;
            }
        }
    }

    public void StartAttack()
    {
        if (!isAttacking && !isReturning)
        {
            isAttacking = true;
            attackFinished = false; 
        }
    }

    public bool IsAttackingOrReturning()
    {
        return isAttacking || isReturning;
    }

    public bool AttackFinished()
    {
        return attackFinished;
    }

    public void RespawnPlayer(GameObject player)
    {
        Debug.Log("Player hit! Respawning...");
        
        StartCoroutine(fadeManager.RespawnFade());
        Ratmovement ratMove = player.gameObject.GetComponent<Ratmovement>();
        player.transform.position = spawnPoint.position;
        ratMove.backLeg.position = spawnPoint.position;
    }
}
