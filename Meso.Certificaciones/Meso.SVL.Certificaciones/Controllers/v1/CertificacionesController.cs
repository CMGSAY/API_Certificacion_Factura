using Microsoft.AspNetCore.Mvc;
using Meso.CSV.Certificaciones.v1;
using Meso.DTO.Certificaciones;
using Meso.DTO.Certificaciones.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Asp.Versioning;

namespace Meso.SVL.Certificaciones.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/certificaciones")]
    public class CertificacionesController : ControllerBase
    {
        private readonly ICertificacionesServiceV1 _certificadoServiceV1;
        public CertificacionesController(ICertificacionesServiceV1 certificadoServiceV1)
        {
            _certificadoServiceV1 = certificadoServiceV1;
        }




        // CERTIFICACIÓN


        /// <summary>
        /// Crea una nueva certificación de un Documento Tributario Electrónico (DTE).
        /// </summary>
        /// <param name="dto">Información necesaria para generar la certificación.</param>
        /// <returns>Resultado de la certificación.</returns>
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<object>>> CrearCertificacion([FromBody] DTE_CertificacionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _certificadoServiceV1.CrearCertificacion(dto);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene el listado completo de certificaciones registradas.
        /// </summary>
        /// <returns>Lista de certificaciones.</returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<DTE_CertificacionDTO>>>> ObtenerCertificaciones()
        {
            var response = await _certificadoServiceV1.ObtenerCertificaciones();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene una certificación específica según su identificador.
        /// </summary>
        /// <param name="id">ID de la certificación.</param>
        /// <returns>Certificación encontrada o error.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDTO<DTE_CertificacionDTO>>> ObtenerCertificacionPorId(int id)
        {
            var response = await _certificadoServiceV1.ObtenerCertificacionPorId(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }




        // ANULACIÓN


        /// <summary>
        /// Registra la anulación de un DTE previamente certificado.
        /// </summary>
        /// <param name="dto">Datos necesarios para procesar la anulación.</param>
        /// <returns>Resultado del proceso de anulación.</returns>
        [HttpPost("anulacion")]
        public async Task<ActionResult<ResponseDTO<object>>> CrearAnulacion([FromBody] DTE_AnulacionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _certificadoServiceV1.CrearAnulacion(dto);

            return response.Success ? Ok(response) : BadRequest(response);
        }




        // EMISOR


        /// <summary>
        /// Activa un emisor en el sistema.
        /// </summary>
        /// <param name="emisorId">Identificador del emisor a activar.</param>
        /// <returns>Emisor actualizado.</returns>
        [HttpPut("emisores/{emisorId:int}/activar")]
        public async Task<ActionResult<ResponseDTO<EmisorDTO>>> ActivarEmisor(int emisorId)
        {
            var response = await _certificadoServiceV1.ActivarEmisor(emisorId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Desactiva un emisor en el sistema.
        /// </summary>
        /// <param name="emisorId">Identificador del emisor a desactivar.</param>
        /// <returns>Emisor actualizado.</returns>
        [HttpPut("emisores/{emisorId:int}/desactivar")]
        public async Task<ActionResult<ResponseDTO<EmisorDTO>>> DesactivarEmisor(int emisorId)
        {
            var response = await _certificadoServiceV1.DesactivarEmisor(emisorId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene una lista completa de emisores registrados.
        /// </summary>
        /// <returns>Listado de emisores.</returns>
        [HttpGet("emisores")]
        public async Task<ActionResult<ResponseDTO<List<EmisorDTO>>>> ObtenerEmisores()
        {
            var response = await _certificadoServiceV1.ObtenerEmisores();
            return StatusCode(200, response);
        }

        /// <summary>
        /// Obtiene un emisor por su identificador.
        /// </summary>
        /// <param name="emisorId">ID del emisor.</param>
        /// <returns>Información del emisor solicitado.</returns>
        [HttpGet("emisores/{emisorId:int}")]
        public async Task<ActionResult<ResponseDTO<EmisorDTO>>> ObtenerEmisorPorId(int emisorId)
        {
            var response = await _certificadoServiceV1.ObtenerEmisorPorId(emisorId);
            return StatusCode(200, response);
        }




        // ESTABLECIMIENTOS


        /// <summary>
        /// Obtiene todos los establecimientos registrados.
        /// </summary>
        [HttpGet("establecimientos")]
        public async Task<ActionResult<ResponseDTO<List<EstablecimientoDTO>>>> ObtenerEstablecimientos()
        {
            var response = await _certificadoServiceV1.ObtenerEstablecimientos();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene un establecimiento por su identificador.
        /// </summary>
        /// <param name="id">ID del establecimiento.</param>
        [HttpGet("establecimientos/{id:int}")]
        public async Task<ActionResult<ResponseDTO<EstablecimientoDTO>>> ObtenerEstablecimientoPorId(int id)
        {
            var response = await _certificadoServiceV1.ObtenerEstablecimientoPorId(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene todos los establecimientos asociados a un emisor específico.
        /// </summary>
        /// <param name="emisorId">ID del emisor.</param>
        [HttpGet("establecimientos/emisor/{emisorId:int}")]
        public async Task<ActionResult<ResponseDTO<List<EstablecimientoDTO>>>> ObtenerEstablecimientosPorEmisor(int emisorId)
        {
            var response = await _certificadoServiceV1.ObtenerEstablecimientosPorEmisor(emisorId);
            return response.Success ? Ok(response) : BadRequest(response);
        }




        // FACTURAS (DTE FACTURA)


        /// <summary>
        /// Crea una nueva factura (DTE Factura).
        /// </summary>
        /// <param name="dto">Datos de la factura a registrar.</param>
        [HttpPost("facturas")]
        public async Task<ActionResult<ResponseDTO<object>>> CrearFactura([FromBody] DTE_FacturaDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _certificadoServiceV1.CrearFactura(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene todas las facturas registradas.
        /// </summary>
        [HttpGet("facturas")]
        public async Task<ActionResult<ResponseDTO<List<DTE_FacturaDTO>>>> ObtenerFacturas()
        {
            var response = await _certificadoServiceV1.ObtenerFacturas();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene una factura específica mediante su ID.
        /// </summary>
        /// <param name="id">ID de la factura.</param>
        [HttpGet("facturas/{id:int}")]
        public async Task<ActionResult<ResponseDTO<DTE_FacturaDTO>>> ObtenerFacturaPorId(int id)
        {
            var response = await _certificadoServiceV1.ObtenerFacturaPorId(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Procesa una factura para validación o certificación según corresponda.
        /// </summary>
        /// <param name="id">ID de la factura.</param>
        [HttpPut("facturas/{id:int}/procesar")]
        public async Task<ActionResult<ResponseDTO<object>>> ProcesarFactura(int id)
        {
            var response = await _certificadoServiceV1.ProcesarFactura(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene los detalles asociados a una factura específica.
        /// </summary>
        /// <param name="facturaId">ID de la factura.</param>
        [HttpGet("facturas/{facturaId:int}/detalles")]
        public async Task<ActionResult<ResponseDTO<List<DTE_DetalleDTO>>>> ObtenerDetallesPorFactura(int facturaId)
        {
            var response = await _certificadoServiceV1.ObtenerDetallesPorFactura(facturaId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Agrega un nuevo detalle a una factura existente.
        /// </summary>
        /// <param name="facturaId">ID de la factura.</param>
        /// <param name="dto">Datos del detalle a agregar.</param>
        [HttpPost("facturas/{facturaId:int}/detalles")]
        public async Task<ActionResult<ResponseDTO<object>>> AgregarDetalleFactura(int facturaId, [FromBody] DTE_DetalleDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _certificadoServiceV1.AgregarDetalleFactura(facturaId, dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }



        // BITÁCORA


        /// <summary>
        /// Obtiene el historial completo de la bitácora de eventos.
        /// </summary>
        [HttpGet("bitacora")]
        public async Task<ActionResult<ResponseDTO<List<Logs_BitacoraDTO>>>> ObtenerBitacora()
        {
            var response = await _certificadoServiceV1.ObtenerBitacora();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene los registros de bitácora asociados a una factura.
        /// </summary>
        /// <param name="facturaId">ID de la factura.</param>
        [HttpGet("bitacora/factura/{facturaId:int}")]
        public async Task<ActionResult<ResponseDTO<List<Logs_BitacoraDTO>>>> ObtenerBitacoraPorFactura(int facturaId)
        {
            var response = await _certificadoServiceV1.ObtenerBitacoraPorFactura(facturaId);
            return response.Success ? Ok(response) : BadRequest(response);
        }



        // VALIDACIÓN & FIRMA


        /// <summary>
        /// Ejecuta la validación de un DTE en proceso.
        /// </summary>
        /// <param name="facturaId">ID del DTE (factura) a validar.</param>
        [HttpPost("facturas/{facturaId:int}/validar")]
        public async Task<ActionResult<ResponseDTO<ValidacionResultadoDTO>>> ValidarDTE(int facturaId)
        {
            var response = await _certificadoServiceV1.ValidarDTE(facturaId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Aplica la firma electrónica a un DTE validado.
        /// </summary>
        /// <param name="facturaId">ID del DTE (factura) a firmar.</param>
        [HttpPost("facturas/{facturaId:int}/firmar")]
        public async Task<ActionResult<ResponseDTO<FirmaResultadoDTO>>> FirmarDTE(int facturaId)
        {
            var response = await _certificadoServiceV1.FirmarDTE(facturaId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
