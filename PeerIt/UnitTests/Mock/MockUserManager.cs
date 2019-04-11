using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using PeerIt.Models;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UnitTests.Mock
{

    public class MockUserManager : Microsoft.AspNetCore.Identity.UserManager<AppUser>
    {
        public MockUserManager()  : base(
                    new Mock<Microsoft.AspNetCore.Identity.IUserStore<AppUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<AppUser>>().Object,
                    new IUserValidator<AppUser>[0],
                    new IPasswordValidator<AppUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<Microsoft.AspNetCore.Identity.UserManager<AppUser>>>().Object)
                   // new Mock<IHttpContextAccessor>().Object)
        { }

        public override Task<AppUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(new AppUser { Email = email });
        }

        public override Task<bool> IsEmailConfirmedAsync(AppUser user)
        {
            return Task.FromResult(user.Email == "test@test.com");
        }

        public override Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return Task.FromResult("---------------");
        }


    }
    
}
