using System;
using UnityEngine;

namespace ByteSizedAttributes { 
    public interface ByteSizedAttributes { }

    public class ValidatorAttribute : PropertyAttribute, ByteSizedAttributes { }

    public class DrawerAttribute : PropertyAttribute, ByteSizedAttributes { }
}