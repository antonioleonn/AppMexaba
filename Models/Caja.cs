using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AppMexaba.Models
{
    public class Caja
    {
        public string Nombre { get; set; }
        public string Ip { get; set; }
        public bool Activa { get; private set; }

        public Caja(string nombre, string ip)
        {
            Nombre = nombre;
            Ip = ip;
        }

        public async Task VerificarEstadoAsync()
        {
            try
            {
                using var ping = new System.Net.NetworkInformation.Ping();
                var reply = await ping.SendPingAsync(Ip, 2000);
                Activa = reply.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
            catch
            {
                Activa = false;
            }
        }
    }
}
