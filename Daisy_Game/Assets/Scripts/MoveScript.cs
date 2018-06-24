using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Vector2 m_InitialClickLocation;
    private Vector3 m_OriginalTransformLocation;

    private Vector2 m_LastMousePosition;
    [SerializeField]
    bool enablemovement = false;

    [SerializeField]
    float m_MaximumDisplacement = 100.0f;

    // Use this for initialization
    void Start()
    {
        //Sets up our variables in a default state
        m_OriginalTransformLocation = this.transform.localPosition;
        m_InitialClickLocation = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDrag()
    {
        if (enablemovement)
        {
            MoveWithCursor();

            //Evaluates if we have moved the cursor far enough away from the start location that we should 
            if (ShouldSnapToTarget())
            {
                Vector3 targetPosition = GetTargetLocation();
                SnapToPosition(targetPosition);
            }

            m_LastMousePosition = Input.mousePosition;
        }
    }

    Vector3 GetTargetLocation()
    {
        //Cast the current mouse location into the world to work out where we are in 2D.

        //TODO: Query the grid for the real location we should use.

        return this.transform.localPosition;
    }

    bool ShouldSnapToTarget()
    {
        float displacement = Vector2.Distance(Input.mousePosition, m_InitialClickLocation);
        return displacement >= m_MaximumDisplacement;
    }

    void SnapToPosition(Vector3 position)
    {
        this.transform.localPosition = position;

        //write over the original location because this is our now location
        m_OriginalTransformLocation = this.transform.localPosition;
    }

    void OnMouseDown()
    {
        if (enablemovement)
        {
            //Store the initial start location
            LogInitialState();


            m_LastMousePosition = Input.mousePosition;
        }
    }

    private void OnMouseUp()

    {
        if (enablemovement)
        {
            //reset if we are not at the desired location
            if (this.transform.localPosition != m_OriginalTransformLocation)
            {
                //Release the trigger
                ResetPosition();
            }
        }
    }

    private void LogInitialState()
    {
        m_InitialClickLocation = Input.mousePosition;
        m_OriginalTransformLocation = this.transform.localPosition;
    }

    private void ResetPosition()
    {
        m_InitialClickLocation = Vector2.zero;
        this.transform.localPosition = m_OriginalTransformLocation;
    }

    //Uses event information to dictate the motion we use on our object
    private void MoveWithCursor()
    {
        Vector2 mouseDisplacement = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - m_LastMousePosition;
        Vector2 deltaPositionChange = mouseDisplacement * Time.deltaTime;

        //Implicit create vector 3 from vector 2
        Vector3 positionChange = deltaPositionChange;
        this.transform.localPosition += positionChange;
    }
}
