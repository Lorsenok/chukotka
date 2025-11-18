using System.Collections;
using System.Linq;
using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(Animal))]
[RequireComponent(typeof(TargetFollower))]
[RequireComponent(typeof(Rigidbody2D))]
public class BirdAIController : MonoBehaviour
{
    [Header("Detection")]
    public Transform playerTransform;
    public float detectionRadius = 5f;

    [Header("Flight Settings")]
    public Transform[] escapePoints;
    public float flyUpVelocity = 3f;
    public float ascendTime = 0.25f;
    public float flightSpeedMultiplier = 2f;
    public float landingSmoothing = 2f;

    [Header("Restore")]
    public float restoreDelay = 0.3f;

    private Animal animal;
    private TargetFollower follower;
    private Rigidbody2D rb;

    private FieldInfo isEscapingField;
    private FieldInfo agrField;

    private bool isHandlingEscape = false;
    private bool isPanicked = false;

    void Awake()
    {
        animal = GetComponent<Animal>();
        follower = GetComponent<TargetFollower>();
        rb = GetComponent<Rigidbody2D>();

        var type = typeof(Animal);
        agrField = type.GetField("agr", BindingFlags.NonPublic | BindingFlags.Instance);
        isEscapingField = type.GetField("isEscaping", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    void Start()
    {
        escapePoints = GameObject.FindGameObjectsWithTag("EscapePoint")
                                 .Select(x => x.transform).ToArray();

        if (playerTransform == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTransform = p.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null || isHandlingEscape || isPanicked) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist < detectionRadius)
            StartEscapeRoutine();
    }

    private void StartEscapeRoutine()
    {
        if (isHandlingEscape || escapePoints.Length == 0) return;

        StartCoroutine(EscapeRoutine());
    }

    private IEnumerator EscapeRoutine()
    {
        isHandlingEscape = true;
        isPanicked = true;

        Transform targetPoint = ChooseNearestPoint();

      
        isEscapingField.SetValue(animal, false);
        agrField.SetValue(animal, true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float origSpeed = follower.SpeedMultiplier;
        follower.SpeedMultiplier = origSpeed * flightSpeedMultiplier;


        float endAscend = Time.time + ascendTime;
        while (Time.time < endAscend)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flyUpVelocity);
            yield return null;
        }


        follower.SetTarget(targetPoint);
        while (Vector2.Distance(transform.position, targetPoint.position) > 0.5f)
        {
            float desiredY = Mathf.Lerp(transform.position.y, targetPoint.position.y, landingSmoothing * Time.deltaTime);
            rb.linearVelocity = new Vector2(follower.SpeedMultiplier * Mathf.Sign(targetPoint.position.x - transform.position.x),
                                      (desiredY - transform.position.y) / Time.deltaTime);
            yield return null;
        }

        rb.gravityScale = originalGravity;
        follower.SpeedMultiplier = origSpeed;
        follower.SetTarget(transform); 
        follower.SetPassive(true);     
        agrField.SetValue(animal, true);

        yield return new WaitForSeconds(restoreDelay);

        isHandlingEscape = false;
        isPanicked = false;
    }

    private Transform ChooseNearestPoint()
    {
        Transform best = escapePoints[0];
        float bestDist = Vector2.Distance(transform.position, best.position);

        foreach (var p in escapePoints)
        {
            float d = Vector2.Distance(transform.position, p.position);
            if (d < bestDist)
            {
                best = p;
                bestDist = d;
            }
        }
        return best;
    }
}