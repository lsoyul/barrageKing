using PD.UnityEngineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestExtensions
{
    public static void ThrowIfNull(this object nullable)
    {
        if (nullable.IsNull())
        {
            throw new ArgumentNullException($"{nullable.ToString()} is null");
        }
    }

    public static void ThrowIfEqual(this object checkable, object comparisionTarget)
    {
        if (checkable.Equals(comparisionTarget))
        {
            throw new InvalidValueException($"{checkable.ToString()} must not equal  {comparisionTarget.ToString()}");
        }
    }

    public static void ThrowIfNotEqual(this object checkable, object comparisionTarget)
    {
        if (!checkable.Equals(comparisionTarget))
        {
            throw new InvalidValueException($"{checkable.ToString()} must equal  {comparisionTarget.ToString()}");
        }
    }

    public static void ThrowIfTrue(this bool condition)
    {
        if (condition)
        {
            throw new InvalidConditionException($"condition is true, condition must be not true");
        }
    }

    public static void ThrowIfFalse(this bool condition)
    {
        if (!condition)
        {
            throw new InvalidConditionException($"condition is false, condition must be not false");
        }
    }

    public static void ThrowIfInRange<T>(this T comparable, T minRange, T maxRange) where T: IComparable
    {
        if (comparable.CompareTo(minRange) >= 0 && comparable.CompareTo(maxRange) <= 0)
        {
            throw new ArgumentOutOfRangeException($"{comparable.ToString()} is in range {minRange.ToString()} ~ {maxRange.ToString()}");
        }
    }

    public static void ThrowIfNotInRange<T>(this T comparable, T minRange, T maxRange) where T : IComparable
    {
        if (comparable.CompareTo(minRange) < 0 || comparable.CompareTo(maxRange) > 0)
        {
            throw new ArgumentOutOfRangeException($"{comparable.ToString()} is not in range {minRange.ToString()} ~ {maxRange.ToString()}");
        }
    }
}

public class InvalidValueException : Exception
{
    public InvalidValueException() : base() { }
    public InvalidValueException(string message) : base(message) { }
    public InvalidValueException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected InvalidValueException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class InvalidConditionException : Exception
{
    public InvalidConditionException() : base() { }
    public InvalidConditionException(string message) : base(message) { }
    public InvalidConditionException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected InvalidConditionException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

