using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration; 
using System.Data;

namespace ProjetoTickets.Conexao
{
    /// <summary>
    /// Classe utilitaria estatica que centraliza e abstrai o acesso ao banco de dados MySQL.
    /// </summary>
    public static class BancoDados 
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ProjetoticketsDB"].ConnectionString;

        /// <summary>
        /// Executa uma query do tipo SELECT, com suporte a parametros, e retorna os resultados.
        /// </summary>
        /// <returns>
        /// Retorna um 'DataTable' com os registros. Em caso de erro, retorna um DataTable vazio para evitar NullReferenceException.
        /// </returns>
        public static DataTable Selecionar(string comando, Dictionary<string, object> parametros, out string erro)
        {
            erro = string.Empty;
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(comando, connection))
                    {
                        if (parametros != null)
                        {
                            foreach (var parametro in parametros)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return new DataTable();
            }
            return dataTable;
        }

        /// <summary>
        /// Executa um comando SQL que nao retorna dados (INSERT, UPDATE, DELETE), com suporte opcional a parametros.
        /// </summary>
        /// <returns>
        /// Retorna 'true' se uma ou mais linhas foram afetadas, 'false' caso contrario ou em caso de erro.
        /// </returns>
        public static bool ExecutarComando(string comando, out string erro, Dictionary<string, object> parametros = null)
        {
            erro = string.Empty;
            int linhasAfetadas = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(comando, connection))
                    {
                        if (parametros != null)
                        {
                            foreach (var parametro in parametros)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }
                        linhasAfetadas = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
            return linhasAfetadas > 0;
        }
    }
}