using AutoMapper;
using Meso.CBL.Certificaciones.v1;
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
    public class DTE_DetalleBL : IDTE_DetalleBL
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public DTE_DetalleBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<DTE_DetalleDTO>> ObtenerTodos()
        {
            var listaDetalles = await _certificacioncontext.DTE_Detalle.ToListAsync();
            return _mapper.Map<List<DTE_DetalleDTO>>(listaDetalles);
        }

        public async Task<DTE_DetalleDTO> ObtenerPorId(int id)
        {
            var detalle = await _certificacioncontext.DTE_Detalle
                         .FirstOrDefaultAsync(x => x.DetalleId == id);
            return _mapper.Map<DTE_DetalleDTO>(detalle);
        }

        public async Task<DTE_DetalleDTO> CrearDetalle(DTE_DetalleDTO modelo)
        {
            var nuevoDetalle = _mapper.Map<DTE_Detalle>(modelo);

            _certificacioncontext.DTE_Detalle.Add(nuevoDetalle);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_DetalleDTO>(nuevoDetalle);
        }

        public async Task<DTE_DetalleDTO> ActualizarDetalle(DTE_DetalleDTO modelo)
        {
            var detalleExistente = await _certificacioncontext.DTE_Detalle
                                  .FirstOrDefaultAsync(x => x.DetalleId == modelo.DetalleId);

            if (detalleExistente == null)
                throw new Exception("Detalle no encontrado");

            // Actualizar campos
            detalleExistente.Nombre = modelo.Nombre;
            detalleExistente.Precio = modelo.Precio;
            detalleExistente.Cantidad = modelo.Cantidad;
            detalleExistente.FacturaId = modelo.FacturaId;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_DetalleDTO>(detalleExistente);
        }

        public async Task<bool> EliminarPorFacturaId(int facturaId)
        {
            var detalles = await _certificacioncontext.DTE_Detalle
                             .Where(x => x.FacturaId == facturaId)
                             .ToListAsync();

            if (!detalles.Any())
                return false;

            _certificacioncontext.DTE_Detalle.RemoveRange(detalles);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }


        // Métodos específicos para detalles
        public async Task<List<DTE_DetalleDTO>> ObtenerPorFacturaId(int facturaId)
        {
            var detalles = await _certificacioncontext.DTE_Detalle
                             .Where(x => x.FacturaId == facturaId)
                             .ToListAsync();
            return _mapper.Map<List<DTE_DetalleDTO>>(detalles);
        }

        public async Task<bool> EliminarDetallesPorFactura(int facturaId)
        {
            var detalles = await _certificacioncontext.DTE_Detalle
                             .Where(x => x.FacturaId == facturaId)
                             .ToListAsync();

            if (!detalles.Any())
                return false;

            _certificacioncontext.DTE_Detalle.RemoveRange(detalles);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> ObtenerTotalPorFactura(int facturaId)
        {
            var total = await _certificacioncontext.DTE_Detalle
                          .Where(x => x.FacturaId == facturaId)
                          .SumAsync(x => x.Precio * x.Cantidad);

            return total;
        }
        public async Task<DTE_DetalleDTO> AgregarDetalle(int facturaId, DTE_DetalleDTO dto)
        {

            dto.FacturaId = facturaId;
            return await CrearDetalle(dto);
        }

    }
}
