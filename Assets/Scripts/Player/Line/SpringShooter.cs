using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TagKinematic
{
    public string tag;
    public bool kinematic;
}

[RequireComponent(typeof(PlayerController))]
public class SpringShooter : MonoBehaviour
{
    private Vector3 _point1LocalPos;
    private Vector3 _point2LocalPos;
    
    private GameObject _point1Object;
    private GameObject _point2Object;
    
    private Transform _cameraTransform;
    private PlayerController _playerController;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float springForce;
    [SerializeField] private float distance;
    [SerializeField] private float springTime;
    [SerializeField] private List<TagKinematic> tagKinematics;
    [SerializeField] private LayerMask layerMask;

    private GameObject _springPointBuffer;
    
    [Header("Line Visuals")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float width;
    [SerializeField] private GameObject springPointPrefab;

    private Dictionary<string, bool> _tagKinematicsDictionary = new Dictionary<string, bool>();
    private int _targetCounter;

    void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _playerController = GetComponent<PlayerController>();

        _playerController.OnSpringShoot += (sender, args) => { Shoot(); };
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _targetCounter = 0;
        foreach (var tagKinematic in tagKinematics)
        {
            _tagKinematicsDictionary.Add(tagKinematic.tag, tagKinematic.kinematic);
        }
    }

    private void Shoot()
    {
        if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, distance, layerMask))
            return;
        if (!_tagKinematicsDictionary.ContainsKey(hit.collider.tag))
            return;
        if (_targetCounter == 0)
        {
            _point1Object = hit.transform.gameObject;
            _point1LocalPos = _point1Object.transform.InverseTransformPoint(hit.point);
            _targetCounter++;
            
            var springPoint = Instantiate(springPointPrefab, hit.point, Quaternion.identity);
            springPoint.transform.SetParent(_point1Object.transform);
            springPoint.transform.up = hit.normal;

            _springPointBuffer = springPoint;
        }
        else if (_targetCounter == 1)
        {
            if (_point1Object == hit.transform.gameObject)
                return;
            _point2Object = hit.transform.gameObject;
            _point2LocalPos = _point2Object.transform.InverseTransformPoint(hit.point);
            _targetCounter = 0;
            
            
            var springPoint = Instantiate(springPointPrefab, hit.point, Quaternion.identity);
            springPoint.transform.SetParent(_point2Object.transform);
            springPoint.transform.up = hit.normal;
            
            Destroy(_springPointBuffer, springTime);
            Destroy(springPoint, springTime);
            
            ConnectObjs(_point1Object, _point2Object);
        }
    }

    private void ConnectObjs(GameObject obj1, GameObject obj2)
    {
        var rb1 = ProcessObjectForJoint(obj1);
        var rb2 = ProcessObjectForJoint(obj2);

        if (!rb1.isKinematic)
        {
            var spring = AddSpring(obj1, _point1LocalPos);
            spring.connectedBody = rb2;
        }

        if (!rb2.isKinematic)
        {
            var spring = AddSpring(obj2, _point2LocalPos);
            spring.connectedBody = rb1;
        }
        
        var springLineRenderer = obj1.AddComponent<SpringLineRenderer>();
        springLineRenderer.SetVisuals(lineMaterial, width);
        springLineRenderer.SetObjects(_point1Object.transform, _point1LocalPos, _point2Object.transform, _point2LocalPos);
        Destroy(springLineRenderer, springTime);
    }

    private Rigidbody ProcessObjectForJoint(GameObject obj)
    {
        var isKinematic1 = _tagKinematicsDictionary[obj.tag];
        Rigidbody rb;
        if (!obj.GetComponent<Rigidbody>())
        {
            rb = obj.AddComponent<Rigidbody>();
            rb.mass = 20;
        }
        else
            rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = isKinematic1;
        return rb;
    }

    private SpringJoint AddSpring(GameObject obj, Vector3 anchorPos)
    {
        var spring = obj.AddComponent<SpringJoint>();
        spring.spring = springForce;
        spring.anchor = anchorPos;
        spring.minDistance = 0.0f;
        spring.maxDistance = 0.1f;
        spring.damper = 100;
        spring.autoConfigureConnectedAnchor = false;
        spring.enableCollision = true;
        Destroy(spring, springTime);

        return spring;
    }
}
