using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerItemController : MonoBehaviour
{

    [SerializeField] private float radius;
    [SerializeField] private float offset;
    [SerializeField] private LayerMask layermask;

    [SerializeField] private Rig rig;
    [SerializeField] private Transform aimRig;
    [SerializeField] private float weightChangeSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] items = Physics.OverlapSphere(transform.position + transform.forward * offset, radius, layermask);
        float rigWeight = 0;
        
        if (items.Length > 0)
        {
            foreach (Collider item in items)
            {
                
                if (item.TryGetComponent(out LookAtItem lookAtItem))
                {
                    rigWeight = 1;
                }
            }
        }
        rig.weight = Mathf.Lerp(rig.weight,rigWeight,weightChangeSpeed*Time.deltaTime); 
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position+transform.forward*offset, radius);
    }
}
