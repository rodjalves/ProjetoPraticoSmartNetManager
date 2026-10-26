using System.Security.Cryptography;

namespace ProjetoPraticoSmartNetManager.Classes
{
    public class Program
    {
        static void Main(string[] args)
        {
            MotorDoSistema motor1 =  new MotorDoSistema();
            int opcao = 0;

            do
            {
                Console.WriteLine("1. Cadastrar Reserva DHCP                         ");
                Console.WriteLine("2. Listar Reservas DHCP                           ");
                Console.WriteLine("3. Adicionar Dispositivo à Fila                   ");
                Console.WriteLine("4. Listar Fila de Autenticação                    ");
                Console.WriteLine("5. Processar Fila de Autenticação                 ");
                Console.WriteLine("6. Exibir Dashboard Estatístico                   ");
                Console.WriteLine("7. Desfazer Última Ação (Rollback)          ]     ");
                Console.WriteLine("8. Sair                                           ");
                Console.WriteLine("--------------------------------------------------");
                Console.Write("Escolha uma opção: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            motor1.CadastrarReservaDHCP();
                            break;
                        case 2:
                            motor1.ListarReservasDHCP();
                            break;
                        case 3:
                            motor1.AdicionarFilaAutenticacao();
                            break;
                        case 4:
                            motor1.ListarFilaAutenticacao();
                            break;
                        case 5:
                            motor1.ProcessarFilaAutenticacao();
                            break;
                        case 6:
                            motor1.ExibirDashboard();
                            break;
                        case 7:
                            motor1.DesfazerUltimaAcao();
                            break;
                        case 8:
                            Console.WriteLine("\nEncerrando o SmartNet Manager. Até logo!");
                            break;
                        default:
                            Console.WriteLine("\n[ERRO] Opção inválida! Escolha entre 1 e 8.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n[ERRO] Entrada inválida! Digite apenas números inteiros.");
                }

                if (opcao != 8)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcao != 8);
        }
    }
}