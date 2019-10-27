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


using CotizacionBCU.UltimoCierreBCU;
using CotizacionBCU.WS_CotizacionBCU;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionBCU
{
    /// <summary>
    /// Clase principal para obtener las cotizaciones del Banco Central Del Uruguay
    /// </summary>
    public class Cotizacion
    {
        #region SINGLETON
        private static Cotizacion instancia;
        private Cotizacion() { }
        public static Cotizacion Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new Cotizacion();
                return instancia;
            }
        }
        #endregion


        /// <summary>
        /// Retorna la ultima cotizacion disponible de la moneda requerida.
        /// </summary>
        /// <param name="tipoMoneda">Moneda la cual se desea la cotizacion</param>
        /// <returns></returns>
        public Moneda UltimaCotizacion(Moneda.TipoMonedaEnum moneda)
        {
            Moneda m = null;

            //Proxy donde obtengo la ultima fecha de cierre del BCU
            wsultimocierreSoapPortClient client_ult_cierre = null;

            try
            {
                client_ult_cierre = new wsultimocierreSoapPortClient();
                client_ult_cierre.Open();
                wsultimocierreout resUltFch = client_ult_cierre.Execute();
                string ultFch = resUltFch.Fecha;

                m = ObtenerCotizacion(ultFch, moneda);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (client_ult_cierre != null)
                    client_ult_cierre.Close();
            }
            return m;
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas solicitadas
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public Moneda[] UltimaCotizacion(Moneda.TipoMonedaEnum[] monedas)
        {
            Moneda[] ret = new Moneda[monedas.Length];
            for (int i = 0; i < monedas.Length; i++)
                ret[i] = UltimaCotizacion(monedas[i]);
            return ret;
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas por defecto DOLAR USA, PESO ARG, REAL, UI ,UR, EURO
        /// </summary>
        /// <returns></returns>
        public Moneda[] UltimaCotizacion()
        {
            Moneda.TipoMonedaEnum[] monedas = {
                Moneda.TipoMonedaEnum.DOLAR_USD,
                Moneda.TipoMonedaEnum.PESO_ARGENTINO,
                Moneda.TipoMonedaEnum.REAL,
                Moneda.TipoMonedaEnum.UNIDAD_INDEXADA,
                Moneda.TipoMonedaEnum.UNIDAD_REAJUSTABLE,
                Moneda.TipoMonedaEnum.EURO };

            return UltimaCotizacion(monedas);
        }

        /// <summary>
        /// Obtiene las cotizaciones de las monedas en la fecha de cierre especificada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public Moneda[] Cotizaciones(DateTime fecha, Moneda.TipoMonedaEnum[] monedas)
        {
            Moneda[] ret = new Moneda[monedas.Length];
            var array_monedas = CodMonedas(monedas);
            string fechaStr = fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            for (int i = 0; i < ret.Length; i++)
                ret[i] = ObtenerCotizacion(fechaStr, monedas[i]);
            return ret;
        }

        /// <summary>
        /// Obtiene la cotizacion de una moneda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="moneda"></param>
        /// <returns></returns>
        private Moneda ObtenerCotizacion(string fecha, Moneda.TipoMonedaEnum moneda)
        {
            Moneda m = null;

            //Proxy WebService donde obtengo las cotizaciones
            wsbcucotizacionesSoapPortClient client_Bcu = null;

            try
            {
                client_Bcu = new wsbcucotizacionesSoapPortClient();
                wsbcucotizacionesin entrada = new wsbcucotizacionesin();

                entrada.FechaDesde = fecha;
                entrada.FechaHasta = fecha;
                entrada.Moneda = CodMonedas(new Moneda.TipoMonedaEnum[] { moneda });

                client_Bcu.Open();
                var res = client_Bcu.Execute(entrada);

                //Todo OK
                if (res != null && res.respuestastatus.status == 1 && res.datoscotizaciones.Count > 0)
                    m = new Moneda()
                    {
                        Id = TipoMonedaToCodigo(moneda),
                        Nombre = res.datoscotizaciones[0].Nombre,
                        Compra = res.datoscotizaciones[0].TCC,
                        Venta = res.datoscotizaciones[0].TCV,
                        Emisor = res.datoscotizaciones[0].Emisor,
                        CodigoISO = res.datoscotizaciones[0].CodigoISO,
                        Fecha = DateTime.ParseExact(res.datoscotizaciones[0].Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    };
                else
                    throw new CotizacionException("No se ha podido obtener la cotizacion | " + res.respuestastatus.mensaje);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (client_Bcu != null)
                    client_Bcu.Close();
            }
            return m;
        }

        /// <summary>
        /// Obtiene la cotizacion de la moneda especificada en la fecha de cierre del banco
        /// </summary>
        /// <param name="fecha">Fecha de cierre del BCU</param>
        /// <param name="moneda">Tipo de moneda</param>
        /// <exception cref="CotizacionException"></exception>
        /// <returns></returns>
        public Moneda ObtenerCotizacion(DateTime fecha, Moneda.TipoMonedaEnum moneda)
        {
            return ObtenerCotizacion(fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), moneda);
        }

        /// <summary>
        /// Pasa los enums de tipo moneda al codigo que reconoce el BCU
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        private ArrayOfint CodMonedas(Moneda.TipoMonedaEnum[] monedas)
        {
            ArrayOfint ret = new ArrayOfint();
            for (int i = 0; i < monedas.Length; i++)
                ret.Add(TipoMonedaToCodigo(monedas[i]));
            return ret;
        }

        /// <summary>
        /// Enum a codigo moneda BCU
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private short TipoMonedaToCodigo(Moneda.TipoMonedaEnum tipo)
        {
            switch (tipo)
            {
                case Moneda.TipoMonedaEnum.BOLIVAR:
                    return 6200;
                case Moneda.TipoMonedaEnum.CORONA_DANESA:
                    return 1800;
                case Moneda.TipoMonedaEnum.CORONA_ISLANDESA:
                    return 4900;
                case Moneda.TipoMonedaEnum.CORONA_NORUEGA:
                    return 4600;
                case Moneda.TipoMonedaEnum.CORONA_SUECA:
                    return 5800;
                case Moneda.TipoMonedaEnum.DER_ESP_DE_GIRO:
                    return 0002;
                case Moneda.TipoMonedaEnum.DOLAR_CANADIENSE:
                    return 2309;
                case Moneda.TipoMonedaEnum.DOLAR_USD:
                    return 2225;
                case Moneda.TipoMonedaEnum.DOL_AUSTRALIANO:
                    return 0105;
                case Moneda.TipoMonedaEnum.DOL_NEOZELANDES:
                    return 1490;
                case Moneda.TipoMonedaEnum.EURO:
                    return 1111;
                case Moneda.TipoMonedaEnum.FORINT_HUNGARO:
                    return 4300;
                case Moneda.TipoMonedaEnum.FRANCO_SUIZO:
                    return 5900;
                case Moneda.TipoMonedaEnum.GUARANI:
                    return 4800;
                case Moneda.TipoMonedaEnum.DOLAR_HONG_KONG:
                    return 5100;
                case Moneda.TipoMonedaEnum.LIBRA_ESTERLINA:
                    return 2700;
                case Moneda.TipoMonedaEnum.LIRA_TURCA:
                    return 4400;
                case Moneda.TipoMonedaEnum.UNIDAD_INDEXADA:
                    return 9800;
                case Moneda.TipoMonedaEnum.UNIDAD_REAJUSTABLE:
                    return 9900;
                case Moneda.TipoMonedaEnum.NVO_SOL_PERUANO:
                    return 4000;
                case Moneda.TipoMonedaEnum.PESO_ARGENTINO:
                    return 0501;
                case Moneda.TipoMonedaEnum.PESO_CHILENO:
                    return 1300;
                case Moneda.TipoMonedaEnum.PESO_COLOMBIANO:
                    return 5500;
                case Moneda.TipoMonedaEnum.PESO_MEXICANO:
                    return 4200;
                case Moneda.TipoMonedaEnum.RAND_SUDAFRICANO:
                    return 1620;
                case Moneda.TipoMonedaEnum.REAL:
                    return 1001;
                case Moneda.TipoMonedaEnum.RINGGIT_MALAYO:
                    return 5600;
                case Moneda.TipoMonedaEnum.RUBLO:
                    return 5400;
                case Moneda.TipoMonedaEnum.RUPIA_INDIA:
                    return 5700;
                case Moneda.TipoMonedaEnum.WON:
                    return 5300;
                case Moneda.TipoMonedaEnum.YEN:
                    return 3600;
                case Moneda.TipoMonedaEnum.YUAN_OFF_SHORE:
                    return 4155;
                case Moneda.TipoMonedaEnum.YUAN_RENMIMBI:
                    return 4150;
                default:
                    return 0;
            }
        }

    }
}
