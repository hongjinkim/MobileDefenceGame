using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public static class Extension
{
    public static string ToABC(this BigNum bigNum) => bigNum.ToRegularFormat();
    public static string ToABC(this double bigNum) => new BigNum(bigNum).ToABC();
    public static string ToA_BC(this double bigNum) => new BigNum(bigNum).ToA_BC();

    public static string ToBCD(this BigNum bigNum) => bigNum.ToWideFormat();
    public static string ToBCD(this double bigNum) => new BigNum(bigNum).ToBCD();
    public static string ToBCD(this float bigNum) => new BigNum(bigNum).ToBCD();
    public static string ToBCD(this int bigNum) => new BigNum(bigNum).ToBCD();
    public static string ToBCD(this long bigNum) => new BigNum(bigNum).ToBCD();

    //소수점 이하도 표시하도록 하는 메서드 (float변환 활용, 최대 2자리까지 표시, 소수점 맨뒷자리 0은 배제)
    public static string ToA_BC(this BigNum bigNum)
    {
        if (bigNum >= 100 || bigNum < -100)
            return bigNum.ToRegularFormat();

        else if (bigNum >= 10 || bigNum <= -10)
            return String.Format("{0:0.#}", (float)bigNum);

        else
            return String.Format("{0:0.##}", (float)bigNum);
    }

    // 표준 Extension Method
    //public static string ToABC(this PoomNum poomNum) => IsKorean ? poomNum.ToKorean() : poomNum.ToRegularFormat();

    public static string ToRound(this float Num) => (Mathf.Round(Num * 100) / 100f).ToString();

    public static string ToRound(this double Num) => (Mathf.Round((float)Num * 100) / 100f).ToString();
    public static void InitializeArray<T>(this T[] array) where T : class, new()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new T();
        }
    }

    public static T[] InitializeArray<T>(int length) where T : class, new()
    {
        T[] array = new T[length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new T();
        }

        return array;
    }



    // string을 enum 목록에서 찾아 해당 enum형태의 index를 반환
    public static int ConvertStringToEnum(string str, Array arr)
    {
        foreach (var e in arr)
        {
            if (e.ToString() == str)
                return (int)e;
        }

        return -1;
    }

    public static int FloatToIntByRatio(this float value)
    {
        int result = (int)value;
        if (UnityEngine.Random.value < value - result) { return result + 1; }
        else { return result; }
    }


    //일반시간 - TimeScale에 영향o : Keyword는 delay의 float ->  WaitForSeconds를 Pooling
    private static readonly Dictionary<float, WaitForSeconds> t_Pool = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y) { return x == y; }
        int IEqualityComparer<float>.GetHashCode(float obj) { return obj.GetHashCode(); }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();


    //일반시간 외부호출 함수
    public static void Invoke(this MonoBehaviour mb, Action f, float delay) { mb.StartCoroutine(InvokeRoutine(f, delay)); }

    private static IEnumerator InvokeRoutine(Action function, float delay)
    {
        WaitForSeconds wfs;
        if (!t_Pool.TryGetValue(delay, out wfs)) { t_Pool.Add(delay, wfs = new WaitForSeconds(delay)); }
        yield return wfs; //time대기
        function(); //지연후 메소드 실행
    }
    public static WaitForSeconds Yield(this MonoBehaviour mb, float tick)
    {
        if (!t_Pool.TryGetValue(tick, out WaitForSeconds result)) { t_Pool.Add(tick, new WaitForSeconds(tick)); }

        return result;
    }

    // 중복없는 난수리스트 메서드
    public static List<int> List_Pick_noDup(int Start_Num, int End_Num, int Pick_Num)
    {
        if (Start_Num >= End_Num) { Debug.LogWarning("시작정수보다 높은 숫자 입력 필요"); return null; }

        List<int> target_list = new List<int>();
        List<int> picked_list = new List<int>();

        int picked_Index;

        for (int i = Start_Num; i <= End_Num; i++) { target_list.Add(i); } //타겟리스트 int순으로 생성

        for (int i = 0; i < Pick_Num; i++) //랜덤하게 뽑음
        {
            picked_Index = (int)(target_list.Count * UnityEngine.Random.value);
            picked_list.Add(target_list[picked_Index]);
            target_list.RemoveAt(picked_Index);
        }

        return picked_list;
    }

    //float 똥값제거해서 int로 변환하는 메서드
    public static int floatToInt(this float f)
    {
        return (int)(f + 0.0001f);
    }


    /* *** 아래는 각자의 게임에서 언어설정을 불러올 수 있게 구조짤 것 *** */

    // 설정언어 참조
    // 예시 : private static SystemLanguage currentLanguage => Blackboard.Settings.CurrentLanguage;
    private static SystemLanguage CurrentLanguage => SystemLanguage.Korean;     // 임시

    // 한글 출력 여부 판별
    // SystemLanguage 사용하지 않는다면 여기를 바꿔야함
    private static bool IsKorean => CurrentLanguage == SystemLanguage.Korean;   // 임시

}

