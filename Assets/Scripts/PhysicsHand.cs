using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicsHand : MonoBehaviour
{
    [Header ("PID")]
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Transform target;
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotationFrequency = 100f;
    [SerializeField] float rotationDamping = 0.9f;

    [Header("Springs")]
    [SerializeField] float climbForce = 1000f;
    [SerializeField] float climbDrag = 500f;

    [Header("Particles")]
    [SerializeField] ParticleSystem pSystem;

    [Header("Sounds")]
    [SerializeField] AudioSource touchAudio;

    Vector3 previousPosition;
    Rigidbody rb;
    bool isColliding;
    XRController xr;
    

    void Start()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity; 
        previousPosition = transform.position;
        
        xr = (XRController) GameObject.FindObjectOfType(typeof(XRController));
    }

    void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();

        CheckHandPosition();
        if(isColliding){
            HookesLaw();
        }
    }

    void PIDMovement(){
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigidbody.velocity - rb.velocity) * kdg;
        rb.AddForce(force, ForceMode.Acceleration);
    }

    void PIDRotation(){
        float kp = (6f * rotationFrequency) * (6f * rotationFrequency) * 0.25f;
        float kd = 4.5f * rotationFrequency * rotationDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if(q.w < 0){
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle - rb.angularVelocity * kdg;
        rb.AddTorque(torque, ForceMode.Acceleration);
    }

    void CheckHandPosition(){
        return;
    }

    void HookesLaw(){
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();

        playerRigidbody.AddForce(force, ForceMode.Acceleration);
        playerRigidbody.AddForce(drag * -playerRigidbody.velocity * climbDrag, ForceMode.Acceleration);
    }   

    float GetDrag(){
        Vector3 handVelocity = (target.localPosition - previousPosition) / Time.fixedDeltaTime;
        float drag = 1f / handVelocity.magnitude + 0.01f;
        drag = drag > 1f ? 1f : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        previousPosition = transform.position;
        return drag;
    }

    void OnCollisionEnter(Collision other)
    {
        isColliding = true;
        playerRigidbody.velocity = new Vector3(0,0,0);

        touchAudio.transform.position = transform.position;
        touchAudio.Play();

        // haptic feedback using vibration
        xr.SendHapticImpulse(0.7f, 0.2f);

        pSystem.transform.position = transform.position;
        pSystem.Play();
    }   

    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
        touchAudio.Stop();
        pSystem.Stop();
    }
}
