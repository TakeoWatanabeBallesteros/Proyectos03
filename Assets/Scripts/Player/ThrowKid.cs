using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowKid : MonoBehaviour
{
    public GameObject endPoint;
    public Rigidbody projectile;
    public LineRenderer lineVisual;
    public int lineSegments = 30;
    private InputPlayerController controls;
    // public ColorBall CoinMarker;
    public GameObject ThisProjectile;
    private bool aiming;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        controls = gameObject.GetComponent<InputPlayerController>();
        cam = Camera.main;
        lineVisual.positionCount = lineSegments;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
        aiming = controls.secondaryShoot;
    }
    private void LaunchProjectile()
    {
        if(!aiming) return;
        
        Vector3 vo = CalculateVelocity(endPoint.transform.position, transform.position, 1f);

        visualize(vo);

        //transform.rotation = Quaternion.LookRotation(vo);
        // if (!controls.secondaryShoot && CoinMarker.CanShoot)
        if (!controls.secondaryShoot && aiming)
        {
            Rigidbody obj = Instantiate(projectile, transform.position, Quaternion.identity);
            ThisProjectile = obj.gameObject;
            obj.velocity = vo;
        }
    }

    private void visualize(Vector3 vo)
    {
        for (int i = 0; i< lineSegments; i++)
        {
            Vector3 pos = CalculatePositionInTime(vo, i / (float)lineSegments);
            lineVisual.SetPosition(i, pos);
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

    }
    Vector3 CalculatePositionInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = transform.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + transform.position.y;

        result.y = sY;

        return result;
    }
}
