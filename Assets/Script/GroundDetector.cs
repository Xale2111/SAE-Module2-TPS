using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float radius = 0.01f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded = false;
    
    private readonly Collider[] _hits = new Collider[1]; 

    // Update is called once per frame
    void Update()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, _hits, groundLayer);
        IsGrounded = hitCount > 0;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
