using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoPraticoSmartNetManager.Classes
{
    public class MotorDoSistema
    {
        // Estruturas de Dados Obrigatórias (Encapsuladas)
        private Dictionary<string, string> dhcpReservas = new Dictionary<string, string>();
        private Queue<Dispositivo> filaAutenticacao = new Queue<Dispositivo>();
        private List<Dispositivo> dispositivosAtivos = new List<Dispositivo>();
        private Stack<string> historicoAuditoria = new Stack<string>();

       
        // Missão 1: Servidor DHCP (Reserva de IP)
       
        public void CadastrarReservaDHCP()
        {
            Console.Clear();
            Console.WriteLine("--- CADASTRO DE RESERVA DHCP ");
            Console.Write("Informe o MAC Address (Ex: AA:BB:CC:11:22:33): ");
            string macAdress = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(macAdress))
            {
                Console.WriteLine("\n[ERRO] O MAC Address não pode ser vazio.");
                return;
            }

            // Regra: Impede duplicidade de MAC no Dicionário
            if (dhcpReservas.ContainsKey(macAdress))
            {
                Console.WriteLine($"\n[ERRO] O MAC '{macAdress}' já possui o IP fixo '{dhcpReservas[macAdress]}' cadastrado!");
                return;
            }

            Console.Write("Informe o Endereço IP (Ex: 192.168.0.50): ");
            string ip = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                Console.WriteLine("\n[ERRO] O Endereço IP não pode ser vazio.");
                return;
            }

            dhcpReservas.Add(macAdress, ip);
            Console.WriteLine($"\n Sucesso! IP {ip} reservado para o MAC {macAdress}.");
        }

        public void ListarReservasDHCP()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE RESERVAS DHCP ATIVAS ---");
            if (dhcpReservas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva cadastrada no momento.");
                return;
            }

            foreach (var r in dhcpReservas)
            {
                Console.WriteLine($"MAC: {r.Key}  ==>  IP Fixo: {r.Value}");
            }
        }

        
        // Missão 2: Fila de Autenticação (QoS)
        
        public void AdicionarFilaAutenticacao()
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR DISPOSITIVO À FILA ---");
            Dispositivo d = new Dispositivo();

            Console.Write("Informe o MAC Address: ");
            string macAdress = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(macAdress))
            {
                Console.WriteLine("\n[ERRO] O MAC Address não pode ser vazio.");
                return;
            }
            d.MacAddress = macAdress;

            // Integração com Missão 1: Aplica IP automático se houver reserva DHCP
            if (dhcpReservas.ContainsKey(macAdress))
            {
                d.EnderecoIP = dhcpReservas[macAdress];
                Console.WriteLine($"\n[DHCP] IP fixo aplicado automaticamente: {d.EnderecoIP}");
            }
            else
            {
                Console.Write("Informe o Endereço IP: ");
                d.EnderecoIP = Console.ReadLine()?.Trim();
            }

            Console.Write("Nome do Dispositivo (Ex: PC-Contabilidade): ");
            d.Nome = Console.ReadLine()?.Trim();

            Console.Write("Fabricante (Ex: Dell, Apple, Samsung): ");
            d.Fabricante = Console.ReadLine()?.Trim();

            Console.Write("Consumo de Banda atual (Mbps): ");
            if (double.TryParse(Console.ReadLine(), out double banda))
            {
                d.ConsumoBanda = banda;
            }
            else
            {
                d.ConsumoBanda = 0.0;
            }

            d.DataConexao = DateTime.Now;

            filaAutenticacao.Enqueue(d);
            Console.WriteLine($"\n Dispositivo '{d.Nome}' adicionado à fila de autenticação.");
        }

        public void ProcessarFilaAutenticacao()
        {
            Console.Clear();
            Console.WriteLine("--- PROCESSAR FILA DE AUTENTICAÇÃO ---");

            if (filaAutenticacao.Count == 0)
            {
                Console.WriteLine("A fila de autenticação está vazia.");
                return;
            }

            Dispositivo d = filaAutenticacao.Dequeue();
            dispositivosAtivos.Add(d);

            // Missão 4: Registra a ação na Pilha de Auditoria
            string logMsg = $"[AUTENTICAÇÃO] Dispositivo '{d.Nome}' ({d.MacAddress}) autenticado em {DateTime.Now:HH:mm:ss}.";
            historicoAuditoria.Push(logMsg);

            Console.WriteLine($"\n Sucesso: {logMsg}");
            Console.WriteLine($"Restam {filaAutenticacao.Count} dispositivo(s) na fila.");
        }

        public void ListarFilaAutenticacao()
        {
            Console.Clear();
            Console.WriteLine("--- FILA DE AUTENTICAÇÃO (AGUARDANDO) ---");
            if (filaAutenticacao.Count == 0)
            {
                Console.WriteLine("Nenhum dispositivo na fila de espera.");
                return;
            }

            foreach (var d in filaAutenticacao)
            {
                Console.WriteLine(d.ToString());
            }
        }

      
        // Missão 3: Dashboard de Conexões Ativas
       
        public void ExibirDashboard()
        {
            Console.Clear();            
            Console.WriteLine("==============DASHBOARD DE CONEXÕES ATIVAS===============");
            
            if (dispositivosAtivos.Count == 0)
            {
                Console.WriteLine("\n[AVISO] Nenhum dispositivo ativo na rede no momento.");
                return;
            }

            // Consumo total de banda atual -->> Linq .Sum
            double consumoTotal = dispositivosAtivos.Sum(d => d.ConsumoBanda);
            Console.WriteLine($"\n Consumo Total de Banda Atual: {consumoTotal:F2} Mbps");

            // Top 3 dispositivos que mais consomem internet -->> Linq OrderByDes e  Take
            Console.WriteLine("\n Top 3 Dispositivos com Maior Consumo:");
            var top3 = dispositivosAtivos.OrderByDescending(d => d.ConsumoBanda).Take(3);
            int rank = 1;
            foreach (var d in top3)
            {
                Console.WriteLine($"  {rank}. {d.Nome} ({d.Fabricante}) - {d.ConsumoBanda} Mbps");
                rank++;
            }

            // Contagem por Fabricante -->>  Linq GroupBy
            Console.WriteLine("\n Aparelhos Conectados por Fabricante:");
            var porFabricante = dispositivosAtivos.GroupBy(d => d.Fabricante)
                                                   .Select(g => new { Fabricante = g.Key, Quantidade = g.Count() });
            foreach (var f in porFabricante)
            {
                Console.WriteLine($"  - {f.Fabricante}: {f.Quantidade} aparelho(s)");
            }

            // Alerta de anomalia se consumo buscas > 500 Mbps -->>  Linq Where 

            var anomalias = dispositivosAtivos.Where(d => d.ConsumoBanda > 500).ToList();
            if (anomalias.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n ALERTA DE ANOMALIA DETECTADO! (> 500 Mbps)");
                foreach (var a in anomalias)
                {
                    Console.WriteLine($" Alerta: O dispositivo '{a.Nome}' ({a.EnderecoIP}) consome {a.ConsumoBanda} Mbps!");
                }
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\n Status da Rede: Normal (Nenhuma anomalia detectada).");
            }
            Console.WriteLine("--------------------------------------------------");
        }

        
        // Missão 4: Histórico de Auditoria & Rollback
        
        public void DesfazerUltimaAcao()
        {
            Console.Clear();
            Console.WriteLine("--- HISTÓRICO DE AUDITORIA & ROLLBACK ---");

            if (historicoAuditoria.Count == 0)
            {
                Console.WriteLine("Não há ações registradas no histórico para desfazer.");
                return;
            }

            string ultimaAcaoRevertida = historicoAuditoria.Pop();
            Console.WriteLine($"\n AÇÃO REVERTIDA COM SUCESSO (ROLLBACK EXECUTADO):");
            Console.WriteLine($"   -> {ultimaAcaoRevertida}");
            Console.WriteLine($"\nRestam {historicoAuditoria.Count} ação(ões) no histórico.");
        }
    }
}