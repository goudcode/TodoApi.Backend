﻿namespace Goudcode.TodoApi.Backend.Features.Authentication
{
    public static class IdentityData
    {
        public const string AdminUserClaimName = "isAdmin";
        public const string UsernameClaimName = "username";
        public const string UserIdClaimName = "userId";

        public const string AdminUserPolicyName = "Admin";
    }
}
