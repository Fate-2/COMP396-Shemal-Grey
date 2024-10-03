using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class EnemyEyes : Perception
{

    [SerializeField] private float _distance = 16;
    [SerializeField] private float _angle = 12; //field of View
    [SerializeField] private float _height = 10;
    [SerializeField] private int _scanFrequency = 32;
    [SerializeField] private LayerMask _trackableLayers; // All layers we want our enemy to be aware of it
    [SerializeField] private LayerMask _occlusionLayers; // Any layer that will block enemy from seeing the trackables
    [SerializeField] private List<GameObject> _objectsInSight = new(); // A collection of objects that are in the visible area

    private Collider[] _colliders = new Collider[50]; // Creating a buffer of colliders to check when a trackable is in sight
    private int _count;
    private float _scanInterval;
    private float _scanTimer;


    public  bool TrackableIsInSight;
   protected override void Initialize()
    {
        TrackableIsInSight = false;
        _scanInterval = 1.0f / _scanFrequency;
    }

    
    protected override void UpdatePerception()
    {
        _scanTimer -= Time.deltaTime;
            if (_scanTimer < 0)
            {
               _scanTimer += _scanInterval;
              Scan();
            }
    }




    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position,
         _distance, _colliders, _trackableLayers, QueryTriggerInteraction.Collide);

        _objectsInSight.Clear();
        for (int i = 0; i < _count; i++)
        {
            var obj = _colliders[i].gameObject;
            if (IsInSight(obj))
            {
                _objectsInSight.Add(obj);
                //Send an event to Enemy to change States
                TrackableIsInSight = true;

            }
            else
            {
                TrackableIsInSight = false;
            }
        }
    }


    private bool IsInSight (GameObject go)
    {
        Vector3 origin = transform.position;
        Vector3 dest = go.transform.position;
        Vector3 direction = dest - origin;


        // Checked the hieght position of the trackable 
        if (direction.y < 0 || direction.y > _height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.position);

        // If the deltaAngle is bigger, means that the trackable is outside the Field of View
        
        if (deltaAngle > _angle)
        { 
            return false;
        }

        // Check if something else is between the trackable
        origin.y += _height * 0.5f;
        dest.y = origin.y;

        if(Physics.Linecast(origin, dest, _occlusionLayers))
        {
            return false;
        }

        return true;
    }


        private void OnDrawGizmos()
        {

            if (!visualDebug) { return; }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _distance);
            Gizmos.color = Color.red;
            for (int i = 0; i < _count; i++)
            {
                Gizmos.DrawSphere(_colliders[i].transform.position, 0.3f);
            }
            Gizmos.color = Color.green;
            foreach (var go in _objectsInSight)
            {
                Gizmos.DrawSphere(go.transform.position, 0.5f);
            }
        }
    }
