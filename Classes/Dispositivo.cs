using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoPraticoSmartNetManager.Classes
{
    public class Dispositivo
    {
        public string MacAddress { get; set; }
        public string EnderecoIP { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public double ConsumoBanda { get; set; }
        public DateTime DataConexao { get; set; }

        public string ToString()
        {
            return $"Dispositivo: {Nome}, MAC: {MacAddress}, IP: {EnderecoIP}, Fabricante: {Fabricante}, Consumo de Banda: {ConsumoBanda} Mbps, Data de Conexão: {DataConexao}";
        }

    }
}

