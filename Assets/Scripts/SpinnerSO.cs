using UnityEngine;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "SpeedHandler", menuName = "ScriptableObjects/SpinHandler", order = 2)]
    public class SpinnerSO : ScriptableObject
    {
        public float duration = 4;
        public float speedMult = 5;
        //[SerializeField] private float endSpeedMult = 5;
        public EaseTypes easeType;
        //private float currentSpeedMult = 0;
        //private float imageVerticalSize = 260;
        private float timer = 0;
        private float spinCurrentPositionY;
        private float destination = 0;
        //private float positionCovered;

        private int symbolCountToScroll;
        //private bool enableSpinning = false;


        public void ResetPositionHandler(float imageVerticalSize)
        {
            timer = 0;
            //positionCovered = 0;
            //currentSpeedMult = 0;
            spinCurrentPositionY = 0;

            symbolCountToScroll = Mathf.RoundToInt(duration * speedMult);
            //symbolCountToScroll = Mathf.FloorToInt(duration * (speedMult + endSpeedMult) / 2);

            destination = -symbolCountToScroll * imageVerticalSize;
            //enableSpinning = false;
        }

        public int GetScrollCount()
        {
            return symbolCountToScroll;
        }


        //public float UpdatePosition(float deltaTime)
        //{
        //    if (duration == 0.0f || enableSpinning) return 0;

        //    currentSpeedMult = EaseManager.GetEaseByType(easeType, speedMult, endSpeedMult, Mathf.Clamp(timer / duration, 0, 1));

        //    timer += deltaTime;

        //    float offset = -currentSpeedMult * imageVerticalSize * deltaTime;

        //    if (timer / duration >= 1)
        //    {
        //        offset = destination - positionCovered;
        //        positionCovered += offset;
        //        enableSpinning = true;
        //        return offset;
        //    }


        //    if (Mathf.Abs(positionCovered + offset) > Mathf.Abs(destination))
        //    {
        //        offset = destination - positionCovered;
        //    }

        //    positionCovered += offset;

        //    return offset;
        //}
        public bool IsTimeUp()
        {
            return timer >= duration;
        }
        
        

        public float UpdatePosition(float deltaTime)
        {
            if (duration == 0.0f) return 0;

            timer += deltaTime;

            float prevCyclePosY = spinCurrentPositionY;

            spinCurrentPositionY = EaseManager.GetEaseByType(easeType, 0, destination, Mathf.Clamp(timer / duration, 0, 1));

            return spinCurrentPositionY - prevCyclePosY;
        }


    }
}