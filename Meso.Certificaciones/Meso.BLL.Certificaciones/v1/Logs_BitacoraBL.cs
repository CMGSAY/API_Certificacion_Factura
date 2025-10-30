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
    public class Logs_BitacoraBL
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public Logs_BitacoraBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<Logs_BitacoraDTO>> ObtenerTodos()
        {
            var listaBitacoras = await _certificacioncontext.Logs_Bitacora
                                 .OrderByDescending(x => x.FechaAccion)
                                 .ToListAsync();
            return _mapper.Map<List<Logs_BitacoraDTO>>(listaBitacoras);
        }

        public async Task<Logs_BitacoraDTO> ObtenerPorId(int id)
        {
            var bitacora = await _certificacioncontext.Logs_Bitacora
                          .FirstOrDefaultAsync(x => x.BitacoraId == id);
            return _mapper.Map<Logs_BitacoraDTO>(bitacora);
        }

        public async Task<Logs_BitacoraDTO> CrearBitacora(Logs_BitacoraDTO modelo)
        {
            var nuevaBitacora = _mapper.Map<Logs_Bitacora>(modelo);

            // Asignar fecha de acción automáticamente
            if (nuevaBitacora.FechaAccion == DateTime.MinValue)
                nuevaBitacora.FechaAccion = DateTime.Now;

            _certificacioncontext.Logs_Bitacora.Add(nuevaBitacora);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<Logs_BitacoraDTO>(nuevaBitacora);
        }

        public async Task<Logs_BitacoraDTO> ActualizarBitacora(Logs_BitacoraDTO modelo)
        {
            var bitacoraExistente = await _certificacioncontext.Logs_Bitacora
                                    .FirstOrDefaultAsync(x => x.BitacoraId == modelo.BitacoraId);

            if (bitacoraExistente == null)
                throw new Exception("Registro de bitácora no encontrado");

            // Actualizar campos permitidos (no actualizar fecha de acción)
            bitacoraExistente.Accion = modelo.Accion;
            bitacoraExistente.Descripcion = modelo.Descripcion;
            bitacoraExistente.FechaProcesamiento = modelo.FechaProcesamiento;
            bitacoraExistente.FacturaId = modelo.FacturaId;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<Logs_BitacoraDTO>(bitacoraExistente);
        }

        public async Task<bool> EliminarBitacora(int id)
        {
            var bitacora = await _certificacioncontext.Logs_Bitacora
                              .FirstOrDefaultAsync(x => x.BitacoraId == id);

            if (bitacora == null)
                return false;

            _certificacioncontext.Logs_Bitacora.Remove(bitacora);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        // Métodos específicos para bitácora
        public async Task<List<Logs_BitacoraDTO>> ObtenerPorFactura(int facturaId)
        {
            var bitacoras = await _certificacioncontext.Logs_Bitacora
                              .Where(x => x.FacturaId == facturaId)
                              .OrderByDescending(x => x.FechaAccion)
                              .ToListAsync();
            return _mapper.Map<List<Logs_BitacoraDTO>>(bitacoras);
        }

        public async Task<List<Logs_BitacoraDTO>> ObtenerPorAccion(string accion)
        {
            var bitacoras = await _certificacioncontext.Logs_Bitacora
                              .Where(x => x.Accion == accion)
                              .OrderByDescending(x => x.FechaAccion)
                              .ToListAsync();
            return _mapper.Map<List<Logs_BitacoraDTO>>(bitacoras);
        }

        public async Task<List<Logs_BitacoraDTO>> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var bitacoras = await _certificacioncontext.Logs_Bitacora
                              .Where(x => x.FechaAccion >= fechaInicio && x.FechaAccion <= fechaFin)
                              .OrderByDescending(x => x.FechaAccion)
                              .ToListAsync();
            return _mapper.Map<List<Logs_BitacoraDTO>>(bitacoras);
        }

        public async Task<List<Logs_BitacoraDTO>> ObtenerNoProcesados()
        {
            var bitacoras = await _certificacioncontext.Logs_Bitacora
                              .Where(x => x.FechaProcesamiento == null)
                              .OrderBy(x => x.FechaAccion)
                              .ToListAsync();
            return _mapper.Map<List<Logs_BitacoraDTO>>(bitacoras);
        }

        public async Task<Logs_BitacoraDTO> MarcarComoProcesado(int bitacoraId)
        {
            var bitacora = await _certificacioncontext.Logs_Bitacora
                              .FirstOrDefaultAsync(x => x.BitacoraId == bitacoraId);

            if (bitacora == null)
                throw new Exception("Registro de bitácora no encontrado");

            bitacora.FechaProcesamiento = DateTime.Now;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<Logs_BitacoraDTO>(bitacora);
        }

        public async Task<int> CrearBitacoraSimple(string accion, string descripcion, int facturaId)
        {
            var nuevaBitacora = new Logs_Bitacora
            {
                Accion = accion,
                Descripcion = descripcion,
                FechaAccion = DateTime.Now,
                FacturaId = facturaId
            };

            _certificacioncontext.Logs_Bitacora.Add(nuevaBitacora);
            await _certificacioncontext.SaveChangesAsync();

            return nuevaBitacora.BitacoraId;
        }

        public async Task<bool> LimpiarBitacorasAntiguas(DateTime fechaLimite)
        {
            var bitacorasAntiguas = await _certificacioncontext.Logs_Bitacora
                                      .Where(x => x.FechaAccion < fechaLimite)
                                      .ToListAsync();

            if (!bitacorasAntiguas.Any())
                return false;

            _certificacioncontext.Logs_Bitacora.RemoveRange(bitacorasAntiguas);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }
    }
}
