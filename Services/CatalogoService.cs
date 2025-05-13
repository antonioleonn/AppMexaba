using AppMexaba.Dtos;
using AppMexaba.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppMexaba.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly AppDbContext _context;

        public CatalogoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArticuloComprador>> ObtenerArticulosAsync(string busqueda)
        {
            var query = _context.cpr_art.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.Trim();
                query = query.Where(a =>
                    a.Art.Contains(busqueda) ||
                    a.Des1.Contains(busqueda) ||
                    a.Comprador.Contains(busqueda));
            }

            return await query.OrderBy(a => a.Art).ToListAsync();
        }

        public async Task<ArticuloComprador?> ObtenerArticuloPorIdAsync(string id)
        {
            return await _context.cpr_art.FindAsync(id.Trim());
        }

        public async Task<bool> CrearArticuloAsync(ArticuloComprador model)
        {
            var nombre = model.Comprador?.Trim();

            var datosComprador = await _context.cpr_map_comprador
                .AsNoTracking()
                .ToListAsync();

            var comparador = CultureInfo.InvariantCulture.CompareInfo;

            var compradorMatch = datosComprador
                .FirstOrDefault(m => comparador.Compare(m.NombreCompleto.Trim(), nombre, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);

            if (compradorMatch == null)
            {
                throw new InvalidOperationException("El comprador no está registrado en el mapa de compradores.");
            }

            model.Usuario = compradorMatch.Usuario;
            model.NumComprador = compradorMatch.NumComprador;
            model.Cve = compradorMatch.Cve;

            _context.cpr_art.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditarArticuloAsync(ArticuloComprador model)
        {
            var nombre = model.Comprador?.Trim();

            var datosComprador = await _context.cpr_map_comprador
                .AsNoTracking()
                .ToListAsync();

            var comparador = CultureInfo.InvariantCulture.CompareInfo;

            var compradorMatch = datosComprador
                .FirstOrDefault(m => comparador.Compare(m.NombreCompleto.Trim(), nombre, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);

            if (compradorMatch == null)
            {
                throw new InvalidOperationException("El comprador no está registrado en el mapa de compradores.");
            }

            model.Usuario = compradorMatch.Usuario;
            model.NumComprador = compradorMatch.NumComprador;
            model.Cve = compradorMatch.Cve;

            _context.cpr_art.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> BuscarArticulosAsync(string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda)) return new();

            string query = @"
             SELECT DISTINCT TOP 30 
                a.art AS Art, 
                 i.des1 AS Descripcion
             FROM invart a
             INNER JOIN inviar i ON a.art = i.art
             WHERE (a.art LIKE @p0 OR i.des1 LIKE @p0)
                AND a.status = '00'
                AND i.status = '00'
             ORDER BY a.art";


            var parametro = $"%{busqueda.Trim()}%";

            var resultados = await _context.ComboArticuloDto
                .FromSqlRaw(query, parametro)
                .ToListAsync();

            return resultados.Select(x => new
            {
                id = $"{x.Art}|{x.Descripcion}",
                text = $"{x.Art} - {x.Descripcion}"
            }).ToList<object>();

        }

        public async Task<List<object>> BuscarCompradoresAsync(string busqueda)
        {
            busqueda = busqueda?.Trim();

            var mapeo = await _context.cpr_map_comprador
                .AsNoTracking()
                .ToListAsync();

            var comparador = CultureInfo.InvariantCulture.CompareInfo;

            var resultados = mapeo
                .Where(m => string.IsNullOrEmpty(busqueda) ||
                            comparador.IndexOf(m.NombreCompleto.Trim(), busqueda, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) >= 0)
                .Select(m => new
                {
                    id = $"{m.NombreCompleto.Trim()}|{m.Cve}|{m.Usuario}|{m.NumComprador}",
                    text = m.NombreCompleto.Trim()
                })
                .ToList<object>();

            return resultados;
        }
    }
}
