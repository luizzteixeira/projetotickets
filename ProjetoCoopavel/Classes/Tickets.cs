using ProjetoTickets.Conexao;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProjetoTickets.Classes
{
    /// <summary>
    /// Representa a entidade de um Ticket, contendo suas propriedades e metodos de acesso a dados.
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public int Quantidade { get; set; }
        public char Situacao { get; set; }
        public DateTime DataEntrega { get; set; }

        /// <summary>
        /// Cadastra um novo lote de tickets, associando-o a um funcionario ativo existente via CPF.
        /// </summary>
        /// <remarks>
       
       
        public static bool CadastrarTicket(string quantidadeStr, string cpf, out string erro)
        {
            erro = string.Empty;
            if (string.IsNullOrEmpty(quantidadeStr))
            {
                erro = "A quantidade nao pode ser vazia";
                return false;
            }
            if (!int.TryParse(quantidadeStr, out int quantidade))
            {
                erro = "A quantidade informada nao eh um numero valido";
                return false;
            }
            if (quantidade <= 0)
            {
                erro = "A quantidade deve ser maior que zero.";
                return false;
            }
            if (string.IsNullOrEmpty(cpf) || cpf.Trim().Length != 11 || !long.TryParse(cpf, out long cpfFunc))
            {
                erro = "Nao eh um CPF valido";
                return false;
            }

           
            Funcionario funcionario = Funcionario.ResgatarFuncionario(cpfFunc);
            if (funcionario == null)
            {
                erro = "Funcionario nao encontrado com o CPF informado";
                return false;
            }
            if (funcionario.Situacao != 'A')
            {
                erro = "O funcionario nao esta ativo no sistema e nao pode receber tickets.";
                return false;
            }

            string sql = "INSERT INTO projetotickets.tickets (IdFuncionario, Quantidade) VALUES (@idFunc, @qtd)";
            var parametros = new Dictionary<string, object>
            {
                { "@idFunc", funcionario.Id },
                { "@qtd", quantidade }
            };
           
            return BancoDados.ExecutarComando(sql, out erro, parametros);
        }

        /// <summary>
        /// Altera a situacao de um ticket especifico, identificado pelo seu ID.
        /// </summary>
       
        public static bool EditarTicket(Ticket ticket, out string erro)
        {
            if (ticket.Situacao == 'A' || ticket.Situacao == 'I')
            {
                string sql = "UPDATE projetotickets.tickets SET Situacao = @situacao WHERE Id = @id";
                var parametros = new Dictionary<string, object>
                {
                    { "@situacao", ticket.Situacao },
                    { "@id", ticket.Id }
                };
              
                return BancoDados.ExecutarComando(sql, out erro, parametros);
            }
            erro = "Situacao invalida. Os valores permitidos sao 'A' (Ativo) ou 'I' (Inativo).";
            return false;
        }

        /// <summary>
        /// Busca um unico ticket pelo seu ID.
        /// </summary>
       
        public static Ticket ResgatarTicketEspecifico(int idTicket)
        {
            string sql = "SELECT * FROM projetotickets.tickets WHERE Id = @id";
            var parametros = new Dictionary<string, object>
            {
                { "@id", idTicket }
            };
           
            DataTable dados = BancoDados.Selecionar(sql, parametros, out string _);
            Ticket ticket = null;
            if (dados != null && dados.Rows.Count == 1)
            {
                DataRow linha = dados.Rows[0];
                ticket = new Ticket
                {
                    Id = Convert.ToInt32(linha["Id"]),
                    IdFuncionario = Convert.ToInt32(linha["IdFuncionario"]),
                    Quantidade = Convert.ToInt32(linha["Quantidade"]),
                    Situacao = Convert.ToChar(linha["Situacao"])
                };
                if (linha["DataEntrega"] != DBNull.Value)
                {
                    ticket.DataEntrega = Convert.ToDateTime(linha["DataEntrega"]);
                }
            }
            return ticket;
        }

        /// <summary>
        /// Retorna uma lista de todos os tickets com situacao 'A' (Ativos) de um funcionario especifico.
        /// </summary>
        
        public static List<Ticket> ResgatarTicketsPorFuncionario(int idFuncionario)
        {
            List<Ticket> listaDeTickets = new List<Ticket>();
            string sql = "SELECT * FROM projetotickets.tickets WHERE IdFuncionario = @idFuncionario AND Situacao = 'A'";
            var parametros = new Dictionary<string, object>
            {
                { "@idFuncionario", idFuncionario }
            };
           
            DataTable dados = BancoDados.Selecionar(sql, parametros, out string erro);
            if (dados != null && dados.Rows.Count > 0)
            {
                foreach (DataRow linha in dados.Rows)
                {
                    Ticket ticket = new Ticket
                    {
                        Id = Convert.ToInt32(linha["Id"]),
                        IdFuncionario = Convert.ToInt32(linha["IdFuncionario"]),
                        Quantidade = Convert.ToInt32(linha["Quantidade"]),
                        Situacao = Convert.ToChar(linha["Situacao"])
                    };
                    if (linha["DataEntrega"] != DBNull.Value)
                    {
                        ticket.DataEntrega = Convert.ToDateTime(linha["DataEntrega"]);
                    }
                    listaDeTickets.Add(ticket);
                }
            }
            return listaDeTickets;
        }

        /// <summary>
        /// Recupera todos os registros de tickets da base de dados, independentemente do status ou funcionario.
        /// </summary>
        
        public static List<Ticket> ResgatarTodos()
        {
            List<Ticket> listaDeTickets = new List<Ticket>();
            string sql = "SELECT * FROM projetotickets.tickets";
            
            DataTable dados = BancoDados.Selecionar(sql, null, out string _);

            if (dados != null && dados.Rows.Count > 0)
            {
                foreach (DataRow linha in dados.Rows)
                {
                    Ticket ticket = new Ticket();
                    ticket.Id = Convert.ToInt32(linha["Id"]);
                    ticket.IdFuncionario = Convert.ToInt32(linha["IdFuncionario"]);
                    ticket.Quantidade = Convert.ToInt32(linha["Quantidade"]);
                    ticket.Situacao = Convert.ToChar(linha["Situacao"]);
                    if (linha["DataEntrega"] != DBNull.Value)
                    {
                        ticket.DataEntrega = Convert.ToDateTime(linha["DataEntrega"]);
                    }
                    listaDeTickets.Add(ticket);
                }
            }
            return listaDeTickets;
        }
    }
}