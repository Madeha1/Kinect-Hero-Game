using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class BodySourceView : MonoBehaviour
{
    //public Material BoneMaterial;
    private Vector3 _leftHand;
    private Vector3 _rightHand;
    private Vector3 _leftKnee;
    private Vector3 _rightKnee;
    private Quaternion _leftShoulderQ;
    private Quaternion _rightShoulderQ;
    private Vector3 _leftElbow;
    private Vector3 _rightElbow;
    private Quaternion _headQ;
    private Vector3 _pelvisSpineBase;//قاعدة العمود الفقري مع الحوض
    private Quaternion _torsoQ; //الجذع


    public GameObject RightHandObj;
    public GameObject LeftHandObj;
    public GameObject RightElbowObj;
    public GameObject LeftElbowObj;
    public GameObject PelvisObj;//حوض
    public GameObject PauseScreen;
    public Change change;

    public float RightShoulderY;
    public float LeftShoulderY;
    public float LeftAnkleY;
    public float RightAnkleY;
    public float HeadZ;
    public float Chest;

    private float _speed = Change.moveSpeed;
    //Dictionary of Bodies and their respective ID's
    private Dictionary<ulong, GameObject> _bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _bodyManager;

    public bool user = false;

    //Dictionary of Joints of the body in the kinect
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    void Start()
    {
        _bodyManager = FindObjectOfType<BodySourceManager>();
    }
    void Update()
    {
        //Kinect initialization
        if (_bodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _bodyManager.GetData();
        if (data == null)
        {
            PauseScreen.SetActive(true);
            Change.moveSpeed = _speed;
            return;
        }

        //Add ID when a body is detected
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

        List<ulong> knownIds = new List<ulong>(_bodies.Keys);

        // Delete bodies that are no longer detected
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_bodies[trackingId]);
                _bodies.Remove(trackingId);
                user = false;
            }
        }
        //Wait for the Kinect to detect a body
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            //A new body is created if detected
            if (body.IsTracked)
            {
                if (!_bodies.ContainsKey(body.TrackingId))
                {
                    _bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    //When new Body added
                    if (_bodies.Count > 1 || _bodies.Count == 0)
                    {
                        stopMoving();
                    }
                    else
                    {
                        startMoving();
                    }
                }
                user = true;
                //place the cubes in the positions corresponding to each joint
                RefreshBodyObject(body, _bodies[body.TrackingId]);

                //hands
                RightHandObj.transform.position = new Vector3(_rightHand.x, _rightHand.y, _rightHand.z);
                LeftHandObj.transform.position = new Vector3(_leftHand.x, _leftHand.y, _leftHand.z);

                //pelvis
                PelvisObj.transform.position = new Vector3(_pelvisSpineBase.x, _pelvisSpineBase.y + 0.1f, _pelvisSpineBase.z);

                //elbows              
                RightElbowObj.transform.position = new Vector3(_rightElbow.x, _rightElbow.y, _rightElbow.z);
                LeftElbowObj.transform.position = new Vector3(_leftElbow.x, _leftElbow.y, _leftElbow.z);
            }
            else
            {
                //All the time(when a body is out) Just one player;
                if (_bodies.Count > 1 || _bodies.Count == 0)
                {
                    stopMoving();
                }
                else if (_bodyManager.IsAvailable()) //check if there is one body and at the same time the sensor is connected
                {
                    startMoving();
                }
            }
        }
    }

    private float roundOut(float joint)
    {
        return Mathf.Round(joint * Mathf.Pow(10, 2)) / 100;
    }

    /*creates the body using the cubes (name and scale)
    but does not give them a location*/
    private GameObject CreateBodyObject(ulong id)
    {
        //create a body and give it an id
        GameObject body = new GameObject("Body:" + id);
        //runs through the joints of the body in the kinect
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            //for each joint create a cube
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //the joint cube is scaled, given a name and assigned to the body     
            jointObj.transform.localScale = new Vector3(0.00005f, 0.00005f, 0.00005f);
            //jointObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        return body;
    }

    //place the cubes in the positions corresponding to each joint
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        //runs through the joints of the body in the kinect 
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            //save the joints in the variable sourceJoint
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            //finds and relates the joints of the created body to the joints of the body object
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            //luego de encontrar el correspondiente joint, le da su correspondiente ubicación
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            //saves the location of each body joint in a variable
            if (jt.ToString().Equals("HandLeft"))
            {
                _leftHand = GetVector3FromJoint(sourceJoint);
                //print(manoIzk.x + "   " + manoIzk.y + "   " + manoIzk.z);
            }

            if (jt.ToString().Equals("HandRight"))
            {
                _rightHand = GetVector3FromJoint(sourceJoint);
                //print(manoDer.x + "   " + manoDer.y + "   " + manoDer.z);
            }

            if (jt.ToString().Equals("ElbowLeft"))
            {
                _leftElbow = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ElbowRight"))
            {
                _rightElbow = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ShoulderLeft"))
            {
                _leftShoulderQ = GetQuaternionJoint(body, jt);
                LeftShoulderY = map(_leftShoulderQ.x, 0.70f, 0.80f, 0, -30);
                //leftShoulderY = map(leftShoulderQ.x, 0.70f, 0.80f, 0, 30);

            }

            if (jt.ToString().Equals("ShoulderRight"))
            {
                _rightShoulderQ = GetQuaternionJoint(body, jt);
                RightShoulderY = map(_rightShoulderQ.x, 0.80f, 0.70f, 0, -30);
            }

            if (jt.ToString().Equals("Neck"))
            {
                _headQ = GetQuaternionJoint(body, jt);
                float zAxis = _headQ.x;
                if (zAxis > 0.05f)
                {
                    zAxis = 0.05f;
                }
                else if (zAxis < -0.05f)
                {
                    zAxis = -0.05f;
                }
                else
                {
                    zAxis = _headQ.x;
                }
                zAxis = map(zAxis, -0.05f, 0.05f, -40, 40);
            }

            if (jt.ToString().Equals("SpineMid"))
            {
                _torsoQ = GetQuaternionJoint(body, jt);
                Chest = map(_torsoQ.w, 0.4f, -0.3f, 110, 240);
            }

            if (jt.ToString().Equals("SpineBase"))
            {
                _pelvisSpineBase = GetVector3FromJoint(sourceJoint);
                //print(pelvisSpineBase.x + "   " + pelvisSpineBase.y + "   " + pelvisSpineBase.z);
            }
        }
    }

    //it returns the position of the joint
    public static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        //return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
        return new Vector3(joint.Position.X * 2.8f, joint.Position.Y * 2.8f, joint.Position.Z * 2.8f);
    }

    //it returns the orientation of the joint
    public static Quaternion GetQuaternionJoint(Kinect.Body body, Kinect.JointType jointTd)
    {
        var orientacion = body.JointOrientations[jointTd].Orientation;

        return new Quaternion(orientacion.X, orientacion.Y, orientacion.Z, orientacion.W);
    }

    public static float map(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.
        return m * x + c;
    }

    public void stopMoving()
    {
        PauseScreen.SetActive(true);
        Change.moveSpeed = 0;
    }

    public void startMoving()
    {
        PauseScreen.SetActive(false);
        Change.moveSpeed = _speed;
    }
}
