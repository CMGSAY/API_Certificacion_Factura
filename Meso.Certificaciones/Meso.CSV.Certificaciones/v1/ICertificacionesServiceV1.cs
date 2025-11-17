using Meso.DTO.Certificaciones;
using Meso.DTO.Certificaciones.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meso.CSV.Certificaciones.v1
{
    public interface ICertificacionesServiceV1
    {
        // CERTIFICACIÓN
        Task<ResponseDTO<object>> CrearCertificacion(DTE_CertificacionDTO dto);
        Task<ResponseDTO<List<DTE_CertificacionDTO>>> ObtenerCertificaciones();
        Task<ResponseDTO<DTE_CertificacionDTO>> ObtenerCertificacionPorId(int id);

        // ANULACIÓN
        Task<ResponseDTO<object>> CrearAnulacion(DTE_AnulacionDTO dto);

        // EMISOR
        Task<ResponseDTO<EmisorDTO>> ActivarEmisor(int emisorId);
        Task<ResponseDTO<EmisorDTO>> DesactivarEmisor(int emisorId);
        Task<ResponseDTO<List<EmisorDTO>>> ObtenerEmisores();
        Task<ResponseDTO<EmisorDTO>> ObtenerEmisorPorId(int emisorId);


        // ESTABLECIMIENTOS
        Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientos();
        Task<ResponseDTO<EstablecimientoDTO>> ObtenerEstablecimientoPorId(int id);
        Task<ResponseDTO<List<EstablecimientoDTO>>> ObtenerEstablecimientosPorEmisor(int emisorId);

        // FACTURAS (DTE en borrador)
        Task<ResponseDTO<object>> CrearFactura(DTE_FacturaDTO dto);
        Task<ResponseDTO<List<DTE_FacturaDTO>>> ObtenerFacturas();
        Task<ResponseDTO<DTE_FacturaDTO>> ObtenerFacturaPorId(int id);
        Task<ResponseDTO<object>> ProcesarFactura(int id);

        // DETALLES DTE
        Task<ResponseDTO<List<DTE_DetalleDTO>>> ObtenerDetallesPorFactura(int facturaId);
        Task<ResponseDTO<object>> AgregarDetalleFactura(int facturaId, DTE_DetalleDTO dto);

        // BITACORA
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacora();
        Task<ResponseDTO<List<Logs_BitacoraDTO>>> ObtenerBitacoraPorFactura(int facturaId);

        // VALIDACION SAT (simulada)
        Task<ResponseDTO<ValidacionResultadoDTO>> ValidarDTE(int facturaId);

        // FIRMA (simulada)
        Task<ResponseDTO<FirmaResultadoDTO>> FirmarDTE(int facturaId);
    }
}
