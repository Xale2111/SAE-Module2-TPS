using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float radius = 0.01f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.2f;

    public bool IsGrounded = true;
    float coyoteDelay = 0;
    
    private readonly Collider[] _hits = new Collider[1]; 

    // Update is called once per frame
    void Update()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, _hits, groundLayer);

        if (hitCount > 0)
        {
            coyoteDelay = coyoteTime;
        }
        else
        {
            coyoteDelay -= Time.deltaTime;
        }

        IsGrounded = coyoteDelay > 0;


    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
