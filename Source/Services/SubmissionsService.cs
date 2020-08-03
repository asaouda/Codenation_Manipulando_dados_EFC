using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly CodenationContext context;
        public SubmissionService(CodenationContext context)
        {
            this.context = context;
        }

        public IList<Submission> FindByChallengeIdAndAccelerationId(int challengeId, int accelerationId)
        {
            var query = from s in context.Submissions
                        join a in context.Accelerations on s.ChallengeId equals a.ChallengeId
                        where a.Id == accelerationId && s.ChallengeId == challengeId

                        select new { userId = s.UserId };
            return context.Submissions.Where(x => query.Select(y => y.userId).Contains(x.UserId) && x.ChallengeId ==challengeId).ToList();
        }

        public decimal FindHigherScoreByChallengeId(int challengeId)
        {
            return context.Submissions.Where(x => x.ChallengeId == challengeId).Max(x => x.Score);
        }

        public Submission Save(Submission submission)
        {

            var _submission = context.Submissions.Where(x => x.ChallengeId == submission.ChallengeId && x.UserId == submission.UserId);
            if (_submission != null)
            {
                context.Entry(submission).State = EntityState.Modified;
                
            }
            else
            {
                if (submission.ChallengeId==null || submission.UserId==null)
                {
                    return new Submission();
                }
                else
                {
                    context.Entry(submission).State = EntityState.Added;
                    context.Add(submission);
                }
            }

            context.SaveChanges();
            return submission;
        }
    }
}
