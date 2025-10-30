using AutoMapper;
using Meso.DAL.Certificaciones;
using Meso.DAL.Certificaciones.Entities;
using Meso.DTO.Certificaciones.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Meso.BLL.Certificaciones.v1
{
    public class DTE_FacturaBL
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public DTE_FacturaBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<DTE_FacturaDTO>> ObtenerTodos()
        {
            var listaFacturas = await _certificacioncontext.DTE_Factura.ToListAsync();
            return _mapper.Map<List<DTE_FacturaDTO>>(listaFacturas);
        }

        public async Task<DTE_FacturaDTO> ObtenerPorId(int id)
        {
            var factura = await _certificacioncontext.DTE_Factura
                         .FirstOrDefaultAsync(x => x.FacturaId == id);
            return _mapper.Map<DTE_FacturaDTO>(factura);
        }

        public async Task<DTE_FacturaDTO> CrearFactura(DTE_FacturaDTO modelo)
        {
            var nuevaFactura = _mapper.Map<DTE_Factura>(modelo);

            // Asignar fechas automáticamente 
            if (nuevaFactura.FechaIngreso == DateTime.MinValue)
                nuevaFactura.FechaIngreso = DateTime.Now;

            if (nuevaFactura.FechaEmision == DateTime.MinValue)
                nuevaFactura.FechaEmision = DateTime.Now;

            // Estado por defecto si no viene
            if (nuevaFactura.Estado == 0)
                nuevaFactura.Estado = 1;

            _certificacioncontext.DTE_Factura.Add(nuevaFactura);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_FacturaDTO>(nuevaFactura);
        }

        public async Task<DTE_FacturaDTO> ActualizarFactura(DTE_FacturaDTO modelo)
        {
            var facturaExistente = await _certificacioncontext.DTE_Factura
                                  .FirstOrDefaultAsync(x => x.FacturaId == modelo.FacturaId);

            if (facturaExistente == null)
                throw new Exception("Factura no encontrada");

            // Actualizar campos permitidos
            facturaExistente.FechaEmision = modelo.FechaEmision;
            facturaExistente.ClienteNIT = modelo.ClienteNIT;
            facturaExistente.ClienteRazonSocial = modelo.ClienteRazonSocial;
            facturaExistente.Total = modelo.Total;
            facturaExistente.Estado = modelo.Estado;
            facturaExistente.FechaProcesamiento = modelo.FechaProcesamiento;
            facturaExistente.EstablecimientoId = modelo.EstablecimientoId;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_FacturaDTO>(facturaExistente);
        }

        public async Task<bool> EliminarFactura(int id)
        {
            var factura = await _certificacioncontext.DTE_Factura
                             .FirstOrDefaultAsync(x => x.FacturaId == id);

            if (factura == null)
                return false;

            _certificacioncontext.DTE_Factura.Remove(factura);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        public async Task<List<DTE_FacturaDTO>> ObtenerPorEstado(byte estado)
        {
            var facturas = await _certificacioncontext.DTE_Factura
                             .Where(x => x.Estado == estado)
                             .ToListAsync();
            return _mapper.Map<List<DTE_FacturaDTO>>(facturas);
        }

        public async Task<List<DTE_FacturaDTO>> ObtenerPorClienteNIT(string nit)
        {
            var facturas = await _certificacioncontext.DTE_Factura
                             .Where(x => x.ClienteNIT == nit)
                             .ToListAsync();
            return _mapper.Map<List<DTE_FacturaDTO>>(facturas);
        }

        public async Task<List<DTE_FacturaDTO>> ObtenerPorEstablecimiento(int establecimientoId)
        {
            var facturas = await _certificacioncontext.DTE_Factura
                             .Where(x => x.EstablecimientoId == establecimientoId)
                             .ToListAsync();
            return _mapper.Map<List<DTE_FacturaDTO>>(facturas);
        }

        public async Task<List<DTE_FacturaDTO>> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var facturas = await _certificacioncontext.DTE_Factura
                             .Where(x => x.FechaEmision >= fechaInicio && x.FechaEmision <= fechaFin)
                             .ToListAsync();
            return _mapper.Map<List<DTE_FacturaDTO>>(facturas);
        }

        public async Task<DTE_FacturaDTO> ActualizarEstado(int facturaId, byte nuevoEstado)
        {
            var factura = await _certificacioncontext.DTE_Factura
                             .FirstOrDefaultAsync(x => x.FacturaId == facturaId);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            factura.Estado = nuevoEstado;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_FacturaDTO>(factura);
        }

        public async Task<DTE_FacturaDTO> MarcarComoProcesada(int facturaId)
        {
            var factura = await _certificacioncontext.DTE_Factura
                             .FirstOrDefaultAsync(x => x.FacturaId == facturaId);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            factura.Estado = 2;
            factura.FechaProcesamiento = DateTime.Now;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_FacturaDTO>(factura);
        }
    }
}
