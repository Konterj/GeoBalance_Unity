using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileController : MonoBehaviour
{

    //Version script of by 0.0.1
    //Pls refactoring this will to saw

    public float RotationSpeed = 5;
    public float VelocityRoteted;
    public Rigidbody2D RotatedVeloc;
    public RectTransform GetRectRight;
    public RectTransform GetRectLeft;

    

    private int LeftFingerId = -1;
    private int RightFingerId = -1;

    public TextMeshProUGUI debugInfo;

    //RightAndLeftValue
    private float touchLeft = 0;
    private float touchRight = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnRotatedPlatform();
        OnDisplayText();
    }

    public void OnRotatedPlatform()
    {
        OnCalcueRotatedTouch();
        OnCalcueMovingFingerId();
        //Left side
        if (touchLeft == 1 && RotatedVeloc.rotation < 80)
        {
            RotatedVeloc.AddTorque(touchLeft * RotationSpeed * Time.fixedDeltaTime);
        }
        //Right side
        else if (touchRight == -1 && RotatedVeloc.rotation > -80)
        {
            RotatedVeloc.AddTorque(touchRight * RotationSpeed * Time.fixedDeltaTime);
        }
        if(RotatedVeloc.angularVelocity > 180 || RotatedVeloc.angularVelocity < -180)
        {
            RotatedVeloc.angularVelocity *= 0.95f;
        }
    }
    public void OnCalcueRotatedTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector2 touchPosition = touch.position;
            //It begin
            if(touch.phase == TouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint(GetRectLeft, touchPosition))
            {
                LeftFingerId = touch.fingerId;
            }
            else if (touch.phase == TouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint(GetRectRight, touchPosition))
            {
                RightFingerId = touch.fingerId;
            }
            //It stationary or Moved
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) 
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(GetRectLeft, touchPosition))
                {
                    RightFingerId = -1;
                    LeftFingerId = touch.fingerId;
                }
                else if(RectTransformUtility.RectangleContainsScreenPoint(GetRectRight, touchPosition))
                {
                    LeftFingerId = -1;
                    RightFingerId = touch.fingerId;
                }
            }

            //Endend
            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) 
            {
                if (touch.fingerId == LeftFingerId)
                {
                    LeftFingerId = -1;
                    touchLeft = 0;
                }
                else if (touch.fingerId == RightFingerId) 
                {
                    RightFingerId = -1;
                    touchRight = 0;
                }
            }
        }
    }

    void OnCalcueMovingFingerId()
    {
        //calcue moving
        if (LeftFingerId >= 0)
        {
            touchLeft = 1;
            touchRight = 0;
        }
        else if (RightFingerId >= 0)
        {
            touchRight = -1;
            touchLeft = 0;
        }
    }

    void OnDisplayText()
    {
        debugInfo.text = $"Information Input.Touch\r\nleft fingerID:{LeftFingerId}                    rightFingerID: {RightFingerId}\r\nTouchLeft: {touchLeft}                    TouchRight: {touchRight}";
    }
}
