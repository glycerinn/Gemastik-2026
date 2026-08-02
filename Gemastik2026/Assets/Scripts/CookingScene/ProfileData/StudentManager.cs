using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [Header("Students")]
    public List<StudentSO> students = new List<StudentSO>();

    [Header("UI")]
    public StudentProfileUI profileUI;

    public StudentSO CurrentStudent { get; private set; }

    private void Start()
    {
        NextStudent();
    }

    public void NextStudent()
    {
        if (students.Count == 0)
        {
            Debug.LogWarning("No students assigned!");
            return;
        }

        CurrentStudent = students[Random.Range(0, students.Count)];

        profileUI.Display(CurrentStudent);
    }
}