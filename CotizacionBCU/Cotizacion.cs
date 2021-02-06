using CotizacionBCU.UltimoCierreBCU;
using CotizacionBCU.WS_CotizacionBCU;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CotizacionBCU
{
    /// <summary>
    /// Clase principal para obtener las cotizaciones del Banco Central Del Uruguay
    /// </summary>
    public class Cotizacion
    {
        /// <summary>
        /// Retorna la ultima cotizacion disponible de la moneda requerida.
        /// </summary>
        /// <param name="tipoMoneda">Moneda la cual se desea la cotizacion</param>
        /// <returns></returns>
        public static Moneda ObtenerUltima(Moneda.TipoMonedaEnum moneda)
        {
            Moneda m = null;

            //Proxy BCU
            wsultimocierreSoapPortClient proxy = null;

            try
            {
                proxy = new wsultimocierreSoapPortClient();
                proxy.Open();
                wsultimocierreout resUltFch = proxy.Execute();
                string ultFch = resUltFch.Fecha;

                m = ObtenerCotizacion(ultFch, moneda);
            }
            catch
            {
                throw;
            }
            finally
            {
                proxy?.Close();
            }
            return m;
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas solicitadas
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static List<Moneda> ObtenerUltima(IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            return monedas.Select(m => ObtenerUltima(m)).ToList();
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas por defecto DOLAR USA, PESO ARG, REAL, UI ,UR, EURO
        /// </summary>
        /// <returns></returns>
        public static List<Moneda> ObtenerUltima()
        {
            return ObtenerUltima(new List<Moneda.TipoMonedaEnum>
            {
                Moneda.TipoMonedaEnum.DOLAR_USD,
                Moneda.TipoMonedaEnum.PESO_ARGENTINO,
                Moneda.TipoMonedaEnum.REAL,
                Moneda.TipoMonedaEnum.UNIDAD_INDEXADA,
                Moneda.TipoMonedaEnum.UNIDAD_REAJUSTABLE,
                Moneda.TipoMonedaEnum.EURO
            });
        }

        /// <summary>
        /// Obtiene las cotizaciones de las monedas en la fecha de cierre especificada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static List<Moneda> Obtener(DateTime fechaCierre, IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            var fecha = fechaCierre.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return monedas.Select(m => ObtenerCotizacion(fecha, m)).ToList();
        }

        /// <summary>
        /// Obtiene la cotizacion de la moneda especificada en la fecha de cierre del banco
        /// </summary>
        /// <param name="fecha">Fecha de cierre del BCU</param>
        /// <param name="moneda">Tipo de moneda</param>
        /// <exception cref="CotizacionException"></exception>
        /// <returns></returns>
        public static Moneda Obtener(DateTime fecha, Moneda.TipoMonedaEnum moneda)
        {
            return ObtenerCotizacion(fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), moneda);
        }

        /// <summary>
        /// Obtiene la cotizacion de una moneda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="moneda"></param>
        /// <exception cref="CotizacionException"></exception>
        /// <returns></returns>
        private static Moneda ObtenerCotizacion(string fecha, Moneda.TipoMonedaEnum moneda)
        {
            Moneda m = null;

            //Proxy BCU
            wsbcucotizacionesSoapPortClient proxy = null;

            try
            {
                proxy = new wsbcucotizacionesSoapPortClient();
                var entrada = new wsbcucotizacionesin
                {
                    FechaDesde = fecha,
                    FechaHasta = fecha,
                    Moneda = CodigosMonedas(moneda)
                };

                proxy.Open();
                var res = proxy.Execute(entrada);

                //Todo OK
                if (res?.respuestastatus.status == 1 && res?.datoscotizaciones?.Count > 0)
                    m = new Moneda
                    {
                        Id = TipoMonedaToCodigo(moneda),
                        Nombre = res.datoscotizaciones.First().Nombre,
                        Compra = res.datoscotizaciones.First().TCC,
                        Venta = res.datoscotizaciones.First().TCV,
                        Emisor = res.datoscotizaciones.First().Emisor,
                        CodigoISO = res.datoscotizaciones.First().CodigoISO,
                        Fecha = DateTime.ParseExact(res.datoscotizaciones.First().Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture)
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
                proxy?.Close();
            }
            return m;
        }

        /// <summary>
        /// Pasa los enums de tipo moneda al codigo que reconoce el BCU
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        private static ArrayOfint CodigosMonedas(IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            var ret = new ArrayOfint();

            foreach (var moneda in monedas)
                ret.Add(TipoMonedaToCodigo(moneda));

            return ret;
        }

        /// <summary>
        /// Pasa los enums de tipo moneda al codigo que reconoce el BCU
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        private static ArrayOfint CodigosMonedas(Moneda.TipoMonedaEnum moneda)
        {
            return CodigosMonedas(new List<Moneda.TipoMonedaEnum> { moneda });
        }

        /// <summary>
        /// Enum a codigo moneda BCU
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private static short TipoMonedaToCodigo(Moneda.TipoMonedaEnum tipo)
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
