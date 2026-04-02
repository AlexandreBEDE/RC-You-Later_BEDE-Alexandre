using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class Timer
{
    private static readonly Stopwatch stopwatch = new();
    private static readonly List<long> steps = new();
    private static readonly List<long> savedSteps = new();


    public static bool IsRunning
    {
        get => stopwatch.IsRunning;
    }

    public static double ElapsedSeconds
    {
        get => stopwatch.ElapsedMilliseconds * 0.001f;
    }

    public static int StepsCount
    {
        get => steps.Count;
    }
    public static int SavedStepsCount => savedSteps.Count;

    

    public static double GetStepElapsedSeconds(int index)
    {
        return steps[index] * 0.001f;
    }
    public static double GetSavedStepElapsedSeconds(int index)
    {
        return savedSteps[index] * 0.001f;
    }

    /// <summary>
    /// Reset the timer and remove any steps.
    /// </summary>
    public static void Reset()
    {
        stopwatch.Reset();
        steps.Clear();
        //On ne clear pas les savedSteps car ce sont les steps chargés depuis le fichier, et on veut les garder pour les afficher au joueur même après un reset.
    }

    public static void Start()
    {
        stopwatch.Start();
    }

    public static void Stop()
    {
        stopwatch.Stop();
    }

    public static void Step()
    {
        steps.Add(stopwatch.ElapsedMilliseconds);
    }

    public static void Save()
    {
        // TODO : save our time steps (line 7 of this script) inside a file.
        string path = Application.dataPath + "/Score.txt";

        // Le temps final est le dernier step enregistré
        long currentTime = steps[steps.Count - 1];

        // On vérifie s'il y a déjà un score sauvegardé
        if (File.Exists(path) && !string.IsNullOrEmpty(File.ReadAllText(path)))
        {
            string existingJson = File.ReadAllText(path);
            StepsWrapper existingWrapper = JsonUtility.FromJson<StepsWrapper>(existingJson);

            if (existingWrapper != null && existingWrapper.steps != null && existingWrapper.steps.Count > 0)
            {
                // Le temps final sauvegardé est aussi le dernier step
                long savedTime = existingWrapper.steps[existingWrapper.steps.Count - 1];

                // On sauvegarde seulement si le temps actuel est meilleur
                if (currentTime >= savedTime) return;
            }
        }

        // Aucun record ou nouveau record : on sauvegarde !
        string json = JsonUtility.ToJson(new StepsWrapper { steps = steps });
        File.WriteAllText(path, json);
    }

    public static void Load()
    {
        // TODO : load our time steps from a file (if we have any)
        // and store them inside our steps variable (line 7 of this script)
        // to show them to the player before starting a race.
        string path = Application.dataPath + "/Score.txt";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // On vérifie que le fichier n'est pas vide
            if (string.IsNullOrEmpty(json)) return;

            StepsWrapper wrapper = JsonUtility.FromJson<StepsWrapper>(json);

            // On vérifie que la désérialisation a bien fonctionné
            if (wrapper == null || wrapper.steps == null) return;
            {
                // mis en commentaire pour éviter que le son "bad time" soit joué dès le lancement
                //steps.Clear();
                //steps.AddRange(wrapper.steps);

                savedSteps.Clear();
                savedSteps.AddRange(wrapper.steps);
            }
        }

    }

    [System.Serializable]
    public class StepsWrapper
    {
        public List<long> steps;
    }
}
