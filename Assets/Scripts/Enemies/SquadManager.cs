using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


//CURRENTLY MISSING: Minimum distance, occupy spots, disable collisions between enemies
public class SquadManager : MonoBehaviour
{
    private class Spot
    {
        private Vector3 _position;
        public Vector3 GetPosition => _position;
        private int _angle;
        public int Angle => _angle;
        private int _occupants;
        public bool Occupied => _occupants > 0;

        public Spot(int angle)
        {
            _angle = angle;
            _occupants = 0;
        }
        
        public void AddOccupant()
        {
            _occupants++;
        }

        public void RemoveOccupant()
        {
            _occupants--;
        }
        
        public void UpdatePos(Vector3 pos)
        {
            _position = pos;
        }
    }
    private int _currentShooters = 0;
    [SerializeField] private int _maxShooters;
    public bool CanShoot => _currentShooters < _maxShooters;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask wallMask;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float topDistanceToPlayer;
    [SerializeField] private int topAngle;
    [SerializeField] private int angleBetweenCasts;
    private Transform _camTransform;
    private List<Spot> _spots = new List<Spot>();
    private Dictionary<GameObject, Spot> _occupiedSpots = new Dictionary<GameObject, Spot>(); 
    

    void Awake()
    {
        Physics.IgnoreLayerCollision(8, 8);
        Physics.IgnoreLayerCollision(3, 8);
        _camTransform = Camera.main.transform;
    }

    private void Start()
    {
        for (int i = 0; i <= topAngle; i += angleBetweenCasts)
        {
            _spots.Add(new Spot(i));
            if(i != 0)
                _spots.Add(new Spot(360 - i));
        }
    }

    private void UpdateSpots()
    {
        var castPos = playerTransform.position + Vector3.up * 0.8f;
        foreach (var spot in _spots)
        {
            var angle = spot.Angle;
            var camFwd = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up);
            var dir = Quaternion.AngleAxis(angle, Vector3.up) * camFwd;
            Vector3 spotPos;
            if (Physics.Raycast(castPos, dir, out RaycastHit hit, topDistanceToPlayer, wallMask))
                spotPos = hit.point;
            else
                spotPos = castPos + dir * topDistanceToPlayer;
            NavMesh.SamplePosition(spotPos, out NavMeshHit navHit, 3, NavMesh.AllAreas);
            spotPos = navHit.position;
            spot.UpdatePos(spotPos);
        }
    }

    public Vector3 GetBestSpot(GameObject enemy)
    {
        ClearSpotByEnemy(enemy);
        UpdateSpots();
        var bestSpot = _spots[0];
        var bestHeuristic = -Mathf.Infinity;
        var castPos = playerTransform.position + Vector3.up * 1.2f;
        foreach (var spot in _spots)
        {
            var heuristic = 10.0f;
            heuristic -= Vector3.Distance(enemy.transform.position, spot.GetPosition) / 30.0f;
            var castDir = spot.GetPosition - playerTransform.position;
            if (Physics.Raycast(castPos, castDir.normalized, out RaycastHit hit, topDistanceToPlayer,
                    obstacleMask))
                heuristic -= 15;
            if (spot.Occupied)
                heuristic -= 30;
            if (heuristic > bestHeuristic)
            {
                bestHeuristic = heuristic;
                bestSpot = spot;
            }
        }
        _occupiedSpots.Add(enemy, bestSpot);
        bestSpot.AddOccupant();
        return bestSpot.GetPosition;
    }

    private void ClearSpotByEnemy(GameObject enemy)
    {
        if (_occupiedSpots.ContainsKey(enemy))
        {
            _occupiedSpots[enemy].RemoveOccupant();
            _occupiedSpots.Remove(enemy);
        }
    }

    public void AddShooter()
    {
        _currentShooters++;
    }

    public void RemoveShooter()
    {
        _currentShooters--;
    }

    private void OnDrawGizmos()
    {
        var castPos = playerTransform.position + Vector3.up * 1.2f;
        foreach (var spot in _spots)
        {
            Gizmos.color = Color.magenta;
            var spotPos = spot.GetPosition;
            Gizmos.DrawLine(castPos, spotPos);
            
            Gizmos.color = Color.cyan;
            var camFwd = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up);
            var camCastDir = Quaternion.AngleAxis(spot.Angle, Vector3.up) * camFwd;
            var camCastPos = castPos + camCastDir * topDistanceToPlayer;
            Gizmos.DrawLine(castPos, camCastPos);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spotPos, 0.15f);
            
            //Handles.Label(spot.GetPosition, spot.Angle.ToString());
        }
    }
}
