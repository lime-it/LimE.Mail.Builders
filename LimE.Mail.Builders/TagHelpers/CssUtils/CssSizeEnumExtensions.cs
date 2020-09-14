namespace LimE.Mail.Builders.TagHelpers.CssUtils
{
    public static class CssSizeEnumExtensions
    {
        public static string AsString(this CssSizeUnit ext)
        {
            return ext switch
            {
                CssSizeUnit.Pixel => "px",
                CssSizeUnit.Percent => "%",
                CssSizeUnit.Point => "pt",
                CssSizeUnit.Centimeter => "cm",
                CssSizeUnit.Em => "em",
                CssSizeUnit.Rem => "rem",
                _ => "px",
            };
        }

        public static CssSizeUnit AsCssUnit(this string ext)
        {
            return ext?.ToLower() switch
            {
                "px" => CssSizeUnit.Pixel,
                "%" => CssSizeUnit.Percent,
                "pt" => CssSizeUnit.Point,
                "cm" => CssSizeUnit.Centimeter,
                "em" => CssSizeUnit.Em,
                "rem" => CssSizeUnit.Rem,
                _ => CssSizeUnit.Pixel,
            };
        }
    }
}
