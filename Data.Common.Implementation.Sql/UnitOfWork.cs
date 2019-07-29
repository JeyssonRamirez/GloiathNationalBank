using Core.DataTransferObject;
using Core.Entities;
using Data.Common.Definition;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace Data.Common.Implementation.Sql
{
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        public int CommitInt()
        {
            return SaveChanges();
        }

        public void RollbackChanges()
        {
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void AttachEntity<T>(T item) where T : Entity
        {
            Attach(item);
        }
        public void AdddEntity<T>(T item) where T : Entity
        {
            this.Add(item);
        }

        public void RemoveEntity<T>(T item) where T : Entity
        {
            this.Remove(item);
        }

        public int ExecuteQuery(string query, List<ParameterDto> parameters, bool procedure = false)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GNBCnxString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.CommandType = procedure ? CommandType.StoredProcedure : CommandType.Text;
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter.ParameterName, (SqlDbType)(int)parameter.DbType).Value = parameter.Value;
                    }
                    cmd.CommandTimeout = 0;
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
