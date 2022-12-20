using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceViewForHands : MonoBehaviour
{
    public BodySourceManagerForHands BodySourceManager;
    public GameObject JointObject;
    
    private Dictionary<ulong, GameObject> _bodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
    };

    void Update () 
    {
        #region Get Kinect Data
        Body[] data = BodySourceManager.GetData();
             
        if (data == null)
        {
            return;
        }
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }
        #endregion

        #region Delete Kinect Bodies
        List<ulong> knownIds = new List<ulong>(_bodies.Keys);
        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                //destroy the game object
                Destroy(_bodies[trackingId]);
                //remove from list
                _bodies.Remove(trackingId);
            }
        }
        #endregion

        #region Create Kinect Bodies
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_bodies.ContainsKey(body.TrackingId))
                {
                    _bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _bodies[body.TrackingId]);
            }
        }
        #endregion
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        foreach (JointType jt in _joints)
        {
            GameObject newJoint = Instantiate(JointObject);
            newJoint.name = jt.ToString();

            //parent to body 
            newJoint.transform.parent = body.transform;
        }
        return body;
    }
    
    private void RefreshBodyObject(Body body, GameObject bodyObject)
    {
        foreach (JointType jt in _joints)
        {
            Joint sourceJoint = body.Joints[jt];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            Transform jointObject = bodyObject.transform.Find(jt.ToString());
            jointObject.position = targetPosition;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
