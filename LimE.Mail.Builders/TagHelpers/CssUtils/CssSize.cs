namespace LimE.Mail.Builders.TagHelpers.CssUtils
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    public class CssSize
    {
        public CssSize(string value)
        {
            var m = Regex.Match(value, @"^(\-?\d+(?:\.\d+)?)([^\d\.]*)$", RegexOptions.Singleline);
            if (!m.Success)
            {
                throw new ArgumentException("The value is not a valid css size");
            }

            this.Value = Convert.ToDouble(m.Groups[1].ToString());
            this.Unit = m.Groups[2].ToString().AsCssUnit();
        }

        public double Value { get; set; }

        public CssSizeUnit Unit { get; set; }

        public static implicit operator CssSize(string value) => new CssSize(value);

        public static implicit operator string(CssSize value) => value?.ToString();

        public override string ToString()
        {
            return $"{this.Value}{this.Unit.AsString()}";
        }
    }
}
