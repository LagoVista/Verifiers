using LagoVista.IoT.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Verifiers.Resources
{
    public static class ErrorCodes
    {
        public static ErrorCode MissingBinaryInput { get { return new ErrorCode() { Code = "VERF1001", Message = "Missing Input to Convert to Binary Payload" }; } }
        public static ErrorCode CouldNotConvertInputToBytes { get { return new ErrorCode() { Code = "VERF1002", Message = "Could not convert input into Binary Payload" }; } }
    }
}