using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.AspNetCore.Identity;

namespace UnitTests.Mock
{
    public class MockRoleManager
    {
        /// <summary>
        ///  Create a mock Role Manager
        /// </summary>
        /// <returns></returns>
        public static Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

        }
    }
}
