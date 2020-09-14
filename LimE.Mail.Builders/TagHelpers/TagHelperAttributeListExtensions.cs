namespace LimE.Mail.Builders.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public static class TagHelperAttributeListExtensions
    {
        public static string TryGet(this IDictionary<string, string> ext, string key)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            return ext.ContainsKey(key) ? ext[key] : null;
        }

        public static IDictionary<string, string> Merge(this IDictionary<string, string> ext, params IDictionary<string, string>[] others)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            foreach (var item in others.Where(p => p != null && p.Count > 0))
            {
                foreach (var kv in item)
                {
                    if (!ext.ContainsKey(kv.Key))
                    {
                        ext.Add(kv.Key, kv.Value);
                    }
                    else
                    {
                        ext[kv.Key] = kv.Value;
                    }
                }
            }

            return ext;
        }

        public static IDictionary<string, string> MergeCopy(this IDictionary<string, string> ext, params IDictionary<string, string>[] others)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            return new Dictionary<string, string>().Merge(new[] { ext }.Union(others).ToArray());
        }

        public static string AddAttributes(this string ext, IDictionary<string, string> attributes)
        {
            if (ext == null || !Regex.IsMatch(ext, @"^<[^/].*>$", RegexOptions.Singleline))
            {
                throw new ArgumentException("ext is not an opening tag", nameof(ext));
            }

            return $"{ext[0..^1]} {string.Join(' ', attributes.Select(p => $"{p.Key}=\"{p.Value}\""))}>";
        }

        public static string AsCssString(this IDictionary<string, string> ext)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            return $"{string.Join(';', ext.Select(p => $"{p.Key}:{p.Value}").ToArray())};";
        }

        public static Dictionary<string, string> GetAttributes(this TagHelperAttributeList ext)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            return ext.ToDictionary(p => p.Name, p => p.Value?.ToString());
        }

        public static void SetAttributes(this TagHelperAttributeList ext, IDictionary<string, string> attributes)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            foreach (var item in attributes)
            {
                ext.SetAttribute(item.Key, item.Value);
            }
        }

        public static void MergeAttributes(this TagHelperAttributeList ext, params IDictionary<string, string>[] attributes)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            ext.SetAttributes(ext.GetAttributes().Merge(attributes));
        }

        public static Dictionary<string, string> GetInlineStyles(this TagHelperAttributeList ext)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            return ext["style"]?.Value?.ToString()?.ToLowerInvariant()?.Split(';')?
                .Where(p => !string.IsNullOrWhiteSpace(p))?.Select(p => p.Split(':'))?
                .ToDictionary(p => p[0].Trim(), p => p[1].TrimEnd(';').Trim()) ?? new Dictionary<string, string>();
        }

        public static string GetInlineStyleValue(this TagHelperAttributeList ext, string name)
        {
            var tmp = ext.GetInlineStyles();
            return tmp.ContainsKey(name) ? tmp[name] : null;
        }

        public static void SetInlineStyles(this TagHelperAttributeList ext, IDictionary<string, string> styles)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            if (styles != null && styles.Count > 0)
            {
                ext.SetAttribute("style", styles.AsCssString());
            }
            else if (ext.TryGetAttribute("style", out TagHelperAttribute attr))
            {
                ext.Remove(attr);
            }
        }

        public static void MergeInlineStyles(this TagHelperAttributeList ext, params IDictionary<string, string>[] styles)
        {
            _ = ext ?? throw new ArgumentNullException(nameof(ext));

            ext.SetInlineStyles(ext.GetInlineStyles().Merge(styles));
        }

        public static void SetInlineStyleValue(this TagHelperAttributeList ext, string name, string value)
        {
            ext.MergeInlineStyles(new Dictionary<string, string>() { { name, value } });
        }
    }
}
