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
    public class DTE_AnulacionBL : IDTE_AnulacionBL
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public DTE_AnulacionBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<DTE_AnulacionDTO>> ObtenerTodos()
        {
            var listaAnulaciones = await _certificacioncontext.DTE_Anulacion.ToListAsync();
            return _mapper.Map<List<DTE_AnulacionDTO>>(listaAnulaciones);
        }

        public async Task<DTE_AnulacionDTO> ObtenerPorId(int id)
        {
            var anulacion = await _certificacioncontext.DTE_Anulacion
                            .FirstOrDefaultAsync(x => x.AnulacionId == id);
            return _mapper.Map<DTE_AnulacionDTO>(anulacion);
        }

        public async Task<DTE_AnulacionDTO> CrearAnulacion(DTE_AnulacionDTO modelo)
        {
            var nuevaAnulacion = _mapper.Map<DTE_Anulacion>(modelo);

            // Asignar fechas automáticamente 
            if (nuevaAnulacion.FechaIngreso == DateTime.MinValue)
                nuevaAnulacion.FechaIngreso = DateTime.Now;

            if (nuevaAnulacion.FechaAnulacion == DateTime.MinValue)
                nuevaAnulacion.FechaAnulacion = DateTime.Now;

            _certificacioncontext.DTE_Anulacion.Add(nuevaAnulacion);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_AnulacionDTO>(nuevaAnulacion);
        }

        public async Task<DTE_AnulacionDTO> ActualizarAnulacion(DTE_AnulacionDTO modelo)
        {
            var anulacionExistente = await _certificacioncontext.DTE_Anulacion
                                    .FirstOrDefaultAsync(x => x.AnulacionId == modelo.AnulacionId);

            if (anulacionExistente == null)
                throw new Exception("Anulación no encontrada");

            // No actualizar fechas de ingreso/anulación
            anulacionExistente.Motivo = modelo.Motivo;
            anulacionExistente.FechaProcesamiento = modelo.FechaProcesamiento;
            anulacionExistente.FacturaId = modelo.FacturaId;

            _certificacioncontext.DTE_Anulacion.Update(anulacionExistente);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_AnulacionDTO>(anulacionExistente);
        }

        public async Task<bool> EliminarAnulacion(int id)
        {
            var anulacion = await _certificacioncontext.DTE_Anulacion
                                .FirstOrDefaultAsync(x => x.AnulacionId == id);

            if (anulacion == null)
                return false;

            _certificacioncontext.DTE_Anulacion.Remove(anulacion);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        // Método específico para anulaciones
        public async Task<List<DTE_AnulacionDTO>> ObtenerPorFacturaId(int facturaId)
        {
            var anulaciones = await _certificacioncontext.DTE_Anulacion
                                .Where(x => x.FacturaId == facturaId)
                                .ToListAsync();
            return _mapper.Map<List<DTE_AnulacionDTO>>(anulaciones);
        }
    }
}
