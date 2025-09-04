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
            //숫자10개, 대문자26개 = 총 36개
            if (Random.value * 36 < 10)
                return (char)Random.Range(48, 58); //숫자
            else
                return (char)Random.Range(65, 91); //대문자
        }

        return SB.ToString();
    }
}
