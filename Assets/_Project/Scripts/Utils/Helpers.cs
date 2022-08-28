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

        public static void ShiftArrayContent<T>(ref T[] array, int shiftCount)
        {
            T[] backupArray = new T[array.Length];

            for (int index = 0; index < array.Length; index++)
            {
                backupArray[(index + array.Length + shiftCount % array.Length) % array.Length] = array[index];
            }

            array = backupArray;
        }

    }
}
