using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMP.Core.utils
{
    public class ScreenConditionBuilder
    {
        //화면조건 로그에서 제외하는 목록
        private static readonly List<string> excludes = new List<string> {
            "X - UIClient", "TOKEN", "SERVER"
        };

        private static readonly List<string> excludes2 = new List<string> {
            "X - UIClient", "TOKEN", "SERVER", "OWNERSHIP", "RQST_MADDR"
        };

        public static string Build(IDictionary<string, string> parameters)
        {
            var condition = parameters.Keys.Where(x => !excludes.Contains(x)).ToDictionary(k => k, v => parameters[v]);
            var 화면조건 = String.Join(",", condition.Values);

            return 화면조건;
        }

        public static string Build2(IDictionary<string, string> parameters)
        {
            var p = string.Join(Environment.NewLine, parameters.Where(x => !excludes2.Contains(x.Key)).Select(x => string.Format("{0}:{1}", x.Key, x.Value)));
            return p;
        }
    }
}
