using UnityEngine;
using System.Collections.Generic;

/// <summary> 
/// This script provides any information that the 
/// Gravity Manipulator script needs to apply the
/// physics upon object.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour {
    #region Gravity Body Properties
    public bool enableGravity = true;

    Rigidbody ObjectBody;
    Transform ObjectTransform;
    public List<GravityManipulator> GravityManipulators = new List<GravityManipulator>();
    #endregion

    /// <summary>
    /// Initalize with Start().
    /// </summary>
    public void InitalizeGravBody() {
        ObjectBody = GetComponent<Rigidbody>();
        ObjectBody.useGravity = false;
        ObjectTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// Must be used during FixedUpdate() for gravitation force to take affect.
    /// </summary>
    public void GravBodyFixedUpdate() {
        if (enableGravity && GravityManipulators != null) {
            foreach (GravityManipulator manipulator in GravityManipulators) {
                manipulator.ApplyGravity(ObjectBody, ObjectTransform);
            }
        }
    }

    #region Add and Remove GravityManipulators to and from the list
    public void AddManipulator(GravityManipulator GravitySource) {
        if (!GravityManipulators.Contains(GravitySource)) {
            GravityManipulators.Add(GravitySource);
        }
    }
    public void RemoveManipulator(GravityManipulator GravitySource) {
        GravityManipulators.Remove(GravitySource);
    }
    #endregion
}