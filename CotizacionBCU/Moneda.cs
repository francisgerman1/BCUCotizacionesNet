using System;

namespace CotizacionBCU
{
    /// <summary>
    /// Clase Moneda
    /// </summary>
    public class Moneda
    {
        /// <summary>
        /// Codigo de moneda que maneja el BCU
        /// </summary>
        public short Id { get; set; }
        /// <summary>
        /// Nombre de Moneda
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Valor Compra
        /// </summary>
        public double Compra { get; set; }

        /// <summary>
        /// Valor Venta
        /// </summary>
        public double Venta { get; set; }

        /// <summary>
        /// Fecha de Cotizacion
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Codigo ISO
        /// </summary>
        public string CodigoISO { get; set; }

        /// <summary>
        /// Emisor de Moneda
        /// </summary>
        public string Emisor { get; set; }

        /// <summary>
        /// Tipos de monedas
        /// </summary>
        public enum TipoMonedaEnum
        {
            EURO, DOLAR_USD, REAL, PESO_ARGENTINO, DOLAR_CANADIENSE,
            PESO_CHILENO, YUAN_OFF_SHORE, YUAN_RENMIMBI, PESO_COLOMBIANO,
            CORONA_DANESA, DOLAR_HONG_KONG, FORINT_HUNGARO,
            RUPIA_INDIA, CORONA_ISLANDESA, YEN, WON, RINGGIT_MALAYO, PESO_MEXICANO,
            CORONA_NORUEGA, GUARANI, NVO_SOL_PERUANO, RUBLO, RAND_SUDAFRICANO,
            CORONA_SUECA, FRANCO_SUIZO, LIRA_TURCA, BOLIVAR, DOL_AUSTRALIANO,
            LIBRA_ESTERLINA, DOL_NEOZELANDES, DER_ESP_DE_GIRO,
            UNIDAD_INDEXADA, UNIDAD_REAJUSTABLE
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre} " +
                   $"Compra: {Compra:C} " +
                   $"Venta: {Venta:C} " +
                   $"Fecha: {Fecha:D} " +
                   $"Emisor: {Emisor} " +
                   $"ISO: {CodigoISO} ";
        }
    }
}
