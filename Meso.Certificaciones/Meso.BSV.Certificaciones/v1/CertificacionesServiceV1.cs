using Meso.DTO.Certificaciones;
using Meso.DTO.Certificaciones.Models;
using Meso.CBL.Certificaciones.v1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meso.CSV.Certificaciones.v1
{
    public class CertificacionesServiceV1 : ICertificacionesServiceV1
    {
        private readonly IDTE_CertificacionBL _certificacionBL;
        private readonly IDTE_AnulacionBL _anulacionBL;
        private readonly IDTE_FacturaBL _facturaBL;
        private readonly IDTE_DetalleBL _detalleBL;
        private readonly IEmisorBL _emisorBL;
        private readonly IEstablecimientoBL _establecimientoBL;
        private readonly ILogs_BitacoraBL _bitacoraBL;

        public CertificacionesServiceV1(
            IDTE_CertificacionBL certificacionBL,
            IDTE_AnulacionBL anulacionBL,
            IDTE_FacturaBL facturaBL,
            IDTE_DetalleBL detalleBL,
            IEmisorBL emisorBL,
            IEstablecimientoBL establecimientoBL,
            ILogs_BitacoraBL bitacoraBL)
        {
            _certificacionBL = certificacionBL;
            _anulacionBL = anulacionBL;
            _facturaBL = facturaBL;
            _detalleBL = detalleBL;
            _emisorBL = emisorBL;
            _establecimientoBL = establecimientoBL;
            _bitacoraBL = bitacoraBL;
        }

        // CERTIFICACION COMPLETA==
        public async Task<ResponseDTO<object>> CrearCertificacion(DTE_CertificacionDTO dto)
        {
            var response = new ResponseDTO<object>();

            try
            {
                //  Validar factura
                var factura = await _facturaBL.ObtenerPorId(dto.FacturaId);
                if (factura == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no existe.";
                    return response;
                }

                //  Obtener establecimiento
                var establecimiento = await _establecimientoBL.ObtenerPorId(factura.EstablecimientoId);
                if (establecimiento == null || !establecimiento.EstadoActivo)
                {
                    response.Success = false;
                    response.DisplayMessage = "El establecimiento no está activo.";
                    return response;
                }

                //  Obtener emisor
                var emisor = await _emisorBL.ObtenerPorId(establecimiento.EmisorId);
                if (emisor == null || !emisor.EstadoActivo || !emisor.PuedeCertificar)
                {
                    response.Success = false;
                    response.DisplayMessage = "El emisor no está activo o no tiene permisos de certificación.";
                    return response;
                }

                //  Obtener detalles de factura
                var detalles = await _detalleBL.ObtenerPorFacturaId(factura.FacturaId);
                if (detalles == null || detalles.Count == 0)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no contiene detalles válidos.";
                    return response;
                }

                //  Crear certificación
                dto.NumeroAutorizacion = Guid.NewGuid().ToString().ToUpper();
                dto.FechaIngreso = DateTime.Now;
                dto.FechaCertificacion = DateTime.Now;
                dto.Serie = "SAT2025";
                dto.Correlativo = new Random().Next(1000, 9999);

                var certificacionCreada = await _certificacionBL.CrearCertificacion(dto);

                // Marcar factura como procesada
                await _facturaBL.MarcarComoProcesada(factura.FacturaId);

                //  Registrar en bitácora
                await _bitacoraBL.CrearBitacoraSimple(
                    "CERTIFICACION",
                    $"Certificación creada con número {dto.NumeroAutorizacion} para factura {factura.FacturaId}",
                    factura.FacturaId
                );

                // Armar respuesta completa (usando varios DTO)
                response.SingleResult = new
                {
                    Certificacion = certificacionCreada,
                    Factura = factura,
                    Detalles = detalles,
                    Establecimiento = establecimiento,
                    Emisor = emisor
                };

                response.DisplayMessage = "Certificación creada exitosamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

 
        // ANULACION COMPLETA
        public async Task<ResponseDTO<object>> CrearAnulacion(DTE_AnulacionDTO dto)
        {
            var response = new ResponseDTO<object>();

            try
            {
                //  Validar factura
                var factura = await _facturaBL.ObtenerPorId(dto.FacturaId);
                if (factura == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no existe.";
                    return response;
                }

                // Obtener establecimiento y emisor
                var establecimiento = await _establecimientoBL.ObtenerPorId(factura.EstablecimientoId);
                var emisor = await _emisorBL.ObtenerPorId(establecimiento.EmisorId);

                if (emisor == null || !emisor.EstadoActivo)
                {
                    response.Success = false;
                    response.DisplayMessage = "El emisor no está activo. No puede anular.";
                    return response;
                }

                //  Crear anulación
                dto.FechaIngreso = DateTime.Now;
                dto.FechaAnulacion = DateTime.Now;
                dto.FechaProcesamiento = DateTime.Now;

                var anulacionCreada = await _anulacionBL.CrearAnulacion(dto);

                // Cambiar estado de factura a "anulada" (0)
                await _facturaBL.ActualizarEstado(dto.FacturaId, 0);

                // Registrar bitácora
                await _bitacoraBL.CrearBitacoraSimple(
                    "ANULACION",
                    $"Factura {dto.FacturaId} anulada. Motivo: {dto.Motivo}",
                    dto.FacturaId
                );

                //  Respuesta completa
                response.SingleResult = new
                {
                    Anulacion = anulacionCreada,
                    Factura = factura,
                    Establecimiento = establecimiento,
                    Emisor = emisor
                };

                response.DisplayMessage = "Factura anulada correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

 
        // CONSULTAS GENERALES

        public async Task<ResponseDTO<List<DTE_CertificacionDTO>>> ObtenerCertificaciones()
        {
            var response = new ResponseDTO<List<DTE_CertificacionDTO>>();
            try
            {
                response.SingleResult = await _certificacionBL.ObtenerTodos();
                response.DisplayMessage = "Lista de certificaciones obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorId(int id)
        {
            var response = new ResponseDTO<DTE_CertificacionDTO>();
            try
            {
                var cert = await _certificacionBL.ObtenerPorId(id);
                if (cert == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "Certificación no encontrada.";
                    return response;
                }
                response.SingleResult = cert;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        // EMISOR (Activar / Desactivar)
        public async Task<ResponseDTO<EmisorDTO>> ActivarEmisor(int emisorId)
        {
            var response = new ResponseDTO<EmisorDTO>();
            try
            {
                response.SingleResult = await _emisorBL.ActivarEmisor(emisorId);
                response.DisplayMessage = "Emisor activado correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO<EmisorDTO>> DesactivarEmisor(int emisorId)
        {
            var response = new ResponseDTO<EmisorDTO>();
            try
            {
                response.SingleResult = await _emisorBL.DesactivarEmisor(emisorId);
                response.DisplayMessage = "Emisor desactivado correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
