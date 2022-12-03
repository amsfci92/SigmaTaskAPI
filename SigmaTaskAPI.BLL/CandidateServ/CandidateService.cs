
using SigmaTaskAPI.BLL.DtoModels;
using SigmaTaskAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTaskAPI.BLL.CandidateServ
{
    public class CandidateService : ICandidateService
    {
        private CSVContext _context;

        public CandidateService(CSVContext context)
        {
            _context = context;
        }

        public async Task<Result> InsertOrUpdate(CandidateModel candidateModel)
        {
            var result = new Result();

            var foundCandidate = _context.Candidates.ToList().Where(m => m.Email == candidateModel.Email.ToString()).FirstOrDefault();
            if (foundCandidate == null)
            {
                _context.Candidates.Add(new DAL.Models.Candidate
                {
                    Email = candidateModel.Email,
                    TimeInterval = candidateModel.TimeInterval,
                    Comment = candidateModel.Comment,
                    FirstName = candidateModel.FirstName,
                    GitHub = candidateModel.GitHub,
                    LastName = candidateModel.LastName,
                    LinkedIn = candidateModel.LinkedIn,
                    PhoneNumber = candidateModel.PhoneNumber,
                });

                result.Note = "Record has been inserted";
            }
            else
            {
                foundCandidate.Email = candidateModel.Email;
                foundCandidate.TimeInterval = candidateModel.TimeInterval;
                foundCandidate.Comment = candidateModel.Comment;
                foundCandidate.FirstName = candidateModel.FirstName;
                foundCandidate.GitHub = candidateModel.GitHub;
                foundCandidate.LastName = candidateModel.LastName;
                foundCandidate.LinkedIn = candidateModel.LinkedIn;
                foundCandidate.PhoneNumber = candidateModel.PhoneNumber;


                result.Note = "Record has been updated";
            }

            var saveResult = await _context.SaveChanges();

            if (saveResult)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Note = string.Empty;
                result.Errors.Add(new Error { Code = "CND00001", Message = "Can not save candadate" });
            }
            return result;
        }
    }
}
