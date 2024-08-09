using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This script allows for any given object to have 
/// characteristics of a bomb / explosive. The script
/// allows for object to pushed by the explosion force,
/// and for the force to fall off over a distance. The 
/// script also allows for a cluster type or recursive 
/// type explosive. If you wish to create a "recursive 
/// cluster" where each cluster create its own set of 
/// clusters, simply put a prefab with "isClusterBomb" 
/// enabled.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class UniversalExplosive : MonoBehaviour {
    #region Universal Explosive Properties
    private enum TriggerTypes { Timer, Trigger }

    [Header("Explosive Properties")]
    [Tooltip("Method of activating the explosive object.")]
    [SerializeField] TriggerTypes explosiveTrigger = TriggerTypes.Trigger;
    [Tooltip("Amount of time before object explodes.")]
    [SerializeField][ConditionalEnumHide("explosiveTrigger", (int)TriggerTypes.Timer)] float timerDelay = 1f;
    [Tooltip("Force of explosion effecting other rigidbodies.")]
    [SerializeField][MinValue(0)] float explosiveForce = 1f;
    [Tooltip("Explosion radius effecting rigidbodies within range.")]
    [SerializeField][MinValue(0)] float explosiveRange = 1f;
    [Tooltip("Explosion audio.")]
    [SerializeField][NotNullable] AudioSource explosionAudio = null;
    [Tooltip("Explosion particle effects.")]
    [SerializeField][NotNullable] ParticleSystem explosionEffects = null;
    [Space(5)][LineDivider(2, LineColors.Black)]
    [Tooltip("Allow for the explosion force to fall off over a distance.")]
    [SerializeField] bool explosiveFallOff = false;
    [Tooltip("Curve of the explosion force fall off.")]
    [SerializeField] AnimationCurve fallOffCurve = null;
    [Space(5)][LineDivider(2, LineColors.Black)]
    [Tooltip("Explosive objects creates more explosive object around it.")]
    [SerializeField] bool isClusterBomb = false;
    [Tooltip("Dealy before next cluster bombs explode.")]
    [SerializeField][ShowIf("isClusterBomb", true)] float clusterDelay = 0;
    [Tooltip("Cluster distance from originating explosive.")]
    [SerializeField][ShowIf("isClusterBomb", true)] float clusterRange = 0;
    [Tooltip("Number of additional explosives object created around this object.")]
    [SerializeField][ShowIf("isClusterBomb", true)] int numberOfCluster = 0;
    [Tooltip("Cluster bomb that will be instantiated by inital bomb.")]
    [SerializeField][ShowIf("isClusterBomb", true)] GameObject clusterBombPrefab = null;
    [Space(5)][LineDivider(2, LineColors.Black)]
    [Tooltip("Allow for object to explode more than once.")]
    [SerializeField] bool isRecursiveBomb = false;
    [Tooltip("The time between each recursive explosion.")]
    [SerializeField][ShowIf("isRecursiveBomb", true)] float recursiveDelay = 0;
    [Tooltip("The numer of times the object will explode, including inital explosion.")]
    [SerializeField][ShowIf("isRecursiveBomb", true)] int numberOfRecursion = 1;


    bool NotTriggered;
    int RecurringTrigger;
    MeshRenderer MeshRender;
    SphereCollider ExplosionVolume;
    List<Rigidbody> ExternalBodies;
    float CurrentTimer, CurrentRecursiveDelay;

    [ExecuteAlways]
    private void OnValidate() {
        ExplosionVolume = GetComponent<SphereCollider>();

        ExplosionVolume.isTrigger = true;
        ExplosionVolume.radius = explosiveRange;

        if (clusterDelay < 0) {
            clusterDelay = 0;
        }
        if (clusterRange < 0) {
            clusterRange = 0;
        }
        if (numberOfCluster < 1) {
            numberOfCluster = 1;
        }
        if (numberOfRecursion < 1) {
            numberOfRecursion = 1;
        }
    }
    #endregion

    #region Getters & Setters Methods
    /// <summary>
    /// Get the range of the area to affect objects by the explosion force.
    /// </summary>
    /// <returns></returns>
    public float GetExplosiveRange() {
        return explosiveRange;
    }
    /// <summary>
    /// Set the range of the area to affect objects by the explosion force.
    /// </summary>
    /// <param name="NewExplosiveRange"></param>
    public void SetExplosiveRange(float NewExplosiveRange) {
        explosiveRange = NewExplosiveRange;
    }

    /// <summary>
    /// Get the force created by the explosion.
    /// </summary>
    /// <returns></returns>
    public float GetExplosiveForce() {
        return explosiveForce;
    }
    /// <summary>
    /// Set the force created by the explosion.
    /// </summary>
    /// <param name="NewExplosiveForce"></param>
    public void SetExplosiveForce(float NewExplosiveForce) {
        explosiveForce = NewExplosiveForce;
    }

    /// <summary>
    /// Gets the boolean value of explosiveFallOff.
    /// </summary>
    /// <returns></returns>
    public bool GetFallOffBool() {
        return explosiveFallOff;
    }
    /// <summary>
    /// Set the boolean value of esplosiveFallOff.
    /// </summary>
    /// <param name="value"></param>
    public void SetFallOffBool(bool value) {
        explosiveFallOff = value;
    }

    /// <summary>
    /// Gets the boolean value of isClusterBomb.
    /// </summary>
    /// <returns></returns>
    public bool GetIsClusterBombBool() {
        return isClusterBomb;
    }
    /// <summary>
    /// Set the boolean value of isClusterBomb..
    /// </summary>
    /// <param name="value"></param>
    public void SetIsClusterBombBool(bool value) {
        isClusterBomb = value;
    }
    /// <summary>
    /// Get the spread range of the cluster bomb.
    /// </summary>
    /// <returns></returns>
    public float GetClusterRange() {
        return clusterRange;
    }
    /// <summary>
    /// Set the spread range of the cluster bomb,
    /// </summary>
    /// <param name="NewRange"></param>
    public void SetClusterRange(float NewRange) {
        clusterRange = NewRange;
    }
    /// <summary>
    /// Get the number of cluster created from inital Bomb.
    /// </summary>
    /// <returns></returns>
    public int GetClusterAmount() {
        return numberOfCluster;
    }
    /// <summary>
    /// Set the number of cluster to be created from the inital bomb,
    /// </summary>
    /// <param name="NewAmount"></param>
    public void SetClusterAmount(int NewAmount) {
        numberOfCluster = NewAmount;
    }

    /// <summary>
    /// Get the boolean value if the bomb is recursive.
    /// </summary>
    /// <returns></returns>
    public bool GetIsRecursiveBomb() {
        return isRecursiveBomb;
    }
    /// <summary>
    /// Set the boolean value if th ebomb is recursive.
    /// </summary>
    /// <param name="IsRecursive"></param>
    public void SetIsRecursiveBomb(bool IsRecursive) {
        isRecursiveBomb = IsRecursive;
    }
    /// <summary>
    /// Get the delay between each recursive explosion.
    /// </summary>
    /// <returns></returns>
    public float GetRecursiveDelay() {
        return recursiveDelay;
    }
    /// <summary>
    /// Set the delay between each recursive explosion.
    /// </summary>
    /// <param name="NewRecursiveDelay"></param>
    public void SetRecursiveDelay(float NewRecursiveDelay) {
        recursiveDelay = NewRecursiveDelay;
    }
    /// <summary>
    /// Get the amount of times the bomb wi explode.
    /// </summary>
    /// <returns></returns>
    public int GetRecrusiveAmount() {
        return numberOfRecursion;
    }
    /// <summary>
    /// Set the amount of times the bomb will explode.
    /// </summary>
    /// <param name="RecursionTimes"></param>
    public void SetRecursiveAmount(int RecursionTimes) { 
        numberOfRecursion = RecursionTimes;
    }
    #endregion
    private void Awake()
    {
        NotTriggered = false;
        ExternalBodies = new List<Rigidbody>();
        MeshRender = GetComponent<MeshRenderer>();
        ExplosionVolume = GetComponent<SphereCollider>();

        ExplosionVolume.isTrigger = true;
        ExplosionVolume.radius = explosiveRange;

        if (isRecursiveBomb)
        {
            RecurringTrigger = numberOfRecursion + 1;
        }
        else
        {
            RecurringTrigger = 1;
        }
    }

    /// <summary>
    /// Activating or triggering explosion script.
    /// </summary>
    public void TriggerExplosion() {
        if (!NotTriggered) {
            switch (explosiveTrigger) {
                case TriggerTypes.Timer:
                    CurrentTimer = timerDelay;
                    StartCoroutine(Explode());
                    break;
                case TriggerTypes.Trigger:
                    StartCoroutine(Explode());
                    break;
            }
        }
        NotTriggered = true;
    }

    IEnumerator Explode() {
        // Timer delay before explosion
        while (CurrentTimer > 0) {
            CurrentTimer -= Time.deltaTime;
            yield return null;
        }

        for (int a = 0; a < RecurringTrigger; a ++) {
            while (CurrentRecursiveDelay > 0) {
                CurrentRecursiveDelay -= Time.deltaTime;
                yield return null;
            }

            if (explosionEffects != null) {
                explosionEffects.Play();
            }
            if (explosionAudio != null) {
                explosionAudio.Play();
            }
            if (a >= RecurringTrigger) {
                MeshRender.enabled = false;
            }

            // All rigidbodies will be pushed away 
            if (ExternalBodies.Count > 0) {
                foreach (Rigidbody RB in ExternalBodies) {
                    Vector3 Direction = RB.transform.position - transform.position;
                    if (explosiveFallOff) {
                        float ForcePercent = (RB.transform.position - transform.position).magnitude / explosiveRange;
                        RB.AddForce(Direction * (explosiveForce * fallOffCurve.Evaluate(ForcePercent)));
                    }
                    else {
                        RB.AddForce(Direction * explosiveForce);
                    }
                }
            }

            if (isClusterBomb) {
                List<GameObject> ClusterBits = new List<GameObject>();
                for (int i = 0; i < numberOfCluster; i++) {
                    float Angle = i * Mathf.PI * 2 / numberOfCluster;
                    float X_Pos = Mathf.Cos(Angle) * clusterRange;
                    float Z_Pos = Mathf.Sin(Angle) * clusterRange;
                    Vector3 ClusterPosition = transform.position + new Vector3(X_Pos, 0, Z_Pos);

                    float ClusterAngle = -Angle * Mathf.Rad2Deg;
                    Quaternion ClusterRotation = Quaternion.Euler(0, ClusterAngle, 0);

                    GameObject ClusterBomb = Instantiate(clusterBombPrefab, ClusterPosition, ClusterRotation);
                    ClusterBits.Add(ClusterBomb);
                }

                float CurrentClusterDelay = clusterDelay;
                while (CurrentClusterDelay > 0) {
                    CurrentClusterDelay -= Time.deltaTime;
                    yield return null;
                }

                foreach (GameObject Clusteroid in ClusterBits) {
                    Clusteroid.GetComponent<UniversalExplosive>().TriggerExplosion();
                }
            }

            CurrentRecursiveDelay = recursiveDelay;
        }

        float EffectDelay = explosionEffects.main.duration;
        while (EffectDelay > 0) {
            EffectDelay -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider Object) {
        if (Object.GetComponent<Rigidbody>() != null) { 
            ExternalBodies.Add(Object.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider Object) {
        if (ExternalBodies.Count > 0) {
            ExternalBodies.Remove(Object.GetComponent<Rigidbody>());
        }
    }
}