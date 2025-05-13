using AppMexaba.Dtos;
using AppMexaba.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppMexaba.Servicios
{
    public class AbastoPorCapaService : IAbastoPorCapaService
    {
        private readonly IConfiguration _config;

        public AbastoPorCapaService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<string>> ObtenerSucursales()
        {
            // Si lo quieres dinámico, luego hacemos un SELECT DISTINCT
            return await Task.FromResult(new List<string> { "ABE1", "ACT1", "CAB1", "CER1", "CMA1", "COM1", "LOM1", "TRV1", "ZAB1", "ZOB1", "ACA1", "MIB1", "MIN1", "ORI1", "SAB1", "SAN1", "TBC1", "BEB2", "TEB1", "TEZ1", "TUX1", "XAC1", "XAL1", "ZAM1", "ACC1", "CCC1", "ICC1", "XCC1", "UCC1", "VCC1", "ZOC1", "CEC1", "CMI1" });
        }

        public async Task<List<AbastoCapaDto>> ObtenerDetallePorSucursal(string sucursal)
        {
            var lista = new List<AbastoCapaDto>();

            using var conn = new SqlConnection(_config.GetConnectionString("ConexionSQL"));
            using var cmd = new SqlCommand("sp_AbastoPorCapasSucursal", conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 120
            };

            cmd.Parameters.AddWithValue("@Sucursal", sucursal);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new AbastoCapaDto
                {
                    Stat = reader["Stat"]?.ToString(),
                    Clas = reader["Clas"]?.ToString(),
                    Almacen = reader["Almacen"]?.ToString(),
                    Proveedor = reader["Proveedor"]?.ToString(),
                    Codigo = reader["Codigo"]?.ToString(),
                    Descripcion = reader["Descripcion"]?.ToString(),
                    Origen = reader["Origen"]?.ToString(),
                    VtaMens = reader.GetDecimal(reader.GetOrdinal("VtaMens")),
                    VtaProm = reader.GetDecimal(reader.GetOrdinal("VtaProm")),
                    Existencia = reader.GetDecimal(reader.GetOrdinal("Existencia")),
                    BackOrder = reader.GetDecimal(reader.GetOrdinal("BackOrder")),
                    CostoProm = reader.GetDecimal(reader.GetOrdinal("CostoProm")),
                    Margen = reader.GetDecimal(reader.GetOrdinal("Margen")),
                    VtaPromDinero = reader.GetDecimal(reader.GetOrdinal("$VtaProm")),
                    ValorInv = reader.GetDecimal(reader.GetOrdinal("ValorInv")),
                    DiasInv = reader.GetDecimal(reader.GetOrdinal("DiasInv")),
                    Sugerido = reader.GetDecimal(reader.GetOrdinal("Sugerido")),
                    Nivel = reader.GetInt32(reader.GetOrdinal("Nivel")),
                    Capas = reader["Capas"]?.ToString(),
                    TipoImpuesto = reader["TipoImpuesto"]?.ToString()
                });
            }

            return lista;
        }
    }
}
