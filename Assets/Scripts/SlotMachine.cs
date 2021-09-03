using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [RequireComponent(typeof(SlotPoolComponent))]
    [RequireComponent(typeof(SlotRandomManagerComponent))]
    [RequireComponent(typeof(SaveManagerComponent))]
    [RequireComponent(typeof(SlotFxComponent))]
    public class SlotMachine : MonoBehaviour
    {
        [SerializeField] private List<ColumnSpinnerData> columnData;
        [SerializeField] private GameObject spinButton;
        [SerializeField] [Range(0, 0.8f)] private float eachColumnSpinStartDelayDurationMin;
        [SerializeField] [Range(0, 0.8f)] private float eachColumnSpinStartDelayDurationMax;

        [SerializeField] private float imageVerticalSize = 260;

        private SaveManagerComponent saveManagerComponent;
        private SlotFxComponent slotFxComponent;
        private SlotRandomManagerComponent slotRandomManagerComponent;
        private SlotPoolComponent slotPoolComponent;

        private int stopppedColumnCount;
        private bool enableSpinning = false;

        private SymbolSequenceProbabilityData selectedSequence = null;


        private void Awake()
        {
            GetComponents();
            AwakeComponents();
            InjectColumnDependencies();
            AwakenSpinners();
        }

        private void GetComponents()
        {
            slotFxComponent = GetComponent<SlotFxComponent>();
            slotPoolComponent = GetComponent<SlotPoolComponent>();
            saveManagerComponent = GetComponent<SaveManagerComponent>();
            slotRandomManagerComponent = GetComponent<SlotRandomManagerComponent>();
        }

        private void AwakeComponents()
        {
            slotPoolComponent.AwakeComponent();
            saveManagerComponent.AwakeComponent();
            slotRandomManagerComponent.AwakeComponent(saveManagerComponent);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.2f);
            enableSpinning = true;
        }

        private void InjectColumnDependencies()
        {
            for (int i = 0; i < columnData.Count; i++)
            {
                if (columnData[i].column)
                {
                    columnData[i].column.SetPool(slotPoolComponent.GetPool());

                    SpinnerManager spinnerManager = new SpinnerManager(imageVerticalSize, columnData.Count, i, columnData[i].spinnersWithCondition);

                    columnData[i].column.SetSpinnerManager(spinnerManager);
                }
            }
        }

        private void AwakenSpinners()
        {
            for (int i = 0; i < columnData.Count; i++)
            {
                if (columnData[i].column)
                {
                    columnData[i].column.AwakeSpinner(imageVerticalSize);
                }
            }
        }

        private void UpdateSpinners()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < columnData.Count; i++)
            {
                if (columnData[i].column)
                {
                    columnData[i].column.UpdateSpinner(deltaTime);
                }
            }
        }

        void Update()
        {
            if (enableSpinning == true) return;

            UpdateSpinners();
        }

        public void StartSpinning()
        {
            if (enableSpinning == false) return;

            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            if (slotRandomManagerComponent == null) yield break;

            selectedSequence = slotRandomManagerComponent.PullRandomSequence();

            if (selectedSequence == null) yield break;
            if (selectedSequence.symbols.Count != columnData.Count) yield break;

            stopppedColumnCount = 0;
            enableSpinning = false;
            if (spinButton) spinButton.gameObject.SetActive(false);

            for (int i = 0; i < columnData.Count && i < selectedSequence.symbols.Count; i++)
            {
                if (columnData[i].column)
                {
                    float delay = Random.Range(eachColumnSpinStartDelayDurationMin, eachColumnSpinStartDelayDurationMax);
                    columnData[i].column.StartSpinning(selectedSequence.symbols[i], selectedSequence.symbols, OnColumnStop);

                    yield return new WaitForSeconds(delay);
                }
            }
        }

        private void OnColumnStop()
        {
            if (columnData != null)
            {
                stopppedColumnCount++;
                if (columnData.Count == stopppedColumnCount)
                {
                    OnSlotMachineStop();
                }
            }
        }

        private void OnSlotMachineStop()
        {
            if (selectedSequence == null) return;

            if (selectedSequence.spawnFxOnFinish && slotFxComponent)
            {
                slotFxComponent.SpawnCoinFx(selectedSequence.fxAmountMultiplier);
                Invoke(nameof(SetEnableSpinning), slotFxComponent.GetDuration());
                Invoke(nameof(EnableSpinButton), slotFxComponent.GetDuration());
            }
            else
            {
                SetEnableSpinning();
                EnableSpinButton();
            }

            saveManagerComponent.SaveGame(slotRandomManagerComponent);
        }


        private void SetEnableSpinning()
        {
            enableSpinning = true;
        }

        private void EnableSpinButton()
        {
            if (spinButton) spinButton.gameObject.SetActive(true);
        }

    }
}