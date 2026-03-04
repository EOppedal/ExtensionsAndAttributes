using System;
using System.Collections;
using UnityEngine;

public static class EnumeratorFunctions {
    public static IEnumerator WaitUntilCondition(Func<bool> condition, Action action) {
        yield return new WaitUntil(condition);
        action.Invoke();
    }

    public static IEnumerator ActionDuringTime(float time, Action action) {
        var startTime = Time.time;
        while (startTime + time > Time.time) {
            action.Invoke();
            Debug.Log("Action during");
            yield return null;
        }
    }

    public static IEnumerator ActionAfterTime(float time, Action action) {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    public static IEnumerator ActionAtInterval(float interval, Action action) {
        while (true) {
            action.Invoke();
            yield return new WaitForSeconds(interval);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}