using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    // 3
    public GameObject ammoPrefab;
    // 4
    static List<GameObject> ammoPool;
    // 5
    public int poolSize;

    public float weaponVelocity;

    // 1
    bool isFiring;
    // 2
    [HideInInspector]
    public Animator animator;
    // 3
    Camera localCamera;
    // 4
    float positiveSlope;
    float negativeSlope;

    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    void Start()
    {
        // 1
        animator = GetComponent<Animator>();
        // 2
        isFiring = false;
        // 3
        localCamera = Camera.main;

        // 1
        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        // 2
        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    // 6
    void Awake()
    {
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }
        // 8
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }
    // 1
    void Update()
    {
        // 2
        if (Input.GetMouseButtonDown(0))
        {
            // 3
            isFiring = true;
            FireAmmo();
        }
        UpdateState();
    }

    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        return HigerThanSlopeLine(inputPosition, positiveSlope);
    }

    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        return HigerThanSlopeLine(inputPosition, negativeSlope);
    }

    bool HigerThanSlopeLine(Vector2 inputPosition, float slope)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (slope * playerPosition.x);
        float inputIntercept = mousePosition.y - (slope * mousePosition.x);
        return inputIntercept > yIntercept;
    }
    // 1
    Quadrant GetQuadrant()
    {

        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);
        // 4
        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }


    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }
    // 4
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            // 2
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                // 4
                print($"shoot location {location.x} {location.y} {location.z}");
                ammo.transform.position = new Vector3(location.x, location.y, location.z);
                // 5
                return ammo;
            }
        }
        return null;
    }

    void UpdateState()
    {
        // 1
        if (isFiring)
        {
            // 2
            Vector2 quadrantVector;
            // 3
            Quadrant quadEnum = GetQuadrant();
            // 4
            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 1.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            // 6
            animator.SetBool("isFiring", true);
            // 7
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);
            // 8
            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }
    // 5
    void FireAmmo()
    {
        //Because the mouse uses Screen Space, we convert the mouse position from Screen Space to World Space.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 2
        GameObject ammo = SpawnAmmo(transform.position);
        // 3
        if (ammo != null)
        {
            // 4
            Arc arcScript = ammo.GetComponent<Arc>();

            if (arcScript == null || mousePosition == null)
            {
                print("arcScript is null");
            }

            // For example, 1.0 / 2.0 = 0.5, so the Ammo will take half a second to travel across the screen to its destination
            float travelDuration = 1.0f / weaponVelocity;
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    void OnDestroy()
    {
        ammoPool = null;
    }
}