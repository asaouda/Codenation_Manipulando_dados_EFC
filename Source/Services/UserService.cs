using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class UserService : IUserService
    {
        private readonly CodenationContext _context;
        public UserService(CodenationContext context)
        {
            this._context = context;
        }

        public IList<User> FindByAccelerationName(string name)
        {
            var query = from u in _context.Users
                        join c in _context.Candidates on u.Id equals c.UserId
                        join a in _context.Accelerations.Distinct()
                          on c.AccelerationId equals a.Id
                        where a.Name==name
                        
                        select new { Id = u.Id };
            var result= _context.Users.Where(x => query.Select(y => y.Id).Contains(x.Id)).ToList();
            return _context.Users.Where(x => query.Select(y => y.Id).Contains(x.Id)).ToList();
        }

        public IList<User> FindByCompanyId(int companyId)
        {
            var query = from a in _context.Users
                        join c in _context.Candidates
                          on a.Id equals c.UserId
                        where c.CompanyId== companyId
                        select new { Id = a.Id };
            return _context.Users.Where(x =>query.Select(y => y.Id).Contains(x.Id)).ToList();
            //return _context.Users.Where(x=>x.Candidates)
        }

        public User FindById(int id)
        {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public User Save(User user)
        {
            var eUser = _context.Users.Find(user.Id);

            if (eUser == null)
            {
                eUser = user;
                _context.Users.Add(eUser);
                _context.Entry(eUser).State = EntityState.Added;
            }
            else
            {
                _context.Entry(eUser).State = EntityState.Modified;
                eUser.Nickname = eUser.Nickname;
                eUser.Password = eUser.Password;
                eUser.FullName = eUser.FullName;
                eUser.Email = eUser.Email;
            }

            _context.SaveChanges();
            return user;

        }
    }
}
