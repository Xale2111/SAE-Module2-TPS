using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float radius = 0.01f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.2f;

    public bool IsGrounded = true;
    private bool _touchingGround = false;
    float _coyoteDelay = 0;
    

    // Update is called once per frame
    void Update()
    {
        _touchingGround = Physics.CheckSphere(transform.position, radius, groundLayer);

        if (_touchingGround)
        {
            _coyoteDelay = coyoteTime;
        }
        else
        {
            _coyoteDelay -= Time.deltaTime;
        }

        IsGrounded = _coyoteDelay > 0;


    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
