using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly CodenationContext _context;
        public ChallengeService(CodenationContext context)
        {
            this._context = context;
        }

        public IList<Models.Challenge> FindByAccelerationIdAndUserId(int accelerationId, int userId)
        {

            //var result = (from ch in _context.Challenges
            //              join a in _context.Accelerations.DefaultIfEmpty().Where(x=>x.Id==accelerationId) on ch.Id equals a.ChallengeId

            //              join  s in _context.Submissions.DefaultIfEmpty().Where(x=>x.UserId==userId) on ch.Id equals s.ChallengeId
            //              select ch).Distinct().ToList();

            //return result;
            return _context.Users.
                Where(x => x.Id == userId).
                SelectMany(x => x.Candidates).
                Where(x => x.AccelerationId == accelerationId).
                Select(x => x.Acceleration.Challenge).
                Distinct().
                ToList();


        }

        public Models.Challenge Save(Models.Challenge challenge)
        {
            if (challenge.Id == 0)
            {
                _context.Entry(challenge).State = EntityState.Added ;
                _context.Add(challenge);
            }
            else
            {
                _context.Entry(challenge).State = EntityState.Modified; 
            }
            _context.SaveChanges();
            return challenge;
        }
    }
}