using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public static class Constants
    {
        public static string JWTPayloadClaim = "Data";
    }

    public static class EmailConstants
    {
        public static string EmailVerificationTemplate = "EmailVerificationTemplate.json";
        public static string ResetPasswordTemplate = "ResetPasswordTemplate.json";
        public static string AuthCodeSendingTemplate = "AuthCodeSendingTemplate.json";
    }
}
