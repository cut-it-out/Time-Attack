using UnityEngine;

namespace TimeAttack
{
    public static class Helpers
    {
        public static string GetFormattedTimer(float input)
        {
            string minutes = Mathf.Floor(input / 60).ToString("00");
            string seconds = (input % 60).ToString("00");

            return string.Format("{0}:{1}", minutes, seconds);
        }
    }
}
