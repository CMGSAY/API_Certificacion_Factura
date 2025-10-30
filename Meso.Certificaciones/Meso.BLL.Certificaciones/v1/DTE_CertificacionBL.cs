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
    public class DTE_CertificacionBL 
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public DTE_CertificacionBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<DTE_CertificacionDTO>> ObtenerTodos()
        {
            var listaCertificaciones = await _certificacioncontext.DTE_Certificacion.ToListAsync();
            return _mapper.Map<List<DTE_CertificacionDTO>>(listaCertificaciones);
        }

        public async Task<DTE_CertificacionDTO> ObtenerPorId(int id)
        {
            var certificacion = await _certificacioncontext.DTE_Certificacion
                            .FirstOrDefaultAsync(x => x.CertificacionId == id);
            return _mapper.Map<DTE_CertificacionDTO>(certificacion);
        }

        public async Task<DTE_CertificacionDTO> CrearCertificacion(DTE_CertificacionDTO modelo)
        {
            var nuevaCertificacion = _mapper.Map<DTE_Certificacion>(modelo);

            // Asignar fechas automáticamente si es necesario
            if (nuevaCertificacion.FechaIngreso == DateTime.MinValue)
                nuevaCertificacion.FechaIngreso = DateTime.Now;

            if (nuevaCertificacion.FechaCertificacion == DateTime.MinValue)
                nuevaCertificacion.FechaCertificacion = DateTime.Now;

            _certificacioncontext.DTE_Certificacion.Add(nuevaCertificacion);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_CertificacionDTO>(nuevaCertificacion);
        }

        public async Task<DTE_CertificacionDTO> ActualizarCertificacion(DTE_CertificacionDTO modelo)
        {
            var certificacionExistente = await _certificacioncontext.DTE_Certificacion
                                        .FirstOrDefaultAsync(x => x.CertificacionId == modelo.CertificacionId);

            if (certificacionExistente == null)
                throw new Exception("Certificación no encontrada");

            // Actualizar solo campos permitidos
            certificacionExistente.NumeroAutorizacion = modelo.NumeroAutorizacion;
            certificacionExistente.Serie = modelo.Serie;
            certificacionExistente.Correlativo = modelo.Correlativo;
            certificacionExistente.FechaProcesamiento = modelo.FechaProcesamiento;
            certificacionExistente.FacturaId = modelo.FacturaId;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<DTE_CertificacionDTO>(certificacionExistente);
        }

        public async Task<bool> EliminarCertificacion(int id)
        {
            var certificacion = await _certificacioncontext.DTE_Certificacion
                                .FirstOrDefaultAsync(x => x.CertificacionId == id);

            if (certificacion == null)
                return false;

            _certificacioncontext.DTE_Certificacion.Remove(certificacion);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        // Métodos específicos para certificaciones
        public async Task<DTE_CertificacionDTO> ObtenerPorFacturaId(int facturaId)
        {
            var certificacion = await _certificacioncontext.DTE_Certificacion
                                .FirstOrDefaultAsync(x => x.FacturaId == facturaId);
            return _mapper.Map<DTE_CertificacionDTO>(certificacion);
        }

        public async Task<DTE_CertificacionDTO> ObtenerPorNumeroAutorizacion(string numeroAutorizacion)
        {
            var certificacion = await _certificacioncontext.DTE_Certificacion
                                .FirstOrDefaultAsync(x => x.NumeroAutorizacion == numeroAutorizacion);
            return _mapper.Map<DTE_CertificacionDTO>(certificacion);
        }

        public async Task<List<DTE_CertificacionDTO>> ObtenerPorSerie(string serie)
        {
            var certificaciones = await _certificacioncontext.DTE_Certificacion
                                .Where(x => x.Serie == serie)
                                .ToListAsync();
            return _mapper.Map<List<DTE_CertificacionDTO>>(certificaciones);
        }
    }
}
