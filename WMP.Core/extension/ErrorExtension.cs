using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMP.Core
{
    public static class ErrorExtension
    {
        public static Exception AddAdditionalInfo(this Exception exception, params object[] ps)
        {
            if (0 == ps.Length) return exception;

            exception.Data["p_error_sql"] = ps[0];

            return exception;
        }

        public static string ThrowIfArgumentIsMissing(this IDictionary<string, string> parameters, string parameterName, string message = null, bool required = false)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentNullException(null, $"서비스 실행에 필요한 항목이 없습니다.[{parameterName}]");
                else
                    throw new ArgumentNullException(null, $"서비스 실행에 필요한 항목이 없습니다.[{message}({parameterName})]");
            }

            if (required)
            {
                if (string.IsNullOrEmpty(parameters[parameterName]))
                    throw new ArgumentNullException(null, $"서비스 실행에 필요한 필수값이 없습니다.[{message}({parameterName})]");
            }

            return parameters[parameterName];
        }
    }
}
