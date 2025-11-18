using System.ComponentModel;

namespace CapaEntidad.Response {
    public enum ResponseCodeToast {
        [Description("success")]
        Success = 1,
        [Description("warning")]
        Warning = 2,
        [Description("info")]
        Info = 3,
        [Description("error")]
        Error = 4,
    }
}
