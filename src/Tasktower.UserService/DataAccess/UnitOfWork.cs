using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISession _dbSession;
        private ITransaction _dbTransaction;

        public IUserRepository UserRepository { get; private set; }
        public UnitOfWork(ISession dbSession)
        {
            _dbSession = dbSession; 
            if (_dbSession != null) {
                _dbTransaction = _dbSession.BeginTransaction();
                UserRepository = new UserRepository(_dbSession);
            }
        }

        public void Complete()
        {
            if (_dbSession != null)
            {
                _dbSession.Flush();
                _dbTransaction.Commit();
            }
        }
        public void Rollback()
        {
            if (_dbSession != null )
            {
                _dbTransaction.Rollback();
                CloseDBSession();
            }
        }

        private void CloseDBSession() 
        {
            _dbTransaction.Dispose();
            _dbTransaction = null;
            _dbSession.Close();
            _dbSession.Dispose();
            _dbSession = null;
        }


        public void Dispose()
        {
            if (_dbSession != null)
            {
                UserRepository.Dispose();
                CloseDBSession();
            }
        }
    }
}
