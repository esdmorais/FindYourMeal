using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FindYourMeal.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FindYourMeal.Repositories
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        private string connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        protected IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public abstract void Add(T item);

        public abstract void Remove(int id);

        public abstract void Update(T item);

        public abstract T FindByID(int id);

        public abstract IEnumerable<T> FindAll();
    }
}
