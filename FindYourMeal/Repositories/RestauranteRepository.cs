using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Npgsql;
using FindYourMeal.Models;

namespace FindYourMeal.Repositories
{
    public class RestauranteRepository : BaseRepository<Restaurante>
    {
        private string connectionString;
        public RestauranteRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public void Add(Restaurante restaurante)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO Restaurante (Nome) VALUES (@Nome)", restaurante);
            }
        }

        public IEnumerable<Restaurante> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Restaurante>("SELECT ID, Nome FROM Restaurante ORDER BY Nome");
            }
        }

        public Restaurante FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Restaurante>("SELECT ID, Nome FROM Restaurante WHERE ID = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Restaurante WHERE Id = @Id", new { Id = id });
                dbConnection.Close();
            }
        }

        public List<Restaurante> FindOpcoesDasPessoas(List<Pessoa> pessoas)
        {
            using (IDbConnection dbConnection = Connection)
            {
                List<Restaurante> restaurantes;
                string pessoasIDs = string.Join(",", pessoas.Select(a => a.ID).ToArray());

                dbConnection.Open();

                restaurantes = dbConnection.Query<Restaurante>(@"
                      SELECT Restaurante.ID, Restaurante.Nome
                        FROM Restaurante
                        JOIN Preferencias
                          ON Preferencias.RestauranteID = Restaurante.ID
                       WHERE Preferencias.PessoaID IN (" + pessoasIDs + @")
                       GROUP BY Restaurante.ID, Restaurante.Nome
                      HAVING COUNT(Preferencias.RestauranteID)  = (SELECT COUNT(ID) Qtd 
                                                                     FROM Pessoa 
                                                                    WHERE ID IN (" + pessoasIDs + "))").AsList<Restaurante>();
                dbConnection.Close();

                return restaurantes;
            }
        }

        public void Update(Restaurante restaurante)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE Restaurante SET Nome = @Nome WHERE Id = @Id", restaurante);
                dbConnection.Close();
            }
        }
    }
}