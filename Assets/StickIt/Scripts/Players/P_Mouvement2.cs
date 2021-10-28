﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class P_Mouvement2 : MonoBehaviour
{
    public Player myPlayer;

    public enum STATE { STICK, AIR }
    public STATE state = STATE.AIR;

    bool isGrounded = false;

    [Header("Dots Preview")] //-----------------------
    [SerializeField] Transform dotPreview;
    public float spaceBetweenDots;
    public int numberOfDots;
    private Transform[] dots;
    private bool isDotsEnabled = false;

    [Header("Movement")]     //-----------------------
    [Tooltip("Force maximale du jump, et clamp de la v�locit� maximale")]
    public float maxSpeed;
    bool isAnimCurveSpeed;
    Vector3 addedVector;
    [Tooltip("% de la velocit� ajout�e au saut en fonction du temps, cette valeur doit finir � 1")]
    [SerializeField] AnimationCurve animCurveJumpSpeed;
    float t_speed;
    float y_speed = 1;
    Vector2 direction;
    bool isChargingJump = false;
    [SerializeField]
    private float minForceJumpMultiplicator;
    float forceJumpMultiplicator;
    [SerializeField]
    float speedIncreaseForceJump;
    bool hasJumped = false;
    [SerializeField] AnimationCurve animCurveJumpGravity;
    float t_jump;
    float y_jump = 1;
    public float gravityStrength;
    private Rigidbody rb;
    public Vector3 velocityLastFrame = Vector3.zero;

    private List<ContactPointSurface> connectedPoints = new List<ContactPointSurface>();
    public float attractionMultiplier;
    [Range(0, 1)] public float repulsionMultiplier;
    [SerializeField] bool isSlippery;

    public int maxNumberOfJumps;
    private int currentNumberOfJumps;

    [Header("CollisionVariables")]
    public GameObject collisionEffect;

    List<StickPoint> stickPoints = new List<StickPoint>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dots = new Transform[numberOfDots];
        for (int i = 0; i < dots.Length; i++)
        {
            Transform newDot = Instantiate(dotPreview, transform);
            newDot.gameObject.SetActive(false);
            dots[i] = newDot;
        }

        currentNumberOfJumps = maxNumberOfJumps;


    }

    // Update is called once per frame
    void Update()
    {

        PreviewDirection();

        if (isChargingJump && connectedPoints.Count > 0)
        {

            if (!isDotsEnabled)
            {
                EnableDots(true);

            }

            IncreaseForceJump();
        }

        if (hasJumped)
        {
            AnimCurveJumpGravity();

        }
        if (isAnimCurveSpeed)
        {
            AnimCurveJumpSpeed();
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.GetChild(0).position, new Vector3(0, -1, 0), 0.1f))
        {
            isGrounded = true;
        }
        else isGrounded = false;


        if (state != STATE.STICK)
        {
            // realVelocity += new Vector2(0, -gravityStrength) * y_jump * Time.fixedDeltaTime;

            rb.velocity += new Vector3(0, -gravityStrength) * y_jump * Time.fixedDeltaTime;

            //rb.velocity -= addedVector;
            //addedVector = rb.velocity * y_speed;
            //rb.velocity += addedVector;
        }

        if (state == STATE.STICK)
            Attraction();

        velocityLastFrame = rb.velocity;

    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginVertical();
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 24;
    //    style.normal.textColor = Color.white;

    //    if (myPlayer.id == 0)
    //    {
    //        GUILayout.Label(" Velocity  = " + rb.velocity, style);
    //        GUILayout.Label("Magnitude = " + rb.velocity.magnitude, style);
    //        if(connectedPoints.Count>0)
    //        GUILayout.Label("Attraction = " + connectedPoints[0].attractionStrength, style);
    //    }
    //    GUILayout.EndVertical();


    //}

    // ----- INPUTS -----
    #region Inputs

    public void InputDirection(InputAction.CallbackContext context)
    {
        if (context.performed) direction = context.ReadValue<Vector2>();
        else if (context.canceled) direction = Vector2.zero;
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.started) isChargingJump = true;
        else if (context.canceled && direction != Vector2.zero) Jump();
    }

    #endregion

    // ----- JUMP -----
    #region JUMP
    void Jump()
    {
        if (currentNumberOfJumps > 0 && state == STATE.STICK)
        {
            // Debug.Break();
            float forceJump = maxSpeed * forceJumpMultiplicator;

            rb.velocity = direction * forceJump;

            forceJumpMultiplicator = minForceJumpMultiplicator;
            isChargingJump = false;
            EnableDots(false);
            isAnimCurveSpeed = true;
            hasJumped = true;
            t_jump = 0;
            y_jump = 0;
            t_speed = 0;
            y_speed = 0;
            addedVector = Vector3.zero;

            foreach (ContactPointSurface contact in connectedPoints)
            {
                contact.attractionStrength = 100f;
            }
        }


    }
    void IncreaseForceJump()
    {
        forceJumpMultiplicator += Time.deltaTime * speedIncreaseForceJump;
        forceJumpMultiplicator = Mathf.Clamp(forceJumpMultiplicator, minForceJumpMultiplicator, 1);

    }


    #endregion


    private void Attraction()
    {


        for (int i = connectedPoints.Count - 1; i >= 0; i--)
        {
            Vector3 localPlayerPosition = connectedPoints[i].transform.position - transform.position;
            Vector3 direction = (connectedPoints[i].localPosition - localPlayerPosition).normalized;


            if (isSlippery)
            {
                Vector3 attraction = -direction * connectedPoints[i].attractionStrength;
                float repulsion = connectedPoints[i].attractionStrength * repulsionMultiplier;
                rb.velocity += attraction * Time.fixedDeltaTime;
                if (!isGrounded)
                {
                    rb.velocity += new Vector3(0, -gravityStrength) * Time.fixedDeltaTime * 0.1f;
                    connectedPoints[i].attractionStrength -= gravityStrength * Time.fixedDeltaTime * repulsion;
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.GetChild(0).position, new Vector3(0, -1, 0), out hit, 0.1f))
                    {
                        if (hit.transform == connectedPoints[i].transform)
                        {
                            connectedPoints[i].attractionStrength += gravityStrength * Time.fixedDeltaTime * 3;
                            connectedPoints[i].attractionStrength = Mathf.Clamp(connectedPoints[i].attractionStrength, 0, 2 * attractionMultiplier);
                        }
                    }
                }

            }


            if (connectedPoints[i].attractionStrength < 0)
            {
                connectedPoints.RemoveAt(i);
            }
        }
    }

    public void PrepareToChangeLevel()
    {
        connectedPoints.Clear();
        state = STATE.AIR;
    }

    // ----- PREVIEW DOTS -----
    #region PreviewDots
    private void EnableDots(bool isTrue)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].gameObject.SetActive(isTrue);
        }
        isDotsEnabled = isTrue;
    }
    private void PreviewDirection()
    {
        if (isChargingJump)
        {
            float forceJump = maxSpeed * forceJumpMultiplicator;
            Vector2 potentialVelocity = direction * forceJump;


            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = GetDotPosition(i * spaceBetweenDots, potentialVelocity);
            }

        }
    }
    Vector2 GetDotPosition(float t, Vector2 potentialVelocity)
    {
        Vector2 gravity = new Vector2(0, -animCurveJumpGravity.Evaluate(t) * gravityStrength);

        Vector2 pos = (Vector2)transform.position + (potentialVelocity * t) + 0.5f * gravity * (t * t);
        return pos;

    }
    #endregion

    // ----- ANIMATIONS CURVE -----
    #region Animations curve
    void AnimCurveJumpGravity()
    {
        if (t_jump < animCurveJumpGravity.keys[animCurveJumpGravity.length - 1].time)
        {

            t_jump += Time.deltaTime;
            y_jump = t_jump;
            y_jump = animCurveJumpGravity.Evaluate(y_jump);
        }
        else
        {
            hasJumped = false;

        }
    }
    void AnimCurveJumpSpeed()
    {
        if (t_speed < animCurveJumpSpeed.keys[animCurveJumpSpeed.length - 1].time)
        {

            t_speed += Time.deltaTime;
            y_speed = t_speed;
            y_speed = animCurveJumpSpeed.Evaluate(y_speed);
        }
        else
        {

            isAnimCurveSpeed = false;
        }
    }
    #endregion


}



