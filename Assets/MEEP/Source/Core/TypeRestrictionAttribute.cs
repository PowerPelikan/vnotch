using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class TypeRestrictionAttribute : PropertyAttribute
{

    public Type targetType;

    public TypeRestrictionAttribute(Type targetType)
    {
        this.targetType = targetType;
    }

}
