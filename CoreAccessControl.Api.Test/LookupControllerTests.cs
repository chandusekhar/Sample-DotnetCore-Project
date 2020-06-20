using CoreAccessControl.API.Controllers;
using CoreAccessControl.DataAccess.Ef.Data;
using Microsoft.AspNetCore.Mvc;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class LookupControllerTests
    {
        public LookupController GetLookupController()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            return new LookupController();
        }

        [Theory]
        [InlineData("Administrator", "Active")]
        [InlineData("Device", "Active")]
        [InlineData("Keyholder", "Active")]
        [InlineData("Space", "Active")]
        public void GetAssets_ReturnStatusByGivenType(string input, string expected)
        {
            var сontroller = GetLookupController();
            var response = сontroller.GetStates(input);

            var okResult = Assert.IsType<OkObjectResult>(response);
            var returnSession = Assert.IsType<string[]>(okResult.Value);
            Assert.Equal(expected, returnSession[0]);
        }

        [Fact]
        public void GetAssets_ReturnBadRequest_GivenEmptyType()
        {
            var сontroller = GetLookupController();
            var response = сontroller.GetStates("");

            var okResult = Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
