using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerService : MonoBehaviour
{
    private List<TimerContainer> timers = new List<TimerContainer>();

    public void AddTimer(float duration, Action method)
    {
	timers.Add(new TimerContainer(){EndTime = DateTime.Now.AddSeconds(duration), FinishMethod = method});
    }

    /// <summary>
    /// ConstantMethod is called each frame for duration.
    /// FinishMethod is called once at the end (optional param).
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="constantMethod"></param>
    /// <param name="finishMethod"></param>
    public void AddConstantTimer(float duration, Action<float> constantMethod, Action finishMethod = null)
    {
	StartCoroutine(ConstantTimer(duration, constantMethod, finishMethod));
    }

    public void CancelAllTimers()
    {
        StopAllCoroutines();
        timers.Clear();
    }

    private IEnumerator ConstantTimer(float duration, Action<float> constantMethod, Action finishMethod)
    {
	float timeLeft = duration;
	while (true)
	{
	    timeLeft -= Time.deltaTime;

	    if (timeLeft > 0)
	    {
		constantMethod.Invoke(timeLeft);
		yield return new WaitForEndOfFrame();
	    }
	    else
	    {
		finishMethod?.Invoke();

		break;
	    }
	}
    }

    private void Update()
    {
	CheckFirstTimer();
    }

    private void CheckFirstTimer()
    {
	if (timers.Count == 0)
	{
	    return;
	}

	if (timers[0].EndTime >= DateTime.Now)
	{
	    return;
	}
	timers[0].FinishMethod?.Invoke();
	timers.RemoveAt(0);
	CheckFirstTimer();
    }


    private void InsertToTimerList(TimerContainer container)
    {
	TimerContainerComparer TCC = new TimerContainerComparer();
	timers.Add(container);
	timers.Sort(TCC);
    }
}

public struct TimerContainer
{
    public DateTime EndTime;
    public Action FinishMethod;
}


public class TimerContainerComparer : IComparer<TimerContainer>
{
    public int Compare(TimerContainer x, TimerContainer y)
    {
	return x.EndTime.CompareTo(y.EndTime);
    }
}
