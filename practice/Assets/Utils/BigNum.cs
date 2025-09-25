using System;
using System.Text;
using UnityEngine;
using GoogleSheet.Type;
using System.ComponentModel;
using Sirenix.OdinInspector;

[Serializable]
public struct BigNum
{
    private static readonly long[] decimals
        = new long[] { 1L, 10L, 100L, 1000L, 10000L, 100000L, 1000000L, 10000000L, 100000000L, 1000000000L, 10000000000L, 100000000000L,
          1000000000000L, 10000000000000L, 100000000000000L, 1000000000000000L, 10000000000000000L, 100000000000000000L, 1000000000000000000L };

    // 제곱을 반환
    public static BigNum Exp(double x)
    {
        if (x < 0)
        {
            throw new Exception("x가 0보다 작은 값의 계산을 지원하지 않습니다.");
        }
        else if (x == 0)
        {
            return new BigNum(1);
        }

        long m, e;
        double y = Math.Log10(Math.E) * x;

        e = (long)y;
        m = (long)(Math.Pow(10, y - e) * 100000000L);

        return new BigNum(m, e - 8, refine: false);
    }

    [HideInInspector]public long M;
    [HideInInspector]public long E;

#if ODIN_INSPECTOR

    [ShowInInspector]
    private string Num => new BigNum(M, E, false).ToBCD();

#endif


    public BigNum(BigNum x)
    {
        M = x.M;
        E = x.E;
    }

    public BigNum(long _m, long _e, bool refine = true)
    {
        M = _m;
        checked
        {
            E = _e;
        }
        if (refine == true) Refine();
    }

    public BigNum(double d)
    {
        if (d == 0d)
        {
            M = 0;
            E = 0;
        }
        else
        {
            if (d > 0)
            {
                if (d < 1)
                {
                    int expo = (int)Math.Floor(Math.Log10(d));
                    M = (long)(d * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
                else
                {
                    int expo = (int)Math.Log10(d);
                    M = (long)(d * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
            }
            else
            {
                if (d > -1)
                {
                    int expo = (int)Math.Floor(Math.Log10(-d));
                    M = (long)(d * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
                else
                {
                    int expo = (int)Math.Log10(-d);
                    M = (long)(d * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
            }
        }
    }

    public BigNum(float f)
    {
        if (f == 0f)
        {
            M = 0;
            E = 0;
        }
        else
        {
            if (f > 0)
            {
                if (f < 1)
                {
                    int expo = (int)Mathf.Floor(Mathf.Log10(f));
                    M = (long)(f * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
                else
                {
                    int expo = (int)Math.Log10(f);
                    M = (long)(f * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
            }
            else
            {
                if (f > -1)
                {
                    int expo = (int)Mathf.Floor(Mathf.Log10(-f));
                    M = (long)(f * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
                else
                {
                    int expo = (int)Math.Log10(-f);
                    M = (long)(f * Math.Pow(10, 8 - expo));
                    E = expo - 8;
                }
            }
        }
    }

    public BigNum(long l)
    {
        if (l == 0)
        {
            M = 0;
            E = 0;
        }
        else
        {
            int expo = (int)Math.Log10(l > 0 ? l : -l);

            if (expo == 8)
            {
                M = l;
                E = 0;
            }
            else if (expo < 8)
            {
                M = l * decimals[8 - expo];
                E = expo - 8;
            }
            else // if (expo > 8)
            {
                M = l / decimals[expo - 8];
                E = expo - 8;
            }
        }
    }

    public BigNum(int i)
    {
        if (i == 0)
        {
            M = 0;
            E = 0;
        }
        else
        {
            int expo = (int)Math.Log10(i > 0 ? i : -i);

            if (expo == 8)
            {
                M = i;
                E = 0;
            }
            else if (expo < 8)
            {
                M = i * decimals[8 - expo];
                E = expo - 8;
            }
            else // if (expo > 8)
            {
                M = i / decimals[expo - 8];
                E = expo - 8;
            }
        }
    }

    private void Refine()
    {
        if (M == 0)
        {
            // M = 0;
            E = 0;
        }
        else if (M < 1000000000 && M >= 100000000)
        {
            return;
        }
        else
        {
            int expo = (int)Math.Log10(M > 0 ? M : -M);

            if (expo < 8)
            {
                M *= decimals[8 - expo];
            }
            else if (expo > 8)
            {
                M /= decimals[expo - 8];
            }

            checked
            {
                E += (expo - 8);
            }
        }
    }


    // operators

    public static BigNum operator +(BigNum x) => x;

    public static BigNum operator -(BigNum x) => new BigNum(-x.M, x.E, false);

    public static BigNum operator +(BigNum x, BigNum y)
    {
        if (y.IsZero() == true) return x;
        else if (x.IsZero() == true) return y;
        else if (x.E == y.E)
        {
            if (x.M == 0) return y;
            else if (y.M == 0) return x;
            else return new BigNum(x.M + y.M, x.E, true);
        }
        else if (x.E > y.E)
        {
            long expoGap = x.E - y.E;
            if (expoGap > 8) return x;
            else return new BigNum(x.M * decimals[expoGap] + y.M, y.E);
        }
        else // if (x.E < y.E)
        {
            long expoGap = y.E - x.E;
            if (expoGap > 8) return y;
            else return new BigNum(x.M + y.M * decimals[expoGap], x.E);
        }
    }

    public static BigNum operator -(BigNum x, BigNum y)
    {
        if (y.IsZero() == true) return x;
        else if (x.IsZero() == true) return -y;
        if (x.E == y.E)
        {
            if (x.M == 0) return -y;
            else if (y.M == 0) return x;
            else return new BigNum(x.M - y.M, x.E, true);
        }
        else if (x.E > y.E)
        {
            long expoGap = x.E - y.E;
            if (expoGap > 8) return x;
            else return new BigNum(x.M * decimals[expoGap] - y.M, y.E);
        }
        else // if (x.E < y.E)
        {
            long expoGap = y.E - x.E;
            if (expoGap > 8) return -y;
            else return new BigNum(x.M - y.M * decimals[expoGap], x.E);
        }
    }

    public static BigNum operator *(BigNum x, BigNum y)
    {
        if (x.IsZero() == true) return x;
        else if (y.IsZero() == true) return y;
        else if (x.IsOne() == true) return y;
        else if (y.IsOne() == true) return x;
        else return new BigNum(x.M * y.M, x.E + y.E, true);
    }

    public static BigNum operator /(BigNum x, BigNum y)
    {
        if (x.M == 0) return x;
        else if (y.IsOne() == true) return x;
        else return new BigNum((x.M * decimals[9]) / y.M, x.E - y.E - 9, true);
    }

    public static bool operator ==(BigNum x, BigNum y) => (x.E == y.E && x.M == y.M);

    public static bool operator !=(BigNum x, BigNum y) => (x.E != y.E || x.M != y.M);

    public static bool operator >(BigNum x, BigNum y)
    {
        if (x.E == y.E) return x.M > y.M; // 둘의 지수가 같다면 가수부만 비교
        else if (x.M * y.M <= 0) return x.M > y.M;  // 둘의 부호가 다르거나 하나 이상이 0일 때, 지수는 의미 없음
        else if (x.M > 0 /*&& y.M > 0*/) return x.E > y.E; // 둘다 양수이고 지수가 다를 때는 지수가 큰 쪽이 큼
        else /*if (x.M < 0 && y.M < 0)*/ return x.E < y.E; // 둘다 음수이고 지수가 다를 때는 지수가 작은 쪽이 큼
    }

    public static bool operator <(BigNum x, BigNum y)
    {
        if (x.E == y.E) return x.M < y.M; // 둘의 지수가 같다면 가수부만 비교
        else if (x.M * y.M <= 0) return x.M < y.M;  // 둘의 부호가 다르거나 하나 이상이 0일 때, 지수는 의미 없음
        else if (x.M > 0 /*&& y.M > 0*/) return x.E < y.E; // 둘다 양수이고 지수가 다를 때는 지수가 큰 쪽이 큼
        else /*if (x.M < 0 && y.M < 0)*/ return x.E > y.E; // 둘다 음수이고 지수가 다를 때는 지수가 작은 쪽이 큼
    }

    public static bool operator >=(BigNum x, BigNum y) => (x > y || x == y);

    public static bool operator <=(BigNum x, BigNum y) => (x < y || x == y);


    public static implicit operator BigNum(int i) => new BigNum(i);

    public static implicit operator BigNum(long l) => new BigNum(l);

    public static implicit operator BigNum(float f) => new BigNum(f);

    public static implicit operator BigNum(double d) => new BigNum(d);

    public static explicit operator float(BigNum x)
    {
        if (x.IsZero() == true) return 0f;
        else if (x.E > 29) throw new Exception("PoomNum -> float 변환가능 범위 초과");
        else if (x.E < -29) return float.Epsilon;
        else return (float)x.M * Mathf.Pow(10f, x.E);
    }

    public static explicit operator double(BigNum x)
    {
        if (x.IsZero() == true) return 0d;
        else if (x.E > 298) throw new Exception("PoomNum -> double 변환가능 범위 초과");
        else if (x.E < -298) return double.Epsilon;
        else return (double)x.M * Math.Pow(10d, x.E);
    }


    // Methods

    public static float Progress01(BigNum numerator, BigNum denominator)
    {
        if (denominator.IsZero() == true) throw new DivideByZeroException();
        if (denominator < 0) throw new Exception("PoomNum.Progress는 분모값이 음수일 수 없습니다.");

        if (numerator.M <= 0) return 0f;
        if (numerator >= denominator) return 1f;

        if (numerator.E > denominator.E + 29) return 1f;
        if (numerator.E < denominator.E - 29) return 0f;

        return Mathf.Clamp01((float)(numerator / denominator));
    }

    public string Log() => $"{GetType()} ( Mantissa: {M:0}, Exponent: {E:0} )";

    public override string ToString()
    {
        if (IsZero() == true) return "0.0000000E+000";
        else if (E < -8)
        {
            string m2string = M.ToString("0");

            if (M > 0)
            {
                return $"{m2string[0]}.{m2string.Substring(1, 8)}E{E + 8:000}";
            }
            else
            {
                return $"{m2string.Substring(0, 2)}.{m2string.Substring(2, 8)}E{E + 8:000}";
            }
        }
        else // if (E >= -8)
        {
            string m2string = M.ToString("0");

            if (M > 0)
            {
                return $"{m2string[0]}.{m2string.Substring(1, 8)}E+{E + 8:000}";
            }
            else
            {
                return $"{m2string.Substring(0, 2)}.{m2string.Substring(2, 8)}E+{E + 8:000}";
            }
        }
    }


    public bool IsNegative() => (M < 0);

    public bool IsZero() => (M == 0);

    public bool IsOne() => (M == 100000000 && E == -8);

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();


    public static BigNum Parse(string format)
    {
        if (format[0] == '0') return new BigNum(0);

        long mantissa = (format[0] == '-') ? long.Parse($"-{format[1]}{format.Substring(3, 8)}") : long.Parse($"{format[0]}{format.Substring(2, 8)}");
        long exponent = long.Parse($"{format.Split('E')[1]}");

        return new BigNum(mantissa, exponent - 8);
    }

    public static bool TryParse(string format, out BigNum poomNum)
    {
        bool success;

        try
        {
            poomNum = Parse(format);
            success = true;
        }
        catch
        {
            poomNum = default;
            success = false;
        }

        return success;
    }
}
public static class PoomNumExtensions
{
    public static string ToRegularFormat(this BigNum x)
    {
        if (x.IsNegative() || x.IsZero() || x.E < -8) return "0"; // 1보다 작다면 0으로 표시

        long m = x.M;
        long e = x.E;

        if (e < -5)
        {
            return e switch
            {
                -8 => (m / 100000000).ToString("0"),
                -7 => (m / 10000000).ToString("0"),
                -6 => (m / 1000000).ToString("0"),
                _ => "0"
            };
        }
        else
        {
            StringBuilder result = new StringBuilder();
            string digit = m.ToString("0");
            string clean = ((e + 8) % 3) switch
            {
                0 => $"{digit[0]}.{digit[1]}{digit[2]}",
                1 => $"{digit[0]}{digit[1]}.{digit[2]}",
                2 => $"{digit[0]}{digit[1]}{digit[2]}.",
                _ => string.Empty,
            };
            result.Append(clean.TrimEnd('0').TrimEnd('.'));

            // normalize Exponent
            e = (e + 5) / 3;

            int letterCount = 0;
            long cutline = 0;
            long multiply = 1;
            long prev, index;

            do
            {
                multiply *= 26;
                prev = cutline;
                cutline += multiply;
                letterCount++;
            }
            while (e >= cutline); 

            for (int i = 0; i < letterCount; i++)
            {
                multiply /= 26;
                index = (e - prev) / multiply;
                result.Append((char)(index % 26 + 65));
                e -= index * multiply;
                prev -= multiply;
            }

            return result.ToString();
        }

    }

    public static string ToWideFormat(this BigNum x)
    {
        if (x.IsNegative() || x.IsZero() || x.E < -8) return "0"; // 1보다 작다면 0으로 표시

        long m = x.M;
        long e = x.E;

        if (e < -2)
        {
            return e switch
            {
                -8 => (m / 100000000).ToString("0"),
                -7 => (m / 10000000).ToString("0"),
                -6 => (m / 1000000).ToString("0"),
                -5 => (m / 100000).ToString("0"),
                -4 => (m / 10000).ToString("0"),
                -3 => (m / 1000).ToString("0"),
                _ => "0"
            };
        }
        else
        {
            StringBuilder result = new();
            string digit = m.ToString("0");
            string clean = ((e + 8) % 3) switch
            {
                0 => $"{digit[0]}.{digit[1]}{digit[2]}",
                1 => $"{digit[0]}{digit[1]}.{digit[2]}",
                2 => $"{digit[0]}{digit[1]}{digit[2]}.",
                _ => string.Empty,
            };
            result.Append(clean.TrimEnd('0').TrimEnd('.'));

            // normalize Exponent
            e = (e + 5) / 3;

            int letterCount = 0;
            long cutline = 0;
            long multiply = 1;
            long prev, index;

            do
            {
                multiply *= 26;
                prev = cutline;
                cutline += multiply;
                letterCount++;
            }
            while (e >= cutline);

            for (int i = 0; i < letterCount; i++)
            {
                multiply /= 26;
                index = (e - prev) / multiply;
                result.Append((char)(index % 26 + 65));
                e -= index * multiply;
                prev -= multiply;
            }

            return result.ToString();
        }

    }
}


[Type(typeof(BigNum), new string[] { "BigNum", "bignum", "BN" })]
public class BigNumType : IType
{
    public object DefaultValue => new BigNum(0);
   
    public object Read(string value)
    {
        return new BigNum(double.Parse(value));
    }
    public string Write(object value)
    {
        return "";
    }
}




