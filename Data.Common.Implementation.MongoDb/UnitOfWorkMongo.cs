using Core.DataTransferObject;
using Core.Entities;
using Data.Common.Definition;
using System.Collections.Generic;

namespace Data.Common.Implementation.MongoDb
{
    public class UnitOfWorkMongo : IUnitOfWork
    {
        public void AdddEntity<T>(T item) where T : Entity
        {
            throw new System.NotImplementedException();
        }

        public void AttachEntity<T>(T item) where T : Entity
        {
            throw new System.NotImplementedException();
        }

        public int CommitInt()
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteQuery(string query, List<ParameterDto> parameters, bool procedure)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEntity<T>(T item) where T : Entity
        {
            throw new System.NotImplementedException();
        }

        public void RollbackChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}
