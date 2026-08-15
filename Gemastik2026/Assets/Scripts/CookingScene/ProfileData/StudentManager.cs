using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [Header("Students")]
    public List<StudentSO> healthyStudents;
    public List<StudentSO> overweightStudents;
    public List<StudentSO> proteinStudents;
    public List<StudentSO> fatStudents;

    [Header("UI")]
    public StudentProfileUI profileUI;

    [Header("Rounds Configuration")]
    [Tooltip("Jumlah siswa dari kategori mayoritas")]
    public int majorityStudentCount = 3;

    [Tooltip("Jumlah siswa acak dari kategori lain")]
    public int randomStudentCount = 1;

    public StudentSO CurrentStudent { get; private set; }

    private List<StudentSO> todaysStudents = new List<StudentSO>();
    private int currentIndex;

    private void Start()
    {
        // The day has already been chosen by the Town scene.
        // Generate today's four students.
        GenerateStudents();
    }

    public void GenerateStudents()
    {
        todaysStudents.Clear();
        currentIndex = 0;

        NutritionProblem majority = DayManager.Instance.currentProblem;
        List<StudentSO> majorityList = new List<StudentSO>(GetList(majority));

        // Gunakan variabel majorityStudentCount alih-alih angka 3
        for (int i = 0; i < majorityStudentCount && majorityList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, majorityList.Count);
            todaysStudents.Add(majorityList[randomIndex]);
            majorityList.RemoveAt(randomIndex);
        }

        // Ambil siswa acak sesuai jumlah randomStudentCount
        for (int i = 0; i < randomStudentCount; i++)
        {
            NutritionProblem randomProblem;
            do
            {
                randomProblem = (NutritionProblem)Random.Range(0, System.Enum.GetValues(typeof(NutritionProblem)).Length);
            }
            while (randomProblem == majority);

            List<StudentSO> randomList = GetList(randomProblem);

            if (randomList.Count > 0)
            {
                todaysStudents.Add(randomList[Random.Range(0, randomList.Count)]);
            }
        }

        ShuffleTodayStudents();
        NextStudent();
    }

    private List<StudentSO> GetList(NutritionProblem problem)
    {
        switch (problem)
        {
            case NutritionProblem.Healthy:
                return healthyStudents;

            case NutritionProblem.OverweightMalnutrition:
                return overweightStudents;

            case NutritionProblem.ProteinMalnutrition:
                return proteinStudents;

            case NutritionProblem.FatMalnutrition:
                return fatStudents;

            default:
                return healthyStudents;
        }
    }

    private void ShuffleTodayStudents()
    {
        for (int i = 0; i < todaysStudents.Count; i++)
        {
            int randomIndex = Random.Range(i, todaysStudents.Count);

            StudentSO temp = todaysStudents[i];
            todaysStudents[i] = todaysStudents[randomIndex];
            todaysStudents[randomIndex] = temp;
        }
    }

    public void NextStudent()
    {
        if (!HasMoreStudents())
            return;

        CurrentStudent = todaysStudents[currentIndex];
        currentIndex++;

        profileUI.Display(CurrentStudent);
    }

    public bool HasMoreStudents()
    {
        return currentIndex < todaysStudents.Count;
    }
}