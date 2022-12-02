using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SigmaTaskAPI.BLL.CandidateServ;
using SigmaTaskAPI.BLL.DtoModels;
using SigmaTaskAPI.Controllers;
using System.Threading.Tasks;

namespace SigmaTaskAPI.Tests
{
    [TestClass]
    public class CandidateTest
    {
        [TestMethod]
        public async Task CheckCandidateIsInsertedAsync()
        {
            // Arrange 
            var candidateService = new Mock<ICandidateService>();

            var candidate = new CandidateModel 
            { 
                Email = "testemail@mail.com",
                FirstName = "name 1",
                LastName = "name 2",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };

            // Act 
            var result = await candidateService.Object.InsertOrUpdate(candidate);

            // Assert 
            Assert.IsTrue(result.Succeeded && result.Note == "Record has been inserted");
        }

        public async Task CheckCandidateIsUpdatedAsync()
        {
            // Arrange 
            var candidateService = new Mock<ICandidateService>();

            var candidate = new CandidateModel
            {
                Email = "testemail@mail.com",
                FirstName = "name 1",
                LastName = "name 2",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };

            // Act 
            var result = await candidateService.Object.InsertOrUpdate(candidate);

            // Assert 
            Assert.IsTrue(result.Succeeded && result.Note == "Record has been updated");
        }

    }
}