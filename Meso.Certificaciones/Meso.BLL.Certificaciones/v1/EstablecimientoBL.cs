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
    public class EstablecimientoBL : IEstablecimientoBL
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public EstablecimientoBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<EstablecimientoDTO>> ObtenerTodos()
        {
            var listaEstablecimientos = await _certificacioncontext.Establecimiento.ToListAsync();
            return _mapper.Map<List<EstablecimientoDTO>>(listaEstablecimientos);
        }

        public async Task<EstablecimientoDTO> ObtenerPorId(int id)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                 .FirstOrDefaultAsync(x => x.EstablecimientoId == id);
            return _mapper.Map<EstablecimientoDTO>(establecimiento);
        }

        public async Task<EstablecimientoDTO> CrearEstablecimiento(EstablecimientoDTO modelo)
        {
            var nuevoEstablecimiento = _mapper.Map<Establecimiento>(modelo);

            _certificacioncontext.Establecimiento.Add(nuevoEstablecimiento);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EstablecimientoDTO>(nuevoEstablecimiento);
        }

        public async Task<EstablecimientoDTO> ActualizarEstablecimiento(EstablecimientoDTO modelo)
        {
            var establecimientoExistente = await _certificacioncontext.Establecimiento
                                          .FirstOrDefaultAsync(x => x.EstablecimientoId == modelo.EstablecimientoId);

            if (establecimientoExistente == null)
                throw new Exception("Establecimiento no encontrado");

            // Actualizar campos permitidos
            establecimientoExistente.NombreEstablecimiento = modelo.NombreEstablecimiento;
            establecimientoExistente.NIT = modelo.NIT;
            establecimientoExistente.Direccion = modelo.Direccion;
            establecimientoExistente.EstadoActivo = modelo.EstadoActivo;
            establecimientoExistente.CodigoUnicoEstablecimiento = modelo.CodigoUnicoEstablecimiento;
            establecimientoExistente.EmisorId = modelo.EmisorId;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EstablecimientoDTO>(establecimientoExistente);
        }

        public async Task<bool> EliminarEstablecimiento(int id)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                     .FirstOrDefaultAsync(x => x.EstablecimientoId == id);

            if (establecimiento == null)
                return false;

            _certificacioncontext.Establecimiento.Remove(establecimiento);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        // Métodos específicos para establecimientos
        public async Task<List<EstablecimientoDTO>> ObtenerActivos()
        {
            var establecimientos = await _certificacioncontext.Establecimiento
                                     .Where(x => x.EstadoActivo == true)
                                     .ToListAsync();
            return _mapper.Map<List<EstablecimientoDTO>>(establecimientos);
        }

        public async Task<List<EstablecimientoDTO>> ObtenerPorEmisor(int emisorId)
        {
            var establecimientos = await _certificacioncontext.Establecimiento
                                     .Where(x => x.EmisorId == emisorId)
                                     .ToListAsync();
            return _mapper.Map<List<EstablecimientoDTO>>(establecimientos);
        }

        public async Task<EstablecimientoDTO> ObtenerPorCodigoUnico(string codigoUnico)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                     .FirstOrDefaultAsync(x => x.CodigoUnicoEstablecimiento == codigoUnico);
            return _mapper.Map<EstablecimientoDTO>(establecimiento);
        }

        public async Task<EstablecimientoDTO> ObtenerPorNIT(string nit)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                     .FirstOrDefaultAsync(x => x.NIT == nit);
            return _mapper.Map<EstablecimientoDTO>(establecimiento);
        }

        public async Task<EstablecimientoDTO> ActivarEstablecimiento(int id)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                     .FirstOrDefaultAsync(x => x.EstablecimientoId == id);

            if (establecimiento == null)
                throw new Exception("Establecimiento no encontrado");

            establecimiento.EstadoActivo = true;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EstablecimientoDTO>(establecimiento);
        }

        public async Task<EstablecimientoDTO> DesactivarEstablecimiento(int id)
        {
            var establecimiento = await _certificacioncontext.Establecimiento
                                     .FirstOrDefaultAsync(x => x.EstablecimientoId == id);

            if (establecimiento == null)
                throw new Exception("Establecimiento no encontrado");

            establecimiento.EstadoActivo = false;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EstablecimientoDTO>(establecimiento);
        }

        public async Task<List<EstablecimientoDTO>> ObtenerActivosPorEmisor(int emisorId)
        {
            var establecimientos = await _certificacioncontext.Establecimiento
                                     .Where(x => x.EmisorId == emisorId && x.EstadoActivo == true)
                                     .ToListAsync();
            return _mapper.Map<List<EstablecimientoDTO>>(establecimientos);
        }

        public async Task<bool> ExisteCodigoUnico(string codigoUnico)
        {
            return await _certificacioncontext.Establecimiento
                         .AnyAsync(x => x.CodigoUnicoEstablecimiento == codigoUnico);
        }

        public async Task<bool> ExisteNIT(string nit)
        {
            return await _certificacioncontext.Establecimiento
                         .AnyAsync(x => x.NIT == nit);
        }

    }
}
