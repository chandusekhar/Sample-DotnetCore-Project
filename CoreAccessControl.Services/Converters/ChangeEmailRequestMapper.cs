using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Converters
{
    public static class ChangeEmailRequestMapper
    {
        public static ChangeEmailRequest UpdateVerificationToken(this ChangeEmailRequest changeEmailRequest, string verificationToken)
        {
            changeEmailRequest.VerificationToken = verificationToken;
            return changeEmailRequest;
        }

        public static ChangeEmailRequest UpdateVerificationTokenExpiry(this ChangeEmailRequest changeEmailRequest, DateTime? verificationTokenExpiry)
        {
            changeEmailRequest.VerificationTokenExpiry = verificationTokenExpiry;
            return changeEmailRequest;
        }
    }
}
