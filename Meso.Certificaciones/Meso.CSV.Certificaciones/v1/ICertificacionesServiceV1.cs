using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Meso.CSV.Certificaciones.v1
{
    public interface ICertificacionesServiceV1
    {
        // DTE_Anulacion
        Task<ResponseDTO<List<DTE_AnulacionDTO>>> ObtenerAnulaciones();
        Task<ResponseDTO<DTE_AnulacionDTO>> ObtenerAnulacionPorId(int anulacionId);
        Task<ResponseDTO<DTE_AnulacionDTO>> CrearAnulacion(DTE_AnulacionDTO anulacion);
        Task<ResponseDTO<DTE_AnulacionDTO>> EditarAnulacion(DTE_AnulacionDTO anulacion);
        Task<ResponseDTO<DTE_AnulacionDTO>> EliminarAnulacion(int anulacionId);
        Task<ResponseDTO<List<DTE_AnulacionDTO>>> ObtenerAnulacionesPorFactura(int facturaId);

        // DTE_Certificacion
        Task<ResponseDTO<List<DTE_CertificacionDTO>>> ObtenerCertificaciones();
        Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorId(int certificacionId);
        Task<ResponseDTO<DTE_CertificacionDTO>> CrearCertificacion(DTE_CertificacionDTO certificacion);
        Task<ResponseDTO<DTE_CertificacionDTO>> EditarCertificacion(DTE_CertificacionDTO certificacion);
        Task<ResponseDTO<DTE_CertificacionDTO>> EliminarCertificacion(int certificacionId);
        Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorFactura(int facturaId);
        Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorNumeroAutorizacion(string numeroAutorizacion);
        Task<ResponseDTO<List<DTE_CertificacionDTO>>> ObtenerCertificacionesPorSerie(string serie);

        // DTE_Detalle
        Task<ResponseDTO<List<DTE_DetalleDTO>>> ObtenerDetalles();
        Task<ResponseDTO<DTE_DetalleDTO>> ObtenerDetallePorId(int detalleId);
        Task<ResponseDTO<DTE_DetalleDTO>> CrearDetalle(DTE_DetalleDTO detalle);
        Task<ResponseDTO<DTE_DetalleDTO>> EditarDetalle(DTE_DetalleDTO detalle);
        Task<ResponseDTO<DTE_DetalleDTO>> EliminarDetalle(int detalleId);
        Task<ResponseDTO<List<DTE_DetalleDTO>>> ObtenerDetallesPorFactura(int facturaId);
        //Task<ResponseDTO<decimal>> ObtenerTotalPorFactura(int facturaId);

        // DTE_Factura
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturas();
        Task<ResponseDTO<DTE_FacturaDTO>> ObtenerFacturaPorId(int facturaId);
        Task<ResponseDTO<DTE_FacturaDTO>> CrearFactura(DTE_FacturaDTO factura);
        Task<ResponseDTO<DTE_FacturaDTO>> EditarFactura(DTE_FacturaDTO factura);
        Task<ResponseDTO<DTE_FacturaDTO>> EliminarFactura(int facturaId);
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturasPorEstado(byte estado);
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturasPorClienteNIT(string nit);
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturasPorEstablecimiento(int establecimientoId);
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<ResponseDTO<DTE_FacturaDTO>> MarcarFacturaComoProcesada(int facturaId);

        // Emisor
        Task<ResponseDTO<List<EmisorDTO>>> ObtenerEmisores();
        Task<ResponseDTO<EmisorDTO>> ObtenerEmisorPorId(int emisorId);
        Task<ResponseDTO<EmisorDTO>> CrearEmisor(EmisorDTO emisor);
        Task<ResponseDTO<EmisorDTO>> EditarEmisor(EmisorDTO emisor);
        Task<ResponseDTO<EmisorDTO>> EliminarEmisor(int emisorId);
        Task<ResponseDTO<List<EmisorDTO>>> ObtenerEmisoresActivos();
        Task<ResponseDTO<List<EmisorDTO>>> ObtenerEmisoresConCertificacion();
        Task<ResponseDTO<EmisorDTO>> ObtenerEmisorPorNIT(string nit);
        Task<ResponseDTO<EmisorDTO>> ActivarEmisor(int emisorId);
        Task<ResponseDTO<EmisorDTO>> DesactivarEmisor(int emisorId);

        // Establecimiento
        Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientos();
        Task<ResponseDTO<EstablecimientoDTO>> ObtenerEstablecimientoPorId(int establecimientoId);
        Task<ResponseDTO<EstablecimientoDTO>> CrearEstablecimiento(EstablecimientoDTO establecimiento);
        Task<ResponseDTO<EstablecimientoDTO>> EditarEstablecimiento(EstablecimientoDTO establecimiento);
        Task<ResponseDTO<EstablecimientoDTO>> EliminarEstablecimiento(int establecimientoId);
        Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientosActivos();
        Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientosPorEmisor(int emisorId);
        Task<ResponseDTO<EstablecimientoDTO>> ObtenerEstablecimientoPorCodigoUnico(string codigoUnico);
        Task<ResponseDTO<EstablecimientoDTO>> ObtenerEstablecimientoPorNIT(string nit);
        Task<ResponseDTO<EstablecimientoDTO>> ActivarEstablecimiento(int establecimientoId);
        Task<ResponseDTO<EstablecimientoDTO>> DesactivarEstablecimiento(int establecimientoId);

        // Logs_Bitacora
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacoras();
        Task<ResponseDTO<Logs_BitacoraDTO>> ObtenerBitacoraPorId(int bitacoraId);
        Task<ResponseDTO<Logs_BitacoraDTO>> CrearBitacora(Logs_BitacoraDTO bitacora);
        Task<ResponseDTO<Logs_BitacoraDTO>> EditarBitacora(Logs_BitacoraDTO bitacora);
        Task<ResponseDTO<Logs_BitacoraDTO>> EliminarBitacora(int bitacoraId);
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacorasPorFactura(int facturaId);
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacorasPorAccion(string accion);
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacorasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacorasNoProcesadas();
        Task<ResponseDTO<Logs_BitacoraDTO>> MarcarBitacoraComoProcesada(int bitacoraId);
    }
}
