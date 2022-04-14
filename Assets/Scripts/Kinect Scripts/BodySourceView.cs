using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class BodySourceView : MonoBehaviour
{
    //public Material BoneMaterial;
    private Vector3 leftHand;
    private Vector3 rightHand;
    private Vector3 leftKnee;
    private Vector3 rightKnee;
    private Quaternion leftShoulderQ;
    private Quaternion rightShoulderQ;
    private Vector3 leftElbow;
    private Vector3 rightElbow;
    private Quaternion headQ;
    private Vector3 pelvisSpineBase;//قاعدة العمود الفقري مع الحوض
    private Quaternion torsoQ; //الجذع


    public GameObject rightHandObj;
    public GameObject leftHandObj;
    public GameObject rightElbowObj;
    public GameObject leftElbowObj;
    public GameObject pelvisObj;//حوض
    public GameObject sensorMessage;
    public Change change;

    public float rightShoulderY;
    public float leftShoulderY;
  
    public float leftAnkleY;
    public float rightAnkleY;

    public float headZ;

    public float chest;

    public Text data;

    public bool user = false;

    private float speed = Change.moveSpeed;
    //Dictionary of Bodies and their respective ID's
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;


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
        _BodyManager = FindObjectOfType<BodySourceManager>();
    }
    void Update()
    {
        //Kinect initialization
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            sensorMessage.SetActive(true);
            Change.moveSpeed = speed;

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

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // Delete bodies that are no longer detected
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
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
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    //When new Body added
                    if (_Bodies.Count > 1 || _Bodies.Count == 0)
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
                RefreshBodyObject(body, _Bodies[body.TrackingId]);

                //hands
                rightHandObj.transform.position = new Vector3(rightHand.x, rightHand.y, rightHand.z);
                leftHandObj.transform.position = new Vector3(leftHand.x, leftHand.y, leftHand.z);

                //pelvis
                pelvisObj.transform.position = new Vector3(pelvisSpineBase.x, pelvisSpineBase.y + 0.1f, pelvisSpineBase.z);

                //elbows              
                rightElbowObj.transform.position = new Vector3(rightElbow.x, rightElbow.y, rightElbow.z);
                leftElbowObj.transform.position = new Vector3(leftElbow.x, leftElbow.y, leftElbow.z);
            }
            else
            {
                //All the time(when a body is out) Just one player;
                if (_Bodies.Count > 1 || _Bodies.Count == 0)
                {
                    stopMoving();
                }
                else if(_BodyManager.isAvailable()) //check if there is one body and at the same time the sensor is connected
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
                leftHand = GetVector3FromJoint(sourceJoint);
                //print(manoIzk.x + "   " + manoIzk.y + "   " + manoIzk.z);
            }

            if (jt.ToString().Equals("HandRight"))
            {
                rightHand = GetVector3FromJoint(sourceJoint);
                //print(manoDer.x + "   " + manoDer.y + "   " + manoDer.z);
            }

            if (jt.ToString().Equals("KneeLeft"))
            {
                leftKnee = GetVector3FromJoint(sourceJoint);
                //print(rodillaIzk.x + "   " + rodillaIzk.y + "   " + rodillaIzk.z);
            }

            if (jt.ToString().Equals("KneeRight"))
            {
                rightKnee = GetVector3FromJoint(sourceJoint);
                //print(rodillaDer.x + "   " + rodillaDer.y + "   " + rodillaDer.z);
            }

            if (jt.ToString().Equals("ElbowLeft"))
            {
                leftElbow = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ElbowRight"))
            {
                rightElbow = GetVector3FromJoint(sourceJoint);
            }

            if (jt.ToString().Equals("ShoulderLeft"))
            {
                leftShoulderQ = GetQuaternionJoint(body, jt);
                leftShoulderY = map(leftShoulderQ.x, 0.70f, 0.80f, 0, -30);
                //leftShoulderY = map(leftShoulderQ.x, 0.70f, 0.80f, 0, 30);

            }

            if (jt.ToString().Equals("ShoulderRight"))
            {
                rightShoulderQ = GetQuaternionJoint(body, jt);
                rightShoulderY = map(rightShoulderQ.x, 0.80f, 0.70f, 0, -30);
            }

            if (jt.ToString().Equals("Neck"))
            {
                headQ = GetQuaternionJoint(body, jt);
                float zAxis = headQ.x;
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
                    zAxis = headQ.x;
                }
                zAxis = map(zAxis, -0.05f, 0.05f, -40, 40);
            }

            if (jt.ToString().Equals("SpineMid"))
            {
                torsoQ = GetQuaternionJoint(body, jt);
                chest = map(torsoQ.w, 0.4f, -0.3f, 110, 240);
            }

            if (jt.ToString().Equals("SpineBase"))
            {
                pelvisSpineBase = GetVector3FromJoint(sourceJoint);
                //print(pelvisSpineBase.x + "   " + pelvisSpineBase.y + "   " + pelvisSpineBase.z);
            }
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
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
        sensorMessage.SetActive(true);
        Change.moveSpeed = 0;
    }

    public void startMoving()
    {
        sensorMessage.SetActive(false);
        Change.moveSpeed = speed;
    }

}
