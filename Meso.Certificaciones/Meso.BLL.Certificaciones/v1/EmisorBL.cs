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
    public class EmisorBL 
    {
        private readonly CertificadorDBContext _certificacioncontext;
        private readonly IMapper _mapper;

        public EmisorBL(CertificadorDBContext certificacionContext, IMapper mapper)
        {
            _certificacioncontext = certificacionContext;
            _mapper = mapper;
        }

        public async Task<List<EmisorDTO>> ObtenerTodos()
        {
            var listaEmisores = await _certificacioncontext.Emisor.ToListAsync();
            return _mapper.Map<List<EmisorDTO>>(listaEmisores);
        }

        public async Task<EmisorDTO> ObtenerPorId(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                         .FirstOrDefaultAsync(x => x.EmisorId == id);
            return _mapper.Map<EmisorDTO>(emisor);
        }

        public async Task<EmisorDTO> CrearEmisor(EmisorDTO modelo)
        {
            var nuevoEmisor = _mapper.Map<Emisor>(modelo);

            if (nuevoEmisor.FechaRegistro == DateTime.MinValue)
                nuevoEmisor.FechaRegistro = DateTime.Now;

            _certificacioncontext.Emisor.Add(nuevoEmisor);
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(nuevoEmisor);
        }

        public async Task<EmisorDTO> ActualizarEmisor(EmisorDTO modelo)
        {
            var emisorExistente = await _certificacioncontext.Emisor
                                  .FirstOrDefaultAsync(x => x.EmisorId == modelo.EmisorId);

            if (emisorExistente == null)
                throw new Exception("Emisor no encontrado");

            // Actualizar campos permitidos
            emisorExistente.Nit = modelo.Nit;
            emisorExistente.RazonSocial = modelo.RazonSocial;
            emisorExistente.EstadoActivo = modelo.EstadoActivo;
            emisorExistente.PuedeCertificar = modelo.PuedeCertificar;

            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(emisorExistente);
        }

        public async Task<bool> EliminarEmisor(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.EmisorId == id);

            if (emisor == null)
                return false;

            _certificacioncontext.Emisor.Remove(emisor);
            await _certificacioncontext.SaveChangesAsync();

            return true;
        }

        public async Task<List<EmisorDTO>> ObtenerActivos()
        {
            var emisores = await _certificacioncontext.Emisor
                             .Where(x => x.EstadoActivo == true)
                             .ToListAsync();
            return _mapper.Map<List<EmisorDTO>>(emisores);
        }

        public async Task<List<EmisorDTO>> ObtenerConCertificacion()
        {
            var emisores = await _certificacioncontext.Emisor
                             .Where(x => x.PuedeCertificar == true && x.EstadoActivo == true)
                             .ToListAsync();
            return _mapper.Map<List<EmisorDTO>>(emisores);
        }

        public async Task<EmisorDTO> ObtenerPorNIT(string nit)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.Nit == nit);
            return _mapper.Map<EmisorDTO>(emisor);
        }

        public async Task<EmisorDTO> ActivarEmisor(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.EmisorId == id);

            if (emisor == null)
                throw new Exception("Emisor no encontrado");

            emisor.EstadoActivo = true;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(emisor);
        }

        public async Task<EmisorDTO> DesactivarEmisor(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.EmisorId == id);

            if (emisor == null)
                throw new Exception("Emisor no encontrado");

            emisor.EstadoActivo = false;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(emisor);
        }

        public async Task<EmisorDTO> HabilitarCertificacion(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.EmisorId == id);

            if (emisor == null)
                throw new Exception("Emisor no encontrado");

            emisor.PuedeCertificar = true;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(emisor);
        }

        public async Task<EmisorDTO> DeshabilitarCertificacion(int id)
        {
            var emisor = await _certificacioncontext.Emisor
                             .FirstOrDefaultAsync(x => x.EmisorId == id);

            if (emisor == null)
                throw new Exception("Emisor no encontrado");

            emisor.PuedeCertificar = false;
            await _certificacioncontext.SaveChangesAsync();

            return _mapper.Map<EmisorDTO>(emisor);
        }
    }
}
