using System;

namespace MathExamWeb.Data
{
    public struct Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) throw new ArgumentException("Denominator cannot be zero.");
            Numerator = numerator;
            Denominator = denominator;
            Simplify();
        }

        private void Simplify()
        {
            int gcd = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
            Numerator /= gcd;
            Denominator /= gcd;
            if (Denominator < 0)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static Fraction operator +(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static Fraction operator -(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static Fraction operator *(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0) throw new DivideByZeroException();
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public override string ToString()
        {
            if (Denominator == 1) return Numerator.ToString();
            return $"{Numerator}/{Denominator}";
        }

        public static bool TryParse(string input, out Fraction result)
        {
            result = new Fraction(0, 1);
            if (string.IsNullOrWhiteSpace(input)) return false;

            string[] parts = input.Split('/');
            if (parts.Length == 1)
            {
                if (int.TryParse(parts[0], out int n))
                {
                    result = new Fraction(n, 1);
                    return true;
                }
            }
            else if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out int n) && int.TryParse(parts[1], out int d) && d != 0)
                {
                    result = new Fraction(n, d);
                    return true;
                }
            }
            return false;
        }

        public bool Equals(Fraction other)
        {
            return Numerator == other.Numerator && Denominator == other.Denominator;
        }
    }
}
