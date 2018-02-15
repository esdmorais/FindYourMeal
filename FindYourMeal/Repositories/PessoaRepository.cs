﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FindYourMeal.Models;
using FindYourMeal.Repositories;
using System.Data;
using Npgsql;
using Dapper;

namespace FindYourMeal.Repositories
{
    public class PessoaRepository : BaseRepository<Pessoa>
    {
        private string connectionString;
        public PessoaRepository(IConfiguration configuration)
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

        public void Add(Pessoa pessoa)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                IDbTransaction transacao = dbConnection.BeginTransaction();

                try
                {
                    pessoa.ID = Convert.ToInt32(dbConnection.ExecuteScalar("INSERT INTO Pessoa (Nome, Telefone) VALUES (@Nome, @Telefone) RETURNING ID ", pessoa));

                    string restaurantesIDs = string.Join(",", pessoa.Preferencias.Select(a => a.ID).ToArray());

                    dbConnection.Execute(@"INSERT INTO Preferencias (PessoaID, RestauranteID) 
                                              SELECT @PessoaID, ID 
                                                FROM Restaurante 
                                               WHERE ID IN (" + restaurantesIDs + ")", new { PessoaID = pessoa.ID, IDs = pessoa.Preferencias.Select(a => a.ID).ToArray() });
                    transacao.Commit();
                }
                catch (Exception e)
                {
                    transacao.Rollback();
                    throw new Exception("Erro ao adicionar uma pessoa.", e);
                }
                finally
                {                    
                    dbConnection.Close();
                }
            }
        }

        public IEnumerable<Pessoa> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                IEnumerable<Pessoa> retorno;

                dbConnection.Open();
                retorno = dbConnection.Query<Pessoa>("SELECT ID, Nome, Telefone FROM Pessoa ORDER BY Nome");
                dbConnection.Close();

                return retorno;
            }
        }

        public Pessoa FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                Pessoa pessoa;
                dbConnection.Open();
                pessoa = dbConnection.Query<Pessoa>("SELECT ID, Nome, Telefone FROM Pessoa WHERE ID = @Id", new { Id = id }).FirstOrDefault();

                pessoa.Preferencias = dbConnection.Query<Restaurante>(
                    " SELECT Restaurante.ID, Restaurante.Nome " +
                    "   FROM Restaurante " +
                    "   JOIN Preferencias " +
                    "     ON Preferencias.RestauranteID = Restaurante.ID " +
                    "  WHERE Preferencias.PessoaID = @PessoaID", new { PessoaID = pessoa.ID }).AsList<Restaurante>();

                dbConnection.Close();
                return pessoa;
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Pessoa WHERE ID = @ID", new { ID = id });
                dbConnection.Close();
            }
        }

        public void Update(Pessoa pessoa)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE Pessoa SET Nome = @Nome WHERE Id = @ID", pessoa);
                dbConnection.Close();

                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Preferencias WHERE PessoaID = @PessoaID", new { PessoaID = pessoa.ID });
                dbConnection.Close();

                string sqlInsert = "INSERT INTO Preferencias (PessoaID, RestauranteID) values (@PessoaID, @RestauranteID)";

                foreach (var restaurante in pessoa.Preferencias)
                {
                    dbConnection.Open();
                    dbConnection.Execute(sqlInsert, new { PessoaID = pessoa.ID, RestauranteID = restaurante.ID });
                    dbConnection.Close();
                }
            }
        }
    }
}