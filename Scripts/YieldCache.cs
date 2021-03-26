using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------------------------------------------------
 * Date: 오후 5:59 2019-06-12
 * Name: YieldCache
 * InObj: 
 * DESC: 코루틴 최적화에 사용..
 * Useage: 
    yield return YieldCache.WaitForSeconds(0.1f);
    yield return YieldCache.WaitForSeconds(1f);
 * History:
 * [author] max.han@me2on.com
-----------------------------------------------------------------------------*/
internal static class YieldCache
{
    #region [ Data ]

    class FloatComparer : IEqualityComparer<float>
    {
        public bool Equals(float x, float y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();       //코루틴 WaitForEndOfFrame..
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();    //코루틴 WaitForFixedUpdate..

    private static readonly Dictionary<float, WaitForSeconds> m_TimeOffset = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    #endregion

    /// <summary>
    /// 코루틴 WaitForSeconds함수 사용시 캐싱해서 사용..
    /// </summary>
    /// <param name="seconds">대기 시간</param>
    /// <returns></returns>
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds waitForSeconds = null;

        if (!m_TimeOffset.TryGetValue(seconds, out waitForSeconds))
        {
            waitForSeconds = new WaitForSeconds(seconds);
            m_TimeOffset.Add(seconds, waitForSeconds);
        }

        return waitForSeconds;
    }
}
