using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SigmaTaskAPI.BLL.CandidateServ;
using SigmaTaskAPI.BLL.DtoModels;
using SigmaTaskAPI.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace SigmaTaskAPI.Tests
{
    [TestClass]
    public class CandidateTest
    {

        private string filePath = "C:\\sigma_csv\\test_csv.csv";

        [TestMethod]
        public async Task CheckCandidateIsInsertedAsync()
        {
            // Arrange 

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var candidateService = new CandidateService(new DAL.CSVContext(filePath));

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
            var result = await candidateService.InsertOrUpdate(candidate);

            // Assert 
            Assert.IsTrue(result.Succeeded && result.Note == "Record has been inserted", "Candidate Successfully Inserted");
        }

        [TestMethod]
        public async Task CheckCandidateIsUpdatedAsync()
        {
            // Arrange 
            var candidateService = new CandidateService(new DAL.CSVContext(filePath));

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
            var result = await candidateService.InsertOrUpdate(candidate);

            // Assert 
            Assert.IsTrue(result.Succeeded && result.Note == "Record has been updated", "Candidate Successfully Updated");
        }
        [TestMethod]
        public void TestModelStateEmailMissingValidation()
        {
            var candidate = new CandidateModel
            {
                Email = "",
                FirstName = "name 1",
                LastName = "name 2",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };
            var context = new ValidationContext(candidate, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(candidate, context, results, true);

            // Assert here
            Assert.IsFalse(isModelStateValid);

        }
        [TestMethod]
        public void TestModelStateFirstAndLastNameMissingValidation()
        {
            var candidate = new CandidateModel
            {
                Email = "abc@homail.com",
                FirstName = "",
                LastName = "",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };
            var context = new ValidationContext(candidate, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(candidate, context, results, true);

            // Assert here
            Assert.IsFalse(isModelStateValid);
        }


        [TestMethod]
        public async Task CheckCandidateControllerWithInValidModelStateAsync()
        {
            // Arrange 
            var candidateService = new CandidateService(new DAL.CSVContext(filePath)); 
            var logger = new Logger<CandidateController>(new LoggerFactory());
            var candidateController = new CandidateController(logger, candidateService);

            candidateController.ModelState.AddModelError("Email", "Missing Email");

            var candidate = new CandidateModel
            {
                Email = "",
                FirstName = "name 1",
                LastName = "name 2",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };

            // Act 
            var result = await candidateController.InsertOrUpdate(candidate);
             
            // Assert 
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task CheckCandidateControllerWithValidModelStateAsync()
        {
            // Arrange 
            var candidateService = new CandidateService(new DAL.CSVContext(filePath));
            var logger = new Logger<CandidateController>(new LoggerFactory());
            var candidateController = new CandidateController(logger, candidateService);

            candidateController.ModelState.Clear();

            var candidate = new CandidateModel
            {
                Email = "",
                FirstName = "name 1",
                LastName = "name 2",
                Comment = "comment text",
                TimeInterval = "2AM-3PM",
                GitHub = "https://github.com",
                LinkedIn = "https://github.com",
                PhoneNumber = "+21312312312"
            };

            // Act 
            var result = await candidateController.InsertOrUpdate(candidate);

            // Assert 
            Assert.IsTrue(result.Result is OkObjectResult);
        }
    }
}