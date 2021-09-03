using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [System.Serializable]
    public class SpinnerManager
    {
        private List<SpinnerConditionHelper> spinnersWithCondition;
        private int index = 0;
        private int scrollCount = 0;
        private int totalColumns = 0;
        private int myColumnIndex = 0;
        private int spinnerIndexLimit = 0;
        private float imageVerticalSize = 0;
        public SpinnerManager(float imageVerticalSize, int totalColumns, int myColumnIndex, List<SpinnerConditionHelper> spinnersData)
        {
            spinnersWithCondition = new List<SpinnerConditionHelper>();
            this.imageVerticalSize = imageVerticalSize;
            this.totalColumns = totalColumns;
            this.myColumnIndex = myColumnIndex;
            this.spinnersWithCondition = spinnersData.ConvertAll(spinnerData => new SpinnerConditionHelper(spinnerData));
        }

        public void Reset(List<SlotSymbolTypes> selectedSymbols)
        {
            spinnerIndexLimit = 0;
            index = 0;
            scrollCount = 0;
            for (int i = 0; i < spinnersWithCondition.Count; i++)
            {
                if (spinnersWithCondition[i] != null)
                {
                    SpinnerSO spinner = spinnersWithCondition[i].SelectSpinnerBasedOnCondition(totalColumns, myColumnIndex, selectedSymbols);
                    if (spinner)
                    {
                        spinnerIndexLimit++;
                        spinner.ResetPositionHandler(imageVerticalSize);
                        scrollCount += spinner.GetScrollCount();
                    }
                }
            }

        }

        public int GetScrollCountToStop()
        {
            return scrollCount;
        }
        public float Spin(float deltaTime)
        {
            if (IsSpinningEnded() || spinnersWithCondition[index] == null) return 0;
            SpinnerSO spinner = spinnersWithCondition[index].GetSelectedSpinner();
            float offset = 0;
            if (spinner)
            {
                offset = spinner.UpdatePosition(deltaTime);

                if (spinner.IsTimeUp())
                {
                    index++;
                }
            }

            return offset;
        }


        public bool IsSpinningEnded()
        {
            return index >= spinnerIndexLimit;
        }

    }
}