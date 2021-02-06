using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CotizacionBCU.Tests
{
    /// <summary>
    /// Descripción resumida de CotizacionTest
    /// </summary>
    [TestClass]
    public class CotizacionTest
    {
        private List<Moneda.TipoMonedaEnum> _todasMonedas = new List<Moneda.TipoMonedaEnum>
        {
                Moneda.TipoMonedaEnum.EURO,
                Moneda.TipoMonedaEnum.DOLAR_USD,
                Moneda.TipoMonedaEnum.REAL,
                Moneda.TipoMonedaEnum.PESO_ARGENTINO,
                Moneda.TipoMonedaEnum.DOLAR_CANADIENSE,
                Moneda.TipoMonedaEnum.PESO_CHILENO,
                Moneda.TipoMonedaEnum.YUAN_OFF_SHORE,
                Moneda.TipoMonedaEnum.YUAN_RENMIMBI,
                Moneda.TipoMonedaEnum.PESO_COLOMBIANO,
                Moneda.TipoMonedaEnum.CORONA_DANESA,
                Moneda.TipoMonedaEnum.DOLAR_HONG_KONG,
                Moneda.TipoMonedaEnum.FORINT_HUNGARO,
                Moneda.TipoMonedaEnum.RUPIA_INDIA,
                Moneda.TipoMonedaEnum.CORONA_ISLANDESA,
                Moneda.TipoMonedaEnum.YEN,
                Moneda.TipoMonedaEnum.WON,
                Moneda.TipoMonedaEnum.RINGGIT_MALAYO,
                Moneda.TipoMonedaEnum.PESO_MEXICANO,
                Moneda.TipoMonedaEnum.CORONA_NORUEGA,
                Moneda.TipoMonedaEnum.GUARANI,
                Moneda.TipoMonedaEnum.NVO_SOL_PERUANO,
                Moneda.TipoMonedaEnum.RUBLO,
                Moneda.TipoMonedaEnum.RAND_SUDAFRICANO,
                Moneda.TipoMonedaEnum.CORONA_SUECA,
                Moneda.TipoMonedaEnum.FRANCO_SUIZO,
                Moneda.TipoMonedaEnum.LIRA_TURCA,
                Moneda.TipoMonedaEnum.BOLIVAR,
                Moneda.TipoMonedaEnum.DOL_AUSTRALIANO,
                Moneda.TipoMonedaEnum.LIBRA_ESTERLINA,
                Moneda.TipoMonedaEnum.DOL_NEOZELANDES,
                Moneda.TipoMonedaEnum.DER_ESP_DE_GIRO,
                Moneda.TipoMonedaEnum.UNIDAD_INDEXADA,
                Moneda.TipoMonedaEnum.UNIDAD_REAJUSTABLE
        };

        public CotizacionTest()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la serie de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        //
        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
        //
        // Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup para ejecutar el código una vez ejecutadas todas las pruebas en una clase
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ObtenerUltima()
        {
            var ultima = Cotizacion.ObtenerUltima();

            Assert.IsNotNull(ultima);
            Assert.IsTrue(ultima.Count > 0);

            foreach (var c in ultima)
            {
                Assert.IsTrue(c.Compra > 0);
                Assert.IsTrue(c.Venta > 0);
                Assert.IsTrue(c.Fecha >= DateTime.Now.AddDays(-7));
                Assert.IsFalse(string.IsNullOrEmpty(c.Emisor));
                Assert.IsFalse(string.IsNullOrEmpty(c.Nombre));
                Assert.IsFalse(string.IsNullOrEmpty(c.CodigoISO));
            }
        }
        [TestMethod]
        public void ObtenerUltimaConUnaSola()
        {
            var m = Cotizacion.ObtenerUltima(Moneda.TipoMonedaEnum.DOLAR_USD);

            Assert.IsNotNull(m);
            Assert.IsTrue(m.Compra > 0);
            Assert.IsTrue(m.Venta > 0);
            Assert.IsTrue(m.Fecha >= DateTime.Now.AddDays(-7));
            Assert.IsFalse(string.IsNullOrEmpty(m.Emisor));
            Assert.IsFalse(string.IsNullOrEmpty(m.Nombre));
            Assert.IsFalse(string.IsNullOrEmpty(m.CodigoISO));
        }

        [TestMethod]
        public void ObtenerUltimaTodas()
        {
            var ultima = Cotizacion.ObtenerUltima(_todasMonedas);

            Assert.IsNotNull(ultima);
            Assert.IsTrue(ultima.Count > 0);

            foreach (var c in ultima)
            {
                Assert.IsTrue(c.Compra > 0);
                Assert.IsTrue(c.Venta > 0);
                Assert.IsTrue(c.Fecha >= DateTime.Now.AddDays(-7));
                Assert.IsFalse(string.IsNullOrEmpty(c.Emisor));
                Assert.IsFalse(string.IsNullOrEmpty(c.Nombre));
                Assert.IsFalse(string.IsNullOrEmpty(c.CodigoISO));
            }
        }

        [TestMethod]
        public void ObtenerUltimaConFecha()
        {
            var m = Cotizacion.Obtener(UltimoViernes(), Moneda.TipoMonedaEnum.DOLAR_USD);

            Assert.IsNotNull(m);
            Assert.IsTrue(m.Compra > 0);
            Assert.IsTrue(m.Venta > 0);
            Assert.IsTrue(m.Fecha >= DateTime.Now.AddDays(-7));
            Assert.IsFalse(string.IsNullOrEmpty(m.Emisor));
            Assert.IsFalse(string.IsNullOrEmpty(m.Nombre));
            Assert.IsFalse(string.IsNullOrEmpty(m.CodigoISO));
        }

        [TestMethod]
        public void ObtenerUltimaConFechaTodas()
        {
            var ultima = Cotizacion.Obtener(UltimoViernes(), _todasMonedas);

            Assert.IsNotNull(ultima);
            Assert.IsTrue(ultima.Count > 0);

            foreach (var c in ultima)
            {
                Assert.IsTrue(c.Compra > 0);
                Assert.IsTrue(c.Venta > 0);
                Assert.IsTrue(c.Fecha >= DateTime.Now.AddDays(-7));
                Assert.IsFalse(string.IsNullOrEmpty(c.Emisor));
                Assert.IsFalse(string.IsNullOrEmpty(c.Nombre));
                Assert.IsFalse(string.IsNullOrEmpty(c.CodigoISO));
            }
        }

        private DateTime UltimoViernes()
        {
            DateTime ultimoViernes = DateTime.Now.AddDays(-1);
            while (ultimoViernes.DayOfWeek != DayOfWeek.Wednesday)
                ultimoViernes = ultimoViernes.AddDays(-1);
            return ultimoViernes;
        }
    }
}
