using AppMexaba.Dtos;
using System.Data;
using System.Data.SqlClient;


namespace AppMexaba.Services
{
    // Services/VentasVipService.cs
    public class VentasVipService
    {
        private readonly IConfiguration _config;

        public VentasVipService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<VentasVipDto>> ObtenerVentasVip(string sucursal, string fecha, string vendedor)
        {
            var lista = new List<VentasVipDto>();
            using var conn = new SqlConnection(_config.GetConnectionString("ConexionSQL"));
            using var cmd = new SqlCommand("sp_VentasVIPC", conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 120 // segundos
            };


            cmd.Parameters.AddWithValue("@suc", sucursal);
            cmd.Parameters.AddWithValue("@fec_fin_mes", fecha);
            cmd.Parameters.AddWithValue("@Vendedor", vendedor);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new VentasVipDto
                {
                    SUCURSAL = reader["SUCURSAL"].ToString(),
                    VENDEDOR = reader["VENDEDOR"].ToString(),
                    NOMBRE_VENDEDOR = reader["NOMBRE VENDEDOR"].ToString(),
                    CLIENTE = reader["CLIENTE"].ToString(),
                    NOMBRE = reader["NOMBRE"].ToString(),
                    WHATSAPP = reader["WHATSAPP"].ToString(),
                    GIRO = reader["GIRO"].ToString(),
                    ULTIMA_VENTA = Convert.ToInt32(reader["ULTIMA VENTA"]),
                    VENTA = Convert.ToDecimal(reader["VENTA"]),
                    CUOTA = Convert.ToDecimal(reader["CUOTA"]),
                    DIFERENCIA = Convert.ToDecimal(reader["DIFERENCIA"]),
                    ESTATUS = reader["ESTATUS"].ToString(),
                    VENTA_MES_ANTERIOR = Convert.ToDecimal(reader["VENTA MES ANTERIOR"]),
                    MES_ANTERIOR = reader["MES ANTERIOR"].ToString(),
                    VENTA_2_MESES_ATRAS = Convert.ToDecimal(reader["VENTA 2 MESES ATRAS"]),
                    DOS_MESES_ATRAS = reader["2 MESES ATRAS"].ToString()
                });
            }

            return lista;
        }

        public async Task<bool> ActualizarWhatsAppCliente(string cliente, string nuevoWhatsApp)
        {
            try
            {
                using var conn = new SqlConnection(_config.GetConnectionString("ConexionSQL"));
                using var cmd = new SqlCommand("sp_ActualizarWhatsAppCliente", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CodCliente", cliente);
                cmd.Parameters.AddWithValue("@NuevoWhatsApp", nuevoWhatsApp);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch
            {
                // Puedes loguear el error si lo deseas
                return false;
            }
        }


        public async Task<string> ObtenerSucursalPorCliente(string codCliente)
        {
            try
            {
                using var conn = new SqlConnection(_config.GetConnectionString("ConexionSQL"));
                using var cmd = new SqlCommand("SELECT TOP 1 suc FROM cxccli WHERE cve = @cve", conn);
                cmd.Parameters.AddWithValue("@cve", codCliente);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado?.ToString()?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }




    }

}
