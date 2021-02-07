using BcuWebserviceUltimaCotizacion;
using BcuWebServiceUltimoCierre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CotizacionBCU
{
    /// <summary>
    /// Clase principal para obtener las cotizaciones del Banco Central Del Uruguay
    /// </summary>
    public class Cotizacion
    {
        private static Task<List<Moneda>> ObtenerCotizacionAsync(DateTime? fecha, IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            return Task.Run(async () =>
            {
                List<Moneda> monedasResult = null;
                wsbcucotizacionesSoapPortClient proxy = null;

                try
                {
                    proxy = new wsbcucotizacionesSoapPortClient();
                    var entrada = new wsbcucotizacionesin
                    {
                        FechaDesde = fecha,
                        FechaHasta = fecha,
                        Moneda = CodigosMonedas(monedas)
                    };

                    await proxy.OpenAsync();
                    var response = await proxy.ExecuteAsync(entrada);

                    //Todo OK
                    if (response?.Salida?.respuestastatus?.status == 1 && response?.Salida?.datoscotizaciones?.Length > 0)
                        monedasResult = response.Salida.datoscotizaciones.Select(c => new Moneda
                        {
                            CodigoISO = c.CodigoISO,
                            Compra = c.TCC,
                            Venta = c.TCV,
                            Emisor = c.Emisor,
                            Fecha = c.Fecha,
                            Nombre = c.Nombre,
                            Arbitraje = c.ArbAct,
                            Codigo = c.Moneda
                        }).ToList();

                    else
                        throw new CotizacionException("No se ha podido obtener la cotizacion | " + response.Salida.respuestastatus.mensaje);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await proxy?.CloseAsync();
                }
                return monedasResult;
            });
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas solicitadas
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static Task<List<Moneda>> ObtenerUltimaAsync(IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            return Task.Run(async () =>
            {
                List<Moneda> _monedas = null;
                wsultimocierreSoapPortClient proxy = null;

                try
                {
                    proxy = new wsultimocierreSoapPortClient();
                    await proxy.OpenAsync();
                    var response = await proxy.ExecuteAsync();

                    _monedas = await ObtenerCotizacionAsync(response.Salida.Fecha, monedas);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    proxy?.CloseAsync();
                }
                return _monedas;
            });
        }

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas solicitadas
        /// </summary>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static List<Moneda> ObtenerUltima(IEnumerable<Moneda.TipoMonedaEnum> monedas) => RunSync(() => ObtenerUltimaAsync(monedas));

        /// <summary>
        /// Retorna la ultima cotizacion disponible de la moneda requerida.
        /// </summary>
        /// <param name="moneda">Moneda la cual se desea la cotizacion</param>
        /// <returns></returns>
        public static Task<Moneda> ObtenerUltimaAsync(Moneda.TipoMonedaEnum moneda) => Task.Run(async () => (await ObtenerUltimaAsync(new[] { moneda })).Single());

        /// <summary>
        /// Retorna la ultima cotizacion disponible de la moneda requerida.
        /// </summary>
        /// <param name="moneda">Moneda la cual se desea la cotizacion</param>
        /// <returns></returns>
        public static Moneda ObtenerUltima(Moneda.TipoMonedaEnum moneda) => RunSync(() => ObtenerUltimaAsync(moneda));

        /// <summary>
        /// Obtiene la ultima cotizacion de las monedas por defecto DOLAR USA, PESO ARG, REAL, UI ,UR, EURO
        /// </summary>
        /// <returns></returns>
        public static Task<List<Moneda>> ObtenerUltimaAsync()
        {
            return ObtenerUltimaAsync(new List<Moneda.TipoMonedaEnum>
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
        /// Obtiene la ultima cotizacion de las monedas por defecto DOLAR USA, PESO ARG, REAL, UI ,UR, EURO
        /// </summary>
        /// <returns></returns>
        public static List<Moneda> ObtenerUltima() => RunSync(() => ObtenerUltimaAsync());

        /// <summary>
        /// Obtiene las cotizaciones de las monedas en la fecha de cierre especificada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static Task<List<Moneda>> ObtenerAsync(DateTime? fecha, IEnumerable<Moneda.TipoMonedaEnum> monedas)
        {
            return ObtenerCotizacionAsync(fecha, monedas);
        }

        /// <summary>
        /// Obtiene las cotizaciones de las monedas en la fecha de cierre especificada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="monedas"></param>
        /// <returns></returns>
        public static List<Moneda> Obtener(DateTime? fecha, IEnumerable<Moneda.TipoMonedaEnum> monedas) => RunSync(() => ObtenerAsync(fecha, monedas));

        /// <summary>
        /// Obtiene la cotizacion de una moneda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="moneda"></param>
        /// <exception cref="CotizacionException"></exception>
        /// <returns></returns>
        public static Task<Moneda> ObtenerAsync(DateTime? fecha, Moneda.TipoMonedaEnum moneda) => Task.Run(async () => (await ObtenerCotizacionAsync(fecha, new[] { moneda })).Single());

        /// <summary>
        /// Obtiene la cotizacion de una moneda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="moneda"></param>
        /// <exception cref="CotizacionException"></exception>
        /// <returns></returns>
        public static Moneda Obtener(DateTime? fecha, Moneda.TipoMonedaEnum moneda) => RunSync(() => ObtenerAsync(fecha, moneda));

        private static TResult RunSync<TResult>(Func<Task<TResult>> task) =>

          Task.Factory.StartNew(task)
                      .Unwrap()
                      .GetAwaiter()
                      .GetResult();
        private static short[] CodigosMonedas(IEnumerable<Moneda.TipoMonedaEnum> monedas) => monedas.Select(m => TipoMonedaToCodigo(m)).ToArray();
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
