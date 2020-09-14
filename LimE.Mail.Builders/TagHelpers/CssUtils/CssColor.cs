namespace LimE.Mail.Builders.TagHelpers.CssUtils
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    public class CssColor
    {
        private readonly string value;

        public CssColor(string value)
        {
            Match m;
            m = Regex.Match(value, @"^#([0-9a-f]{2})([0-9a-f]{2})([0-9a-f]{2})([0-9a-f]{2})?$", RegexOptions.Singleline);
            if (m.Success)
            {
                this.Red = this.ConstrainValue(Convert.ToInt32(m.Groups[1].ToString(), 16), 0 ,255);
                this.Green = this.ConstrainValue(Convert.ToInt32(m.Groups[2].ToString(), 16), 0, 255);
                this.Blue = this.ConstrainValue(Convert.ToInt32(m.Groups[3].ToString(), 16), 0, 255);

                return;
            }

            m = Regex.Match(value, @"^rgba?\((\d+(?:\.\d+)?),(\d+(?:\.\d+)?),(\d+(?:\.\d+)?)(?:,(\d+(?:\.\d+)?))?\)$", RegexOptions.Singleline);
            if (m.Success)
            {
                this.Red = this.ConstrainValue(Convert.ToDouble(m.Groups[1].ToString()), 0, 255);
                this.Green = this.ConstrainValue(Convert.ToDouble(m.Groups[2].ToString()), 0, 255);
                this.Blue = this.ConstrainValue(Convert.ToDouble(m.Groups[3].ToString()), 0, 255);

                return;
            }

            this.value = value;
        }

        public double Red { get; set; }

        public double Green { get; set; }

        public double Blue { get; set; }

        public static implicit operator CssColor(string value) => new CssColor(value);

        public static implicit operator string(CssColor value) => value?.ToString();

        private double ConstrainValue(double value, double min = double.MinValue, double max = double.MaxValue)
        {
            return value < min ? min : value > max ? max : value;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.value))
            {
                var b = new StringBuilder("#");

                b.Append(BitConverter.ToString(new byte[] { (byte)this.Red, (byte)this.Green, (byte)this.Blue })).Replace("-", string.Empty);

                return b.ToString();
            }
            else
            {
                return this.value;
            }
        }
    }
}
