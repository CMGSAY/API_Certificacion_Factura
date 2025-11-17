using Meso.DTO.Certificaciones;
using Meso.DTO.Certificaciones.Models;
using Meso.CBL.Certificaciones.v1;
using Meso.CSV.Certificaciones.v1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meso.BSV.Certificaciones.v1
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

        // =====================================================
        // CERTIFICACION COMPLETA
        // =====================================================
        public async Task<ResponseDTO<object>> CrearCertificacion(DTE_CertificacionDTO dto)
        {
            var response = new ResponseDTO<object>();

            try
            {
                // 1️⃣ Validar factura
                var factura = await _facturaBL.ObtenerPorId(dto.FacturaId);
                if (factura == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no existe.";
                    return response;
                }

                // 2️⃣ Obtener establecimiento
                var establecimiento = await _establecimientoBL.ObtenerPorId(factura.EstablecimientoId);
                if (establecimiento == null || !establecimiento.EstadoActivo)
                {
                    response.Success = false;
                    response.DisplayMessage = "El establecimiento no está activo.";
                    return response;
                }

                // 3️⃣ Obtener emisor
                var emisor = await _emisorBL.ObtenerPorId(establecimiento.EmisorId);
                if (emisor == null || !emisor.EstadoActivo || !emisor.PuedeCertificar)
                {
                    response.Success = false;
                    response.DisplayMessage = "El emisor no está activo o no tiene permisos de certificación.";
                    return response;
                }

                // 4️⃣ Obtener detalles de factura
                var detalles = await _detalleBL.ObtenerPorFacturaId(factura.FacturaId);
                if (detalles == null || detalles.Count == 0)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no contiene detalles válidos.";
                    return response;
                }

                // 5️⃣ Crear certificación
                dto.NumeroAutorizacion = Guid.NewGuid().ToString().ToUpper();
                dto.FechaIngreso = DateTime.Now;
                dto.FechaCertificacion = DateTime.Now;
                dto.Serie = "SAT2025";
                dto.Correlativo = new Random().Next(1000, 9999);

                var certificacionCreada = await _certificacionBL.CrearCertificacion(dto);

                // 6️⃣ Marcar factura como procesada
                await _facturaBL.MarcarComoProcesada(factura.FacturaId);

                // 7️⃣ Registrar en bitácora
                await _bitacoraBL.CrearBitacoraSimple(
                    "CERTIFICACION",
                    $"Certificación creada con número {dto.NumeroAutorizacion} para factura {factura.FacturaId}",
                    factura.FacturaId
                );

                // 8️⃣ Armar respuesta completa (usando varios DTO)
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

        // =====================================================
        // ANULACION COMPLETA
        // =====================================================
        public async Task<ResponseDTO<object>> CrearAnulacion(DTE_AnulacionDTO dto)
        {
            var response = new ResponseDTO<object>();

            try
            {
                // 1️⃣ Validar factura
                var factura = await _facturaBL.ObtenerPorId(dto.FacturaId);
                if (factura == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "La factura no existe.";
                    return response;
                }

                // 2️⃣ Obtener establecimiento y emisor
                var establecimiento = await _establecimientoBL.ObtenerPorId(factura.EstablecimientoId);
                var emisor = await _emisorBL.ObtenerPorId(establecimiento.EmisorId);

                if (emisor == null || !emisor.EstadoActivo)
                {
                    response.Success = false;
                    response.DisplayMessage = "El emisor no está activo. No puede anular.";
                    return response;
                }

                // 3️⃣ Crear anulación
                dto.FechaIngreso = DateTime.Now;
                dto.FechaAnulacion = DateTime.Now;
                dto.FechaProcesamiento = DateTime.Now;

                var anulacionCreada = await _anulacionBL.CrearAnulacion(dto);

                // 4️⃣ Cambiar estado de factura a "anulada" (0)
                await _facturaBL.ActualizarEstado(dto.FacturaId, 0);

                // 5️⃣ Registrar bitácora
                await _bitacoraBL.CrearBitacoraSimple(
                    "ANULACION",
                    $"Factura {dto.FacturaId} anulada. Motivo: {dto.Motivo}",
                    dto.FacturaId
                );

                // 6️⃣ Respuesta completa
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

        // =====================================================
        // CONSULTAS GENERALES
        // =====================================================

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

        // =====================================================
        // EMISOR (Activar / Desactivar)
        // =====================================================
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

        public async Task<ResponseDTO<List<EmisorDTO>>> ObtenerEmisores()
        {
            var response = new ResponseDTO<List<EmisorDTO>>();

            try
            {
                var emisores = await _emisorBL.ObtenerTodos();

                response.SingleResult = emisores;
                response.DisplayMessage = "Lista de emisores obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<ResponseDTO<EmisorDTO>> ObtenerEmisorPorId(int emisorId)
        {
            var response = new ResponseDTO<EmisorDTO>();

            try
            {
                var emisor = await _emisorBL.ObtenerPorId(emisorId);

                if (emisor == null)
                {
                    response.Success = false;
                    response.DisplayMessage = "El emisor no fue encontrado.";
                    return response;
                }

                response.SingleResult = emisor;
                response.DisplayMessage = "Emisor obtenido correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        // ESTABLECIMIENTOS
        public async Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientos()
        {
            var response = new ResponseDTO<List<EstablecimientoDTO>>();
            try
            {
                response.SingleResult = await _establecimientoBL.ObtenerTodos();
                response.DisplayMessage = "Lista de establecimientos obtenida correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO<EstablecimientoDTO>> ObtenerEstablecimientoPorId(int id)
        {
            var response = new ResponseDTO<EstablecimientoDTO>();
            try
            {
                var est = await _establecimientoBL.ObtenerPorId(id);
                if (est == null) { response.Success = false; response.DisplayMessage = "Establecimiento no encontrado."; return response; }
                response.SingleResult = est;
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientosPorEmisor(int emisorId)
        {
            var response = new ResponseDTO<List<EstablecimientoDTO>>();
            try
            {
                response.SingleResult = await _establecimientoBL.ObtenerPorEmisor(emisorId);
                response.DisplayMessage = "Establecimientos obtenidos correctamente.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        // FACTURAS (DTE)
        public async Task<ResponseDTO<object>> CrearFactura(DTE_FacturaDTO dto)
        {
            var response = new ResponseDTO<object>();
            try
            {
                var created = await _facturaBL.CrearFactura(dto);
                response.SingleResult = created;
                response.DisplayMessage = "Factura creada correctamente.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturas()
        {
            var response = new ResponseDTO<List<DTE_FacturaDTO>>();
            try
            {
                response.SingleResult = await _facturaBL.ObtenerTodos();
                response.DisplayMessage = "Facturas obtenidas.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<DTE_FacturaDTO>> ObtenerFacturaPorId(int id)
        {
            var response = new ResponseDTO<DTE_FacturaDTO>();
            try
            {
                var f = await _facturaBL.ObtenerPorId(id);
                if (f == null) { response.Success = false; response.DisplayMessage = "Factura no encontrada."; return response; }
                response.SingleResult = f;
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<object>> ProcesarFactura(int id)
        {
            var response = new ResponseDTO<object>();
            try
            {
                var factura = await _facturaBL.ObtenerPorId(id);
                if (factura == null) { response.Success = false; response.DisplayMessage = "Factura no encontrada."; return response; }
                // validar mínimos (por ejemplo: tiene detalles y total > 0)
                var detalles = await _detalleBL.ObtenerPorFacturaId(id);
                if (detalles == null || detalles.Count == 0) { response.Success = false; response.DisplayMessage = "Factura sin detalles."; return response; }
                // marcar procesada
                await _facturaBL.MarcarComoProcesada(id);
                response.DisplayMessage = "Factura procesada y lista para certificación.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        // DETALLES
        public async Task<ResponseDTO<List<DTE_DetalleDTO>>> ObtenerDetallesPorFactura(int facturaId)
        {
            var response = new ResponseDTO<List<DTE_DetalleDTO>>();
            try
            {
                response.SingleResult = await _detalleBL.ObtenerPorFacturaId(facturaId);
                response.DisplayMessage = "Detalles obtenidos.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<object>> AgregarDetalleFactura(int facturaId, DTE_DetalleDTO dto)
        {
            var response = new ResponseDTO<object>();
            try
            {
                var detalle = await _detalleBL.AgregarDetalle(facturaId, dto);
                response.SingleResult = detalle;
                response.DisplayMessage = "Detalle agregado.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        // BITACORA
        public async Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacora()
        {
            var response = new ResponseDTO<List<Logs_BitacoraDTO>>();
            try
            {
                response.SingleResult = await _bitacoraBL.ObtenerTodos();
                response.DisplayMessage = "Bitácora obtenida.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        public async Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacoraPorFactura(int facturaId)
        {
            var response = new ResponseDTO<List<Logs_BitacoraDTO>>();
            try
            {
                response.SingleResult = await _bitacoraBL.ObtenerPorFactura(facturaId);
                response.DisplayMessage = "Bitácora por factura obtenida.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        // VALIDACIÓN SAT (SIMULADA)
        public async Task<ResponseDTO<ValidacionResultadoDTO>> ValidarDTE(int facturaId)
        {
            var response = new ResponseDTO<ValidacionResultadoDTO>();
            try
            {
                var factura = await _facturaBL.ObtenerPorId(facturaId);
                if (factura == null) { response.Success = false; response.DisplayMessage = "Factura no encontrada."; return response; }

                var detalles = await _detalleBL.ObtenerPorFacturaId(facturaId);
                var resultado = new ValidacionResultadoDTO { Valido = true };

                // reglas simples de validación simulada
                if (detalles == null || detalles.Count == 0) { resultado.Valido = false; resultado.Mensajes.Add("La factura no tiene detalles."); }
                if (factura.Total <= 0) { resultado.Valido = false; resultado.Mensajes.Add("Total inválido."); }

                response.SingleResult = resultado;
                response.DisplayMessage = resultado.Valido ? "Factura válida." : "Factura inválida.";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }

        // FIRMA SIMULADA
        public async Task<ResponseDTO<FirmaResultadoDTO>> FirmarDTE(int facturaId)
        {
            var response = new ResponseDTO<FirmaResultadoDTO>();
            try
            {
                var factura = await _facturaBL.ObtenerPorId(facturaId);
                if (factura == null) { response.Success = false; response.DisplayMessage = "Factura no encontrada."; return response; }

                // Simulación: generar hash aleatorio y marcar como firmada
                var hash = Guid.NewGuid().ToString("N");
                var resultado = new FirmaResultadoDTO { Firmado = true, Hash = hash, Mensaje = "DTE firmado correctamente (simulado)." };

                // opcional: registrar en bitácora
                await _bitacoraBL.CrearBitacoraSimple("FIRMA", $"DTE {facturaId} firmado. Hash: {hash}", facturaId);

                response.SingleResult = resultado;
                response.DisplayMessage = "DTE firmado (simulado).";
            }
            catch (Exception ex) { response.Success = false; response.ErrorMessage = ex.Message; }
            return response;
        }


    }
}


