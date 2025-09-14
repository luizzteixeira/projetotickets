using ProjetoTickets.Classes;
using ProjetoTickets.Conexao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTickets
{
    /// <summary>
    /// Ponto de entrada (Main) e camada de apresentacao (UI de Console) da aplicacao.
    /// Direciona o fluxo de navegacao do usuario entre os diferentes modulos do sistema.
    /// </summary>
    /// <remarks>
   
    internal class Program
    {
        /// <summary>
        /// Metodo principal que inicia a aplicacao.
        /// Executa o loop principal do menu e gerencia a navegacao de alto nivel.
        /// </summary>
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sistema de tickets");
                Console.Write("\n");
                Console.WriteLine("1 - Funcionario");
                Console.WriteLine("2 - Tickets");
                Console.WriteLine("3 - Relatorio");
                Console.WriteLine("4 - Sair");
                Console.Write("\n");
                Console.Write("Escolha uma opcao: ");
                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    MenuFuncionario();
                }
                else if (opcao == "2")
                {
                    MenuTickets();
                }
                else if (opcao == "3")
                {
                    MenuRelatorios();
                }
                else if (opcao == "4")
                {
                    Console.WriteLine("\nSaindo do sistema...");
                    break;
                }
                else
                {
                    Console.WriteLine("\nOpcao invalida. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Controla o sub-menu de operacoes relacionadas a entidade Funcionario.
        /// Captura a entrada do usuario e delega as acoes para os metodos estaticos da classe 'Funcionario'.
        /// </summary>
        static void MenuFuncionario()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu de Funcionarios");
                Console.Write("\n");
                Console.WriteLine("1 - Cadastrar");
                Console.WriteLine("2 - Editar");
                Console.WriteLine("9 - Voltar");
                Console.Write("\n");
                Console.Write("Escolha uma opcao: ");
                string opcao = Console.ReadLine();

                if (int.TryParse(opcao, out int id))
                {
                    if (id == 1)
                    {
                        Console.WriteLine("\n--- Cadastrar Funcionario ---");
                        Console.Write("Digite seu Nome: ");
                        string nome = Console.ReadLine();
                        Console.Write("Digite seu CPF: ");
                        string cpf = Console.ReadLine();

                        if (Funcionario.CadastrarFuncionario(nome, cpf, out string erro))
                        {
                            Console.WriteLine("\nSalvo com sucesso");
                        }
                        else
                        {
                            Console.WriteLine("\nFalha ao salvar. " + erro);
                        }
                    }
                    else if (id == 2)
                    {
                        Console.WriteLine("\n--- Editar Funcionario ---");
                        Console.Write("Digite seu CPF: ");
                        string cpf = Console.ReadLine();

                        if (!long.TryParse(cpf, out long cpfvalido))
                        {
                            Console.WriteLine("\nO CPF nao eh valido");
                            Console.ReadKey();
                            continue;
                        }

                        Funcionario funcionario = Funcionario.ResgatarFuncionario(cpfvalido);

                        if (funcionario == null)
                        {
                            Console.WriteLine("\nFuncionario com o CPF informado nao foi encontrado.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.Write("Digite: 1 - Ativo; 2 - Inativo: ");
                        string situacao = Console.ReadLine();

                        if (int.TryParse(situacao, out int situacaoInt))
                        {
                            if (situacaoInt == 1)
                            {
                                funcionario.Situacao = 'A';
                            }
                            else if (situacaoInt == 2)
                            {
                                funcionario.Situacao = 'I';
                            }
                            else
                            {
                                Console.WriteLine("\nOpcao de situacao invalida. Digite 1 ou 2.");
                                Console.ReadKey();
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nEntrada invalida. A situacao deve ser um numero (1 ou 2).");
                            Console.ReadKey();
                            continue;
                        }

                        if (Funcionario.EditarFuncionario(funcionario, out string erro))
                        {
                            Console.WriteLine("\nEditado com sucesso");
                        }
                        else
                        {
                            Console.WriteLine("\nFalha ao editar. " + erro);
                        }
                    }
                    else if (id == 9)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\nOpcao invalida.");
                    }
                }
                else
                {
                    Console.WriteLine("\nOpcao invalida. Digite um numero.");
                }
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Controla o sub-menu de operacoes relacionadas a entidade Ticket.
        /// Captura a entrada do usuario e delega as acoes para os metodos estaticos da classe 'Ticket'.
        /// </summary>
        static void MenuTickets()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("      MENU DE TICKETS     ");
                Console.Write("\n");
                Console.WriteLine("1 - Cadastrar Novo Ticket");
                Console.WriteLine("2 - Consultar Tickets por Funcionario");
                Console.WriteLine("3 - Editar Ticket");
                Console.WriteLine("9 - Voltar ao Menu Principal");
                Console.Write("\n");
                Console.Write("Escolha uma opcao: ");
                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    Console.WriteLine("\n--- Cadastrar Novo Ticket ---");
                    Console.Write("Digite a quantidade: ");
                    string quantidade = Console.ReadLine();
                    Console.Write("Digite o CPF do funcionario para atribuir o ticket: ");
                    string cpf = Console.ReadLine();

                    if (Ticket.CadastrarTicket(quantidade, cpf, out string erro))
                    {
                        Console.WriteLine("\nTicket cadastrado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("\nFalha ao cadastrar o ticket. " + erro);
                    }
                }
                else if (opcao == "9")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("\nOpcao invalida ou nao implementada!");
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Apresenta a interface para o usuario especificar o periodo do relatorio.
        /// Valida as datas fornecidas antes de invocar a logica de geracao do relatorio.
        /// </summary>
        static void MenuRelatorios()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("    RELATORIO DE TICKETS ENTREGUES      ");
            Console.WriteLine("========================================");

            DateTime dataInicio;
            Console.Write("Digite a data de inicio (dd/mm/aaaa): ");
            if (!DateTime.TryParse(Console.ReadLine(), out dataInicio))
            {
                Console.WriteLine("Data de inicio invalida. Pressione qualquer tecla para voltar.");
                Console.ReadKey();
                return;
            }

            DateTime dataFim;
            Console.Write("Digite a data de fim (dd/mm/aaaa): ");
            if (!DateTime.TryParse(Console.ReadLine(), out dataFim))
            {
                Console.WriteLine("Data de fim invalida. Pressione qualquer tecla para voltar.");
                Console.ReadKey();
                return;
            }

            dataFim = dataFim.Date.AddDays(1).AddTicks(-1);

            if (dataInicio > dataFim)
            {
                Console.WriteLine("A data de inicio nao pode ser maior que a data de fim.");
                Console.ReadKey();
                return;
            }

            GerarRelatorioTicketsEntregues(dataInicio, dataFim);

            Console.WriteLine("\nRelatorio gerado. Pressione qualquer tecla para voltar ao menu principal.");
            Console.ReadKey();
        }

        /// <summary>
        /// Contem a logica de negocio para gerar o relatorio de tickets entregues por funcionario em um periodo.
        /// </summary>
        /// <param name="dataInicio">A data inicial do periodo para o relatorio.</param>
        /// <param name="dataFim">A data final do periodo para o relatorio.</param>
        /// <remarks>
       
        static void GerarRelatorioTicketsEntregues(DateTime dataInicio, DateTime dataFim)
        {
            List<Funcionario> todosOsFuncionarios;
            List<Ticket> todosOsTickets;

            try
            {
                todosOsFuncionarios = Funcionario.ResgatarTodos();
                todosOsTickets = Ticket.ResgatarTodos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcorreu um erro ao buscar os dados do banco: {ex.Message}");
                Console.WriteLine("Verifique sua connection string no App.config e a conexao com o banco.");
                return;
            }

            char situacaoEntregue = 'I';

            var ticketsEntreguesNoPeriodo = todosOsTickets.Where(t =>
                t.Situacao == situacaoEntregue &&
                t.IdFuncionario != 0 &&
                t.DataEntrega >= dataInicio &&
                t.DataEntrega <= dataFim
            ).ToList();

            Console.Clear();
            Console.WriteLine($"\n--- EXIBINDO RELATORIO DE {dataInicio:dd/MM/yyyy} ATE {dataFim:dd/MM/yyyy} ---");

            if (!ticketsEntreguesNoPeriodo.Any())
            {
                Console.WriteLine("\nNenhum ticket foi entregue neste periodo.");
                return;
            }

            var ticketsPorFuncionario = ticketsEntreguesNoPeriodo
                .GroupBy(t => t.IdFuncionario);

            foreach (var grupo in ticketsPorFuncionario)
            {
                var funcionario = todosOsFuncionarios.FirstOrDefault(f => f.Id == grupo.Key);
                string nomeFuncionario = funcionario != null ? funcionario.Nome : "Funcionario Desconhecido";
                int totalQuantidadeFuncionario = grupo.Sum(t => t.Quantidade);

                Console.WriteLine("\n-------------------------------------------------");
                Console.WriteLine($"Funcionario: {nomeFuncionario} | Total de Tickets: {grupo.Count()} | Total Quantidade: {totalQuantidadeFuncionario}");
                Console.WriteLine("-------------------------------------------------");

                foreach (var ticket in grupo)
                {
                    Console.WriteLine($"  - Ticket #{ticket.Id} (Quantidade: {ticket.Quantidade}) | Entregue em: {ticket.DataEntrega:dd/MM/yyyy}");
                }
            }

            int totalGeralTickets = ticketsEntreguesNoPeriodo.Count();
            int totalGeralQuantidade = ticketsEntreguesNoPeriodo.Sum(t => t.Quantidade);

            Console.WriteLine("\n=================================================");
            Console.WriteLine($"  TOTAL GERAL DE TICKETS NO PERIODO: {totalGeralTickets}");
            Console.WriteLine($"  SOMA GERAL DE QUANTIDADES NO PERIODO: {totalGeralQuantidade}");
            Console.WriteLine("=================================================");
        }
    }
}