using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Converters
{
    public static class UserMapper
    {
        public static User ToUser(RegisterModel registerModel, string salt)
        {
            return new User
            {
                Email = registerModel.Email.ToLower(),
                Name = registerModel.Email,
                PasswordHash = HashUtility.CreatePasswordHash(registerModel.Password, salt),
                SecurityQuestion = registerModel.SecurityQuestion,
                SecurityQuestionAnswer = registerModel.SecurityQuestionAnswer,
                LastUpdatedOn = DateTime.UtcNow,
                IsEmailVerified = false,
                VerificationToken = Guid.NewGuid().ToString(),
                VerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };
        }



        public static User ToUser(AdministratorReqModel model, string pwd, string secret)
        {
            return new User
            {
                Email = model.Email.ToLower(),
                Name = model.Name,
                PasswordHash = HashUtility.CreatePasswordHash(pwd, secret),
                IsTemporaryPassword = true,
                LastUpdatedOn = DateTime.UtcNow,
                IsEmailVerified = true,
                VerificationToken = Guid.NewGuid().ToString(),
                VerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };
        }

        public static User UpdateVerificationToken(this User user, string verificationToken)
        {
            user.VerificationToken = verificationToken;
            return user;
        }

        public static User UpdateVerificationTokenExpiry(this User user, DateTime? verificationTokenExpiry)
        {
            user.VerificationTokenExpiry = verificationTokenExpiry;
            return user;
        }

        public static User UpdateIsEmailVerified(this User user, bool isEmailVerified)
        {
            user.IsEmailVerified = isEmailVerified;
            return user;
        }

        public static User UpdateLastUpdatedOn(this User user, DateTime lastUpdatedOn)
        {
            user.LastUpdatedOn = lastUpdatedOn;
            return user;
        }

        public static User UpdateLastUpdatedBy(this User user, long lastUpdatedBy)
        {
            user.LastUpdatedBy = lastUpdatedBy;
            return user;
        }

        public static User UpdateSecurityQuestion(this User user, string securityQuestion)
        {
            user.SecurityQuestion = securityQuestion;
            return user;
        }

        public static User UpdateSecurityQuestionAnswer(this User user, string securityQuestionAnswer)
        {
            user.SecurityQuestionAnswer = securityQuestionAnswer;
            return user;
        }

        public static User UpdatePasswordHash(this User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return user;
        }

        public static User UpdateIsTemporaryPassword(this User user, bool isTemporaryPassword)
        {
            user.IsTemporaryPassword = isTemporaryPassword;
            return user;
        }

        public static User UpdateEmail(this User user, string email)
        {
            user.Email = email;
            return user;
        }

        public static User UpdateName(this User user, string name)
        {
            user.Name = name;
            return user;
        }
    }
}
