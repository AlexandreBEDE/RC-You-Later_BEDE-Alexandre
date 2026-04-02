using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerLabel : MonoBehaviour
{
    private TextMeshProUGUI label = null;

    [SerializeField] AudioManager audioManager = null;
    private int lastPlayedStepIndex = -1;

    private void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        string text = $"Time: {Timer.ElapsedSeconds:F3} s";
        int stepsCount = Timer.StepsCount;
        int savedStepsCount = Timer.SavedStepsCount;

        if (stepsCount == 0)
        {
            lastPlayedStepIndex = -1;
        }

        for (int index = 0; index < stepsCount; index++)
        {
            double currentStep = Timer.GetStepElapsedSeconds(index);
            string line = $"\n{(index + 1)}. {currentStep:F3}";

            // Si on a un ancien temps pour cette porte
            if (index < savedStepsCount)
            {
                double savedStep = Timer.GetSavedStepElapsedSeconds(index);
                double diff = currentStep - savedStep;

                if (diff < 0)
                {
                    line += $" <color=green>({diff:F3})</color>"; // meilleur temps couleur verte
                    if (index > lastPlayedStepIndex) 
                    {
                        audioManager.PlayClip("GoodTime");
                        lastPlayedStepIndex = index;
                    }
                }
                else
                {
                    line += $" <color=red>(+{diff:F3})</color>"; // moins bon temps couleur rouge
                    if (index > lastPlayedStepIndex)
                    {
                        audioManager.PlayClip("BadTime");
                        lastPlayedStepIndex = index;
                    }
                }
            }

            text += line;
        }

        // Affiche les anciens temps si la course n'a pas encore commencé
        if (stepsCount == 0 && savedStepsCount > 0)
        {
            for (int index = 0; index < savedStepsCount; index++)
            {
                text += $"\n{(index + 1)}. {Timer.GetSavedStepElapsedSeconds(index):F3}";
            }
        }

        label.SetText(text);
    }
}
