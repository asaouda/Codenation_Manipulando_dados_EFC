using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class AccelerationService : IAccelerationService
    {
        private readonly CodenationContext context;
        public AccelerationService(CodenationContext context)
        {
            this.context = context;
        }

        public IList<Acceleration> FindByCompanyId(int companyId)
        {
            // var query = context.Accelerations.Where(x => (context.Candidates.Where(y => y.AccelerationId == x.Id && y.CompanyId = companyId))).toList();
            var query = from a in context.Accelerations
                        join c in context.Candidates
                          on a.Id equals c.AccelerationId
                          where c.CompanyId==companyId
                          select new { Id = a.Id};
            return context.Accelerations.Where(x => query.Select(y=>y.Id).Contains(x.Id)).ToList();
        }

        public Acceleration FindById(int id)
        {
            return context.Accelerations.Where(x => x.Id == id).FirstOrDefault();

        }

        public Acceleration Save(Acceleration acceleration)
        {
            var _acceleration = context.Accelerations.Find(acceleration.Id);

            if (_acceleration == null)
            {
                _acceleration = acceleration;
                context.Entry(_acceleration).State = EntityState.Added;
                context.Accelerations.Add(_acceleration);
            }
            else
            { 
                context.Entry(_acceleration).State = EntityState.Modified;
                _acceleration.Name = acceleration.Name;
                _acceleration.Slug = acceleration.Slug;
                
            }

            context.SaveChanges();
            
            return _acceleration;

        }
    }
}
