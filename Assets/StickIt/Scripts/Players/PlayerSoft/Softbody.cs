using UnityEditor;
using UnityEngine;
public static class Softbody
{
    #region --- helpers ---
    public enum ColliderShape
    {
        Box,
        Sphere,
    }
    #endregion
    public static float ColliderSize;
    public static float RigidbodyMass;
    public static float Spring;
    public static float Damper;
    public static RigidbodyConstraints Constraints;
    public static PhysicMaterial matBones;
    public static LineRenderer PrefabLine;
    public static bool ViewLines;
    public static SoftJointLimitSpring limitSpring;


    public static void Init(float collidersize, float rigidbodymass, float spring, float damper, RigidbodyConstraints constraints, PhysicMaterial mat)
    {
        ColliderSize = collidersize;
        RigidbodyMass = rigidbodymass;
        Spring = spring;
        Damper = damper;
        Constraints = constraints;
        matBones = mat;
    }
    public static Rigidbody AddCollider(ref GameObject go)
    {
        return AddCollider(ref go, ColliderSize, RigidbodyMass);
    }
    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2)
    {
        SpringJoint sp = AddSpring(ref go1, ref go2, Spring, Damper);
        sp.enableCollision = true;
        sp.tolerance = 0.01f;
        return sp;
    }
    public static Rigidbody AddCollider(ref GameObject go, float size, float mass)
    {
        SphereCollider sc = go.AddComponent<SphereCollider>();
        sc.radius = size;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = 0f;
        rb.angularDrag = 1.0f;
        rb.constraints = Constraints;
        return rb;
    }
    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2, float spring, float damper)
    {
        SpringJoint sp = go1.AddComponent<SpringJoint>();
        sp.connectedBody = go2.GetComponent<Rigidbody>();
        sp.spring = spring;
        sp.damper = damper;
        return sp;
    }
    public static ConfigurableJoint AddConfJoint(ref GameObject go1, ref GameObject go2)
    {
        ConfigurableJoint cj = AddConfJoint(ref go1, ref go2, Spring, Damper);
        cj.enableCollision = true;
        //cj.tolerance = 0.01f;
        return cj;
    }
    public static ConfigurableJoint AddConfJoint(ref GameObject go1, ref GameObject go2, float spring, float damper)
    {
        ConfigurableJoint cj = go1.AddComponent<ConfigurableJoint>();
        cj.connectedBody = go2.GetComponent<Rigidbody>();
        limitSpring.spring = spring;
        limitSpring.damper = damper;
        cj.linearLimitSpring = limitSpring;
        cj.angularXLimitSpring = limitSpring;
        cj.angularYZLimitSpring = limitSpring;
        cj.angularXLimitSpring = limitSpring;
        JointDrive jd = new JointDrive();
        jd.maximumForce = 3.402823e+38f;
        jd.positionSpring = spring;
        jd.positionDamper = damper;
        cj.xDrive = jd;
        cj.yDrive = jd;
        cj.zDrive = jd;
        cj.angularXDrive = jd;
        cj.angularYZDrive = jd;
        cj.slerpDrive = jd;
        return cj;
    }
}