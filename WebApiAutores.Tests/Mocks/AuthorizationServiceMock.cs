﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAutores.Tests.Mocks
{
    internal class AuthorizationServiceMock : IAuthorizationService
    {
        public AuthorizationResult Estado { get; set; }
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, 
            IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(Estado);
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            return Task.FromResult(Estado);
        }
    }
}
