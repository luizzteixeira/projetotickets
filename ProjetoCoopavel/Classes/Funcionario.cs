using ProjetoTickets.Conexao;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProjetoTickets.Classes
{
    /// <summary>
    /// Representa a entidade Funcionario, contendo suas propriedades e metodos de acesso a dados.
    /// </summary>
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public long Cpf { get; set; }
        public char Situacao { get; set; }
        public DateTime DataAlteracao { get; set; }

        public Funcionario() { }

        /// <summary>
        /// Valida as regras de negocio e, se aprovado, insere um novo funcionario no banco de dados.
        /// </summary>
        /// <remarks>
        
        public static bool CadastrarFuncionario(string nomeFuncionario, string cpfFuncionario, out string erro)
        {
            if (string.IsNullOrEmpty(nomeFuncionario))
            {
                erro = "Nome nao pode ser vazio";
                return false;
            }

            if (nomeFuncionario.Length > 90)
            {
                erro = "Nome so pode ter ate 90 caracteres";
                return false;
            }

            if (string.IsNullOrEmpty(cpfFuncionario)
                || cpfFuncionario.Trim().Length != 11
                || !long.TryParse(cpfFuncionario, out long cpfFunc))
            {
                erro = "Nao eh um CPF valido";
                return false;
            }

          
            if (ResgatarFuncionario(cpfFunc) != null)
            {
                erro = "CPF ja cadastrado";
                return false;
            }

            string sql = "INSERT INTO projetotickets.funcionario (Nome, Cpf) VALUES (@nome, @cpf)";
            var parametros = new Dictionary<string, object>
            {
                { "@nome", nomeFuncionario },
                { "@cpf", cpfFunc }
            };
           
            return BancoDados.ExecutarComando(sql, out erro, parametros);
        }

        /// <summary>
        /// Busca um funcionario unico utilizando o CPF como chave de busca.
        /// </summary>
        /// <remarks>
       
        public static Funcionario ResgatarFuncionario(long cpf)
        {
            string sql = "SELECT * FROM projetotickets.funcionario WHERE Cpf = @cpf";
            var parametros = new Dictionary<string, object>
            {
                { "@cpf", cpf }
            };

           
            DataTable dados = BancoDados.Selecionar(sql, parametros, out string _);
            Funcionario funcionario = null;
            if (dados != null && dados.Rows.Count == 1)
            {
                DataRow linha = dados.Rows[0];
                funcionario = new Funcionario();
                funcionario.Id = Convert.ToInt32(linha["Id"]);
                funcionario.Nome = linha["Nome"].ToString();
                funcionario.Cpf = Convert.ToInt64(linha["Cpf"]);
                funcionario.Situacao = Convert.ToChar(linha["Situacao"]);

                if (linha["DataAlteracao"] != DBNull.Value)
                {
                    funcionario.DataAlteracao = Convert.ToDateTime(linha["DataAlteracao"]);
                }
            }

            return funcionario;
        }

        /// <summary>
        /// Altera a situacao cadastral (Ativo/Inativo) de um funcionario existente.
        /// </summary>
        /// <remarks>
        
        public static bool EditarFuncionario(Funcionario funcionario, out string erro)
        {
            if (funcionario.Situacao == 'A' || funcionario.Situacao == 'I')
            {
                string sql = "UPDATE projetotickets.funcionario SET Situacao = @situacao WHERE Cpf = @cpf";
                var parametros = new Dictionary<string, object>
                {
                    { "@situacao", funcionario.Situacao },
                    { "@cpf", funcionario.Cpf }
                };
                
                return BancoDados.ExecutarComando(sql, out erro, parametros);
            }

            erro = "situacao deve ser A ou I";
            return false;
        }

        /// <summary>
        /// Recupera todos os registros de funcionarios da base de dados.
        /// </summary>
        /// <remarks>
       
        public static List<Funcionario> ResgatarTodos()
        {
            List<Funcionario> listaDeFuncionarios = new List<Funcionario>();
            string sql = "SELECT * FROM projetotickets.funcionario";
           
            DataTable dados = BancoDados.Selecionar(sql, null, out string _);

            if (dados != null && dados.Rows.Count > 0)
            {
                foreach (DataRow linha in dados.Rows)
                {
                    Funcionario funcionario = new Funcionario();
                    funcionario.Id = Convert.ToInt32(linha["Id"]);
                    funcionario.Nome = linha["Nome"].ToString();
                    funcionario.Cpf = Convert.ToInt64(linha["Cpf"]);
                    funcionario.Situacao = Convert.ToChar(linha["Situacao"]);

                    if (linha["DataAlteracao"] != DBNull.Value)
                    {
                        funcionario.DataAlteracao = Convert.ToDateTime(linha["DataAlteracao"]);
                    }
                    listaDeFuncionarios.Add(funcionario);
                }
            }
            return listaDeFuncionarios;
        }
    }
}