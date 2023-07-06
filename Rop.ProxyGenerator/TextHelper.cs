using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Rop.ProxyGenerator
{
    public static class TextHelper
    {

        /// <summary>
        /// Appendlines to a stringbuilder
        /// </summary>
        public static void AppendLines(this StringBuilder sb, params string[] lines)
        {
            AppendLines(sb, lines as IEnumerable<string>);
        }
        public static void AppendLines(this StringBuilder sb, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }
        }
        /// <summary>
        /// Appendlines to a stringbuilder
        /// </summary>
        public static void AppendLines(this StringBuilder sb,int tabs, params string[] lines)
        {
            AppendLines(sb,tabs, lines as IEnumerable<string>);
        }
        public static void AppendLines(this StringBuilder sb,int tabs, IEnumerable<string> lines)
        {
            var t = new string('\t', tabs);
            foreach (var line in lines)
            {
                sb.Append(t);
                sb.AppendLine(line);
            }
        }
        public static void AppendTab(this StringBuilder sb,int tabs,string cad)
        {
            var t = new string('\t', tabs);
            sb.Append(t);
            sb.Append(cad);
        }
        public static bool InList(this string cad, ImmutableHashSet<string> list) => list.Contains(cad);
        public static bool InList(this string cad, IEnumerable<string> list) => list.Contains(cad);
    }
}