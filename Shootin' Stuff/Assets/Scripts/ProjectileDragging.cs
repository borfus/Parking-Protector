using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDragging : MonoBehaviour
{
    public GameObject CatapaultPrefab;
    public GameObject ParentCatapault;

    public float MaxStretch = 3.0f;
    public LineRenderer CatapultLineFront;
    public LineRenderer CatapultLineBack;
    public GameObject Asteroid;
    public Material[] materials;
    public Material material;
    public GameObject TrajectoryPointPrefeb;
    public int numOfTrajectoryPoints = 30;
    public float LineLengthScalar = 1.0f;
    public LineRenderer TrajectoryLineRenderer;

    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSqr;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;
    private bool ammo = true;
    private Vector3 startLocation;
    private SpringJoint2D newSpring;
    private Rigidbody2D connectedBody;
    private CircleCollider2D circle;

    void Start()
    {
        ParentCatapault = transform.parent.gameObject;
        spring = GetComponent<SpringJoint2D>();
        catapult = spring.connectedBody.transform;
        CatapultLineBack.material = materials[0];
        CatapultLineFront.material = materials[0];
        startLocation = transform.position;
        LineRendererSetup();
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(CatapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = MaxStretch * MaxStretch;
        circle = GetComponent<Collider2D>() as CircleCollider2D;
        circleRadius = circle.radius;
        newSpring = this.GetComponent<SpringJoint2D>();
        connectedBody = GetComponent<SpringJoint2D>().connectedBody;
        LineRendererUpdate();
    }

    void Update()
    {
        AmmoCheck();

        if (clickedOn)
        {
            Dragging();
        }
        if (spring != null)
        {
            if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
            {
                Destroy(spring);
                GetComponent<Rigidbody2D>().velocity = prevVelocity;
                ammo = false;
                StartCoroutine(waitAndDestroy());
                print(prevVelocity);
            }
            if (clickedOn)
            {
                //prevVelocity = GetComponent<Rigidbody2D>().velocity;
                prevVelocity = GetForceFrom(transform.position, catapult.position);
            }

            LineRendererUpdate();
        }
        else
        {
            CatapultLineBack.material = materials[1];
            CatapultLineFront.material = materials[1];
        }
    }

    void LineRendererSetup()
    {
        CatapultLineFront.SetPosition(0, CatapultLineFront.transform.position);
        CatapultLineBack.SetPosition(0, CatapultLineBack.transform.position);

        CatapultLineFront.sortingLayerName = "Foreground";
        CatapultLineBack.sortingLayerName = "Foreground";
        TrajectoryLineRenderer.sortingLayerName = "Foreground";

        CatapultLineFront.sortingOrder = 3;
        CatapultLineBack.sortingOrder = 1;
        TrajectoryLineRenderer.sortingOrder = 2;
    }

    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
        TrajectoryLineRenderer.enabled = true;
    }

    void OnMouseUp()
    {
        //spring.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        clickedOn = false;
        TrajectoryLineRenderer.enabled = false;
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
        if (catapultToMouse.magnitude > MaxStretch)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(MaxStretch);
        }

        mouseWorldPoint.z = 0;
        transform.position = mouseWorldPoint;

        // Display trajectory path
        Vector3 vel = GetForceFrom(transform.position, catapult.position);
        UpdateTrajectory(transform.position, vel);
    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        var distance = Mathf.Sqrt(Mathf.Pow(catapult.position.x - transform.position.x, 2) + Mathf.Pow(catapult.position.y - transform.position.y, 2));
        var vel = 13.5f;
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * vel;
    }

    void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
    {
        Renderer rend = TrajectoryLineRenderer.GetComponent<Renderer>();
        Vector3 gravity = Physics2D.gravity;
        int numSteps = numOfTrajectoryPoints;
        float timeDelta = LineLengthScalar / initialVelocity.magnitude; // for example

        TrajectoryLineRenderer.positionCount = numSteps;

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        for (int i = 0; i < numSteps; ++i)
        {
            TrajectoryLineRenderer.SetPosition(i, position);

            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;
        }

        rend.material.mainTextureScale = new Vector2(1, 1);
    }

    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - CatapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        CatapultLineFront.SetPosition(1, holdPoint);
        CatapultLineBack.SetPosition(1, holdPoint);
    }

    void AmmoCheck()
    {
        if (!ammo)
        {
            ammo = true;
            StartCoroutine(waitAndSpawn());
        }
    }

    IEnumerator waitAndSpawn()
    {
        print("started waitAndSpawn");
        yield return new WaitForSeconds(0.5f);
        PrefabManager.Instance.SpawnCatapault();

        print("should have spawned new catapault");

        //// Makes a new projectile after a certain time
        //yield return new WaitForSeconds(0.5f);
        //var newAmmo = GameObject.Instantiate(Asteroid, startLocation, Quaternion.identity);

        //// Setting up new ammo variables
        //newAmmo.GetComponent<SpringJoint2D>().connectedBody = connectedBody;
        //newAmmo.GetComponent<ProjectileDragging>().catapult = connectedBody.transform;
        //newAmmo.GetComponent<ProjectileDragging>().spring = newSpring;
        //newAmmo.GetComponent<ProjectileDragging>().MaxStretch = this.MaxStretch;
        //newAmmo.GetComponent<ProjectileDragging>().Asteroid = this.Asteroid;
        //newAmmo.GetComponent<ProjectileDragging>().CatapultLineBack = CatapultLineBack;
        //newAmmo.GetComponent<ProjectileDragging>().CatapultLineFront = CatapultLineFront;
        //newAmmo.GetComponent<ProjectileDragging>().materials = materials;
        //newAmmo.GetComponent<ProjectileDragging>().CatapultLineBack.material = materials[0];
        //newAmmo.GetComponent<ProjectileDragging>().CatapultLineFront.material = materials[0];
    }

    IEnumerator waitAndDestroy()
    {
        yield return new WaitForSeconds(3.0f); //amount of seconds to wait to destroy projectile after firing
        Destroy(gameObject);
        Destroy(ParentCatapault);
    }
}
