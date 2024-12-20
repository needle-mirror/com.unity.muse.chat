using System.Collections.Generic;
using UnityEngine;

namespace Unity.Muse.Chat
{
    internal static class ContextUtils
    {
        public static void RemoveInvalidObjects(List<Object> objectList)
        {
            if (objectList == null || objectList.Count == 0)
                return;

            for (var i = objectList.Count - 1; i>= 0; i--)
            {
                if (objectList[i] == null)
                {
                    objectList.RemoveAt(i);
                }
            }
        }
    }
}
