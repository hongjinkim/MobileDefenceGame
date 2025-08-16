using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TextRoller
{
    public enum Mode
    {
        Original,       // 예전 그대로: 랜덤 프레임 → 최종 문자열로 고정
        Progressive,    // 왼쪽 자리부터 숫자 고정 (원하면 선택)
        CountUp         // 숫자 보간 (옵션: formatter 필요)
    }

    private static readonly Dictionary<TMP_Text, Coroutine> running = new();

    public static void Roll(
        MonoBehaviour host,
        TMP_Text text,
        string finalText,
        Mode mode = Mode.Original,
        float duration = 0.3f,
        float minInterval = 0.02f,
        float maxInterval = 0.06f,
        System.Func<long, string> formatterForCountUp = null // CountUp모드에서만 사용
    )
    {
        if (host == null || text == null) return;

        if (running.TryGetValue(text, out var prev))
            host.StopCoroutine(prev);

        Coroutine co = mode switch
        {
            Mode.Progressive => host.StartCoroutine(RollProgressive(text, finalText, duration, minInterval, maxInterval)),
            Mode.CountUp => host.StartCoroutine(RollCountUp(text, finalText, duration, formatterForCountUp)),
            _ => host.StartCoroutine(RollOriginal(text, finalText, duration, minInterval, maxInterval)),
        };

        running[text] = co;
    }

    // === 처음 버전: 랜덤 프레임 → 최종 고정 ===
    private static IEnumerator RollOriginal(TMP_Text text, string finalText, float duration, float minInterval, float maxInterval)
    {
        char[] finalChars = finalText.ToCharArray();
        var digitIdx = new List<int>();
        for (int i = 0; i < finalChars.Length; i++)
            if (char.IsDigit(finalChars[i])) digitIdx.Add(i);

        // 숫자가 하나도 없으면 바로 표기
        if (digitIdx.Count == 0)
        {
            text.text = finalText;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float p = Mathf.Clamp01(elapsed / duration);
            var frame = new char[finalChars.Length];

            for (int i = 0; i < finalChars.Length; i++)
            {
                if (!char.IsDigit(finalChars[i]))
                {
                    frame[i] = finalChars[i];
                }
                else
                {
                    frame[i] = (char)('0' + Random.Range(0, 10)); // 완전 랜덤 (처음 느낌 그대로)
                }
            }

            text.text = new string(frame);

            float interval = Mathf.Lerp(minInterval, maxInterval, p);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        text.text = finalText;
        running.Remove(text);
    }

    // === 선택: 왼쪽부터 고정 (원하면 이 모드로) ===
    private static IEnumerator RollProgressive(TMP_Text text, string finalText, float duration, float minInterval, float maxInterval)
    {
        char[] finalChars = finalText.ToCharArray();
        var digitIdx = new List<int>();
        for (int i = 0; i < finalChars.Length; i++)
            if (char.IsDigit(finalChars[i])) digitIdx.Add(i);

        if (digitIdx.Count == 0)
        {
            text.text = finalText;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float p = Mathf.Clamp01(elapsed / duration);
            float eased = 1f - Mathf.Pow(1f - p, 3f);
            int locked = Mathf.FloorToInt(eased * digitIdx.Count);

            var frame = new char[finalChars.Length];
            for (int i = 0; i < finalChars.Length; i++)
            {
                if (!char.IsDigit(finalChars[i]))
                {
                    frame[i] = finalChars[i];
                    continue;
                }
                int rank = digitIdx.IndexOf(i);
                frame[i] = (rank < locked) ? finalChars[i] : (char)('0' + Random.Range(0, 10));
            }

            text.text = new string(frame);

            float interval = Mathf.Lerp(minInterval, maxInterval, p);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        text.text = finalText;
        running.Remove(text);
    }

    // === 선택: 순수 카운트업 (문자열 포맷 유지하려면 formatter 제공 권장) ===
    private static IEnumerator RollCountUp(TMP_Text text, string finalText, float duration, System.Func<long, string> formatter)
    {
        if (!TryParseLongFromString(finalText, out var target))
        {
            // 파싱 불가하면 그냥 즉시 표기
            text.text = finalText;
            running.Remove(text);
            yield break;
        }

        long start = TryParseLongFromString(text.text, out var cur) ? cur : 0;
        if (start == target)
        {
            text.text = formatter != null ? formatter(target) : finalText;
            running.Remove(text);
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            float p = Mathf.Clamp01(t / duration);
            float eased = 1f - Mathf.Pow(1f - p, 3f);
            long val = LerpLong(start, target, eased);

            text.text = formatter != null ? formatter(val) : val.ToString(); // formatter 없으면 간단 표기
            yield return null;
            t += Time.deltaTime;
        }

        text.text = formatter != null ? formatter(target) : finalText;
        running.Remove(text);
    }

    private static long LerpLong(long a, long b, float t)
    {
        double v = a + (b - a) * t;
        return (long)System.Math.Round(v);
    }

    private static bool TryParseLongFromString(string s, out long value)
    {
        value = 0;
        if (string.IsNullOrEmpty(s)) return false;
        System.Text.StringBuilder sb = new(s.Length);
        bool neg = false;
        foreach (var ch in s)
        {
            if (ch == '-' && sb.Length == 0) { neg = true; continue; }
            if (char.IsDigit(ch)) sb.Append(ch);
        }
        if (sb.Length == 0) return false;
        if (!long.TryParse(sb.ToString(), out var v)) return false;
        value = neg ? -v : v;
        return true;
    }
}
