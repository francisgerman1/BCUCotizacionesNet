# BCUCotizacionesNetFramework ![Download Laps](https://img.shields.io/github/license/francisgerman1/BCUCotizacionesNetFramework)
Librería .NET para consultar las cotizaciones del Banco Central del Uruguay

### Instalación

1. [![Download Laps](https://img.shields.io/badge/DLL-Descargar-green)](https://github.com/francisgerman1/BCUCotizacionesNetFramework/releases/latest/download/CotizacionBCU.dll)
2. Copiar este código en tu archivo de configuración
```xml
<configuration>
	<system.serviceModel>
		<bindings>
			<basicHttpBinding>
				<binding name="wsultimocierreSoapBinding">
					<security mode="Transport" />
				</binding>
				<binding name="wsultimocierreSoapBinding1" />
				<binding name="wsbcucotizacionesSoapBinding">
					<security mode="Transport" />
				</binding>
				<binding name="wsbcucotizacionesSoapBinding1" />
			</basicHttpBinding>
		</bindings>
		<client>
			<endpoint address="https://cotizaciones.bcu.gub.uy/wscotizaciones/servlet/awsultimocierre"
                binding="basicHttpBinding" bindingConfiguration="wsultimocierreSoapBinding"
                contract="UltimoCierreBCU.wsultimocierreSoapPort" name="wsultimocierreSoapPort" />
			<endpoint address="https://cotizaciones.bcu.gub.uy/wscotizaciones/servlet/awsbcucotizaciones"
                binding="basicHttpBinding" bindingConfiguration="wsbcucotizacionesSoapBinding"
                contract="WS_CotizacionBCU.wsbcucotizacionesSoapPort" name="wsbcucotizacionesSoapPort" />
		</client>
	</system.serviceModel>
</configuration>
```
