﻿using Dommel;
using Npgsql;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public abstract class RepositorioBase<T> where T : EntidadeBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioBase(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected virtual IDbConnection ObterConexao()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSerap);
            conexao.Open();
            return conexao;
        }

        public virtual async Task<IEnumerable<T>> ObterTudoAsync()
        {
            using var conexao = ObterConexaoLeitura();
            try
            {
                return await conexao.GetAllAsync<T>();
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        protected virtual IDbConnection ObterConexaoLeitura()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSerapLeitura);
            conexao.Open();
            return conexao;
        }

        protected IDbConnection ObterConexaoSgp()
        {
            var conexao = new NpgsqlConnection(connectionStrings.ApiSgp);
            conexao.Open();
            return conexao;
        }

        public virtual async Task<T> ObterPorIdAsync(long id)
        {
            using var conexao = ObterConexaoLeitura();
            try
            {
                return await conexao.GetAsync<T>(id: id);
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        public virtual async Task<long> SalvarAsync(T entidade)
        {
            var conexao = ObterConexao();
            try
            {
                if (entidade.Id > 0)
                {
                    conexao.Update(entidade);
                }
                else
                {
                    entidade.Id = (long)await conexao.InsertAsync(entidade);
                }
                return entidade.Id;
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        public virtual async Task<long> UpdateAsync(T entidade)
        {
            var conexao = ObterConexao();
            try
            {
                await conexao.UpdateAsync(entidade);

                return entidade.Id;
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }

        public virtual async Task<long> IncluirAsync(T entidade)
        {
            var conexao = ObterConexao();
            try
            {
                entidade.Id = (long)await conexao.InsertAsync(entidade);
                return entidade.Id;
            }
            finally
            {
                conexao.Close();
                conexao.Dispose();
            }
        }
    }
}