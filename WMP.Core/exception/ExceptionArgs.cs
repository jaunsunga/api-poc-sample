using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMP.Core {
    [Serializable]
    public abstract class ExceptionArgs {
        public virtual string Message { get { return string.Empty; } }
    }
}
