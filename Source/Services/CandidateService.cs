using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly CodenationContext context;
        public CandidateService(CodenationContext context)
        {
            this.context = context;    
        }

        public IList<Candidate> FindByAccelerationId(int accelerationId)
        {
            return context.Candidates.Where(x => x.AccelerationId == accelerationId).ToList();
        }

        public IList<Candidate> FindByCompanyId(int companyId)
        {
            return context.Candidates.Where(x => x.CompanyId == companyId).ToList();
        }

        public Candidate FindById(int userId, int accelerationId, int companyId)
        {
            return context.Candidates.Where(x =>  x.UserId == userId && x.AccelerationId == accelerationId && x.CompanyId == companyId).FirstOrDefault();
        }

        public Candidate Save(Candidate candidate)
        {
            var _candidate = FindById(candidate.UserId,candidate.AccelerationId, candidate.CompanyId );
            if (_candidate == null)
            {
                _candidate = candidate;
                context.Entry(_candidate).State = EntityState.Added;
                context.Add(_candidate);
            }
            else
            {
                context.Entry(_candidate).State = EntityState.Modified;
                _candidate.Status = candidate.Status;
                
            }
            context.SaveChanges();
            return candidate;
        }
    }
}
