using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class IdGenerator : MonoBehaviour
{
    private const int LETTER_NUMBER = 6;

    public static string CreateCustomID()
    {
        StringBuilder SB = new StringBuilder();
        SB.Clear();

        int NumOfExtraLetter = LETTER_NUMBER;
        for (int i = 0; i < NumOfExtraLetter; i++) { SB.Append(getRandomLetter()); }

        char getRandomLetter()
        {
            //����10��, �빮��26�� = �� 36��
            if (Random.value * 36 < 10)
                return (char)Random.Range(48, 58); //����
            else
                return (char)Random.Range(65, 91); //�빮��
        }

        return SB.ToString();
    }
}
