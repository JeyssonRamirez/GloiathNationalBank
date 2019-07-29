using Core.DataTransferObject;
using Core.Entities;
using System.Collections.Generic;

namespace Data.Common.Definition
{
    public interface IUnitOfWork
    {

        int CommitInt();
        void RollbackChanges();
        void AttachEntity<T>(T item) where T : Entity;
        void AdddEntity<T>(T item) where T : Entity;
        void RemoveEntity<T>(T item) where T : Entity;
        int ExecuteQuery(string query, List<ParameterDto> parameters, bool procedure);
    }
}
