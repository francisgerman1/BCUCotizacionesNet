//MIT License

//Copyright(c) 2019 Francis Espindola

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   $"Compra: {Compra.ToString("C")} " +
                   $"Venta: {Venta.ToString("C")} " +
                   $"Fecha: {Fecha.ToString("D")} " +
                   $"Emisor: {Emisor} " +
                   $"ISO: {CodigoISO} ";
        }
    }
}
