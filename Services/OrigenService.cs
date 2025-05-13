using AppMexaba.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppMexaba.Services
{
    public class OrigenService : IOrigenService
    {
        private readonly string _connectionString;
        private readonly AppDbContext _context;

        public OrigenService(IConfiguration configuration, AppDbContext context)
        {
            _connectionString = configuration.GetConnectionString("ConexionSQL");
            _context = context;
        }

        public async Task InsertarOrigenesAsync(List<alm_art> origenes)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                foreach (var origen in origenes)
                {
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO alm_art (alm, salm, art, par) 
                                                              VALUES (@alm, @salm, @art, @par)", conn))
                    {
                        cmd.Parameters.AddWithValue("@alm", origen.alm);
                        cmd.Parameters.AddWithValue("@salm", origen.salm);
                        cmd.Parameters.AddWithValue("@art", origen.art);
                        cmd.Parameters.AddWithValue("@par", string.IsNullOrEmpty(origen.par) ? (object)DBNull.Value : origen.par);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task<alm_art> ObtenerOrigenAsync(string alm, string salm, string art)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT alm, salm, art, par FROM alm_art WHERE alm = @alm AND salm = @salm AND art = @art", conn);
                cmd.Parameters.AddWithValue("@alm", alm);
                cmd.Parameters.AddWithValue("@salm", salm);
                cmd.Parameters.AddWithValue("@art", art);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new alm_art
                        {
                            alm = reader["alm"].ToString(),
                            salm = reader["salm"].ToString(),
                            art = reader["art"].ToString(),
                            par = reader["par"]?.ToString()
                        };
                    }
                }
            }

            return null;
        }


        public async Task<List<alm_art>> ObtenerOrigenesPorArticuloAsync(string art)
        {
            var lista = new List<alm_art>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT alm, salm, art, par FROM alm_art WHERE art = @art", conn);
                cmd.Parameters.AddWithValue("@art", art);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new alm_art
                        {
                            alm = reader["alm"].ToString(),
                            salm = reader["salm"].ToString(),
                            art = reader["art"].ToString(),
                            par = reader["par"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }



        public async Task<string> ObtenerDescripcionArticuloAsync(string art)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT des1 FROM inviar WHERE art = @art AND status = '00'", conn);
            cmd.Parameters.AddWithValue("@art", art);

            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString() ?? "";
        }

        public async Task<List<alm_art>> ObtenerOrigenesAsync()
        {
            var lista = new List<alm_art>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand("SELECT alm, salm, art, par FROM alm_art", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new alm_art
                        {
                            alm = reader["alm"].ToString(),
                            salm = reader["salm"].ToString(),
                            art = reader["art"].ToString(),
                            par = reader["par"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public async Task ActualizarOrigenesAsync(List<alm_art> origenes)
        {
            if (origenes == null || origenes.Count == 0)
                return;

            var art = origenes[0].art;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Eliminar todos los registros previos del artículo
                using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM alm_art WHERE art = @art", conn))
                {
                    deleteCmd.Parameters.AddWithValue("@art", art);
                    await deleteCmd.ExecuteNonQueryAsync();
                }

                // Insertar nuevos registros
                foreach (var origen in origenes)
                {
                    using (SqlCommand insertCmd = new SqlCommand(@"INSERT INTO alm_art (alm, salm, art, par) 
                                                           VALUES (@alm, @salm, @art, @par)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@alm", origen.alm);
                        insertCmd.Parameters.AddWithValue("@salm", origen.salm);
                        insertCmd.Parameters.AddWithValue("@art", origen.art);
                        insertCmd.Parameters.AddWithValue("@par", string.IsNullOrEmpty(origen.par) ? (object)DBNull.Value : origen.par);

                        await insertCmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }




        public async Task<List<object>> BuscarArticulosAsync(string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda)) return new();

            string query = @"
                SELECT DISTINCT TOP 30 
                    a.art AS Art, 
                    i.des1 AS Descripcion, 
                    a.cve_pro AS NumProveedor, 
                    p.nom AS Proveedor
                FROM invart a
                INNER JOIN inviar i ON a.art = i.art
                INNER JOIN cprprv p ON a.cve_pro = p.proveedor
                WHERE (a.art LIKE @p0 OR i.des1 LIKE @p0)
                  AND a.status = '00'
                  AND i.status = '00'
                ORDER BY a.art";

            string parametro = $"%{busqueda.Trim()}%";

            var resultados = await _context.ComboArticuloDto
                .FromSqlRaw(query, parametro)
                .ToListAsync();

            return resultados.Select(x => new
            {
                id = $"{x.Art}|{x.Descripcion}|{x.NumProveedor}|{x.Proveedor}",
                text = $"{x.Art} - {x.Descripcion}"
            }).ToList<object>();
        }
    }
}
