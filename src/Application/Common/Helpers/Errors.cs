using Attender.Server.Application.Common.Models;

namespace Attender.Server.Application.Common.Helpers
{
    public static class Errors
    {
        public static class User
        {
            public static Error Exists() =>
                new("user_already_exists", "User with such settings already exists");

            public static Error UserNameRequired() =>
                new("user_name_required", "User name wasn't provided");

            public static Error UserNameTooLong(int maxLength) =>
                new("user_name_too_long", $"User name exceeds {maxLength} characters");

            public static Error EmailInvalid() =>
                new("email_invalid", "Email has incorrect format");

            public static Error EmailTooLong(int maxLength) =>
                new("email_too_long", $"Email exceeds {maxLength} characters");
        }

        public static class Auth
        {
            public static Error AccessTokenNotExpired() =>
                new("access_token_not_expired", "Access token hasn't expired yet");

            public static Error AccessTokenRequired() =>
                new("access_token_required", "Access token wasn't provided");

            public static Error AccessTokenInvalidAlgorithm() =>
                new("access_token_invalid_algorithm", "Access token has invalid algorithm");

            public static Error AccessTokenSecurityIssue(string message) =>
                new("access_token_security_issue", message);

            public static Error RefreshTokenRequired() =>
                new("refresh_token_required", "Refresh token wasn't provided");

            public static Error RefreshTokenNotExist() =>
                new("refresh_token_not_exist", "Refresh token doesn't exist");

            public static Error RefreshTokenExpired() =>
                new("refresh_token_expired", "Refresh token has expired");

            public static Error RefreshTokenUsed() =>
                new("refresh_token_used", "Refresh token has already been used");

            public static Error RefreshTokenNotMatchJwt() =>
                new("refresh_token_not_match_jwt", "Refresh token doesn't match access JWT");
        }

        public static class Sms
        {
            public static Error VerificationCodeRequired() =>
                new("sms_code_required", "SMS code wasn't provided");

            public static Error VerificationCodeCreationFailed() =>
                new("sms_code_creation_failed", "An error occurred while creating the SMS code");

            public static Error VerificationCodeInvalid() =>
                new("sms_code_invalid", "Verification code is invalid");

            public static Error VerificationInvalidInput() =>
                new("sms_code_verify_invalid_input", "Phone number or verification code has incorrect format");

            public static Error VerificationCodeApproved() =>
                new("sms_code_already_approved", "SMS code already approved");

            public static Error PhoneNumberFlood() =>
                new("sms_phone_number_flood", "You asked for the code too many times");
        }

        public static class PhoneNumber
        {
            public static Error Required() =>
                new("phone_number_required", "Phone number is required");

            public static Error Invalid() =>
                new("phone_number_invalid", "Phone number has incorrect format");
        }

        public static class SubCategories
        {
            public static Error AlreadyAppliedForUser() =>
                new("sub_categories_already_applied_for_user", "Given sub-categories already applied for the user");
        }
    }
}
