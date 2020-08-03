using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly CodenationContext context;
        public CompanyService(CodenationContext context)
        {
            this.context = context;
        }

        public IList<Company> FindByAccelerationId(int accelerationId)
        {
            return context.Candidates.Where(x => x.AccelerationId == accelerationId)
                .Include(x => x.Company)
                .Select(x => x.Company)
                .ToList();
            
        }


        public Company FindById(int id)
        {
            return context.Companies.Where(x => x.Id == id).FirstOrDefault();
        }

        public IList<Company> FindByUserId(int userId)
        {
            var result = context.Candidates.Where(x => x.UserId == userId)
                .Select(x => x.Company)
                .Distinct()
                .ToList();
            return result;
        }

        public Company Save(Company company)
        {
            if (company.Id == 0)
            {
                context.Entry(company).State= EntityState.Added;
                context.Add(company);
            }
            else
            {
                context.Entry(company).State = EntityState.Modified;
            }
            
            context.SaveChanges();
            return company;
        }
    }
}