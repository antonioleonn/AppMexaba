using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AppMexaba.Models
{
    public class Sucursal
    {
        public string Nombre { get; }
        public string IpServidor { get; }
        public string IpInternet { get; }
        public bool ServidorActivo { get; private set; }
        public bool InternetActivo { get; private set; }
        public List<Caja>? Cajas { get; set; }  // ← Añadido

        public Sucursal(string nombre, string ipServidor, string ipInternet)
        {
            Nombre = nombre;
            IpServidor = ipServidor;
            IpInternet = ipInternet;
            Cajas = new();  // ← Inicialización opcional
        }

        public async Task VerificarEstadoAsync()
        {
            ServidorActivo = await HacerPing(IpServidor);
            InternetActivo = await HacerPing(IpInternet);

            if (Cajas != null)
            {
                foreach (var caja in Cajas)
                {
                    await caja.VerificarEstadoAsync();
                }
            }
        }

        private async Task<bool> HacerPing(string ip)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(ip, 2000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
