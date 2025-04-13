using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRatMove : Ratmovement
{    
    public bool moveForward = true;
    public List<Transform> wayPoints;
    public Transform currentWayPoint;
    public int wayPointID = 0;
    public float wayPointDist = 3f; //Distance from waypoint

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWayPoint = wayPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = currentWayPoint.position; //Ai uses mouse position as target position

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
            AimRat();
        }

        if (false) //Best not to make the ai rat jump if possible, way too hard to control
        {
            JumpRat();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            enterGrounded();
        }

        jumpLockOut -= Time.deltaTime;

        for (int i = 0; i < speedStates.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeSpeed(i);
            }
        }

        if(Vector3.Distance(transform.position, currentWayPoint.position) < wayPointDist){
            if(wayPointID + 1 < wayPoints.Count){
                wayPointID += 1;
                currentWayPoint = wayPoints[wayPointID];
            }else{
                wayPointID += 0;
                currentWayPoint = wayPoints[wayPointID];
                moveForward = false;
              //  restartPath();
            }
        }


        CheckGroundedState();
    }

    void FixedUpdate(){
                rb.angularVelocity = Vector3.zero; // Prevent unwanted rotation

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
        //Input taken outside of function to simplify making ai character controller
            if (moveForward)
            {
                MoveRat();
            }else{
                backRB.velocity = new Vector3(0, backRB.velocity.y, 0);
            }
        }
        BalanceRat();
    }

   public void AimRat()
    {
        if (Camera.main == null)
            return;

        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = playerScreenPos.z;

        Vector3 worldMousePos = currentWayPoint.position;

        Vector3 direction = worldMousePos - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            // Calculate the target rotation in the Y-axis direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Get the current rotation and the difference
            Quaternion currentRotation = transform.rotation;
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(currentRotation);
            
            // Extract the yaw (rotation around the Y-axis) from the difference
            float yaw = rotationDifference.eulerAngles.y;
            
            // Normalize yaw to avoid weird behavior when crossing 180 degrees
            if (yaw > 180f) yaw -= 360f;

            if (Mathf.Abs(yaw) > 0.1f)
            {
                Vector3 torque = Vector3.up * yaw * turnPower * Time.deltaTime;
                rb.AddTorque(torque, ForceMode.Force);
            }
        }
    }

    public void restartPath(){
        wayPointID = 0;
        currentWayPoint = wayPoints[0];
        moveForward = true;
    }

    public void replacePath(List<Transform> waypointList){
        wayPoints = waypointList;
    }

}
