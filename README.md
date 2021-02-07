# BCUCotizacionesNet ![Download Laps](https://img.shields.io/github/license/francisgerman1/BCUCotizacionesNetFramework)
Wrapper .NET para consultar las cotizaciones del Banco Central del Uruguay

## Instalación

Package Manager o .Net CLI

```
Install-Package BCUCotizaciones
```
```
dotnet add package BCUCotizaciones
```

## Como usar

### Última cotización de las monedas por defecto DOLAR USA, PESO ARG, REAL, UI ,UR, EURO
```csharp
List<Moneda> cotizaciones = Cotizacion.ObtenerUltima();
```

### Última cotización disponible de la moneda requerida.
```csharp
Moneda cotizacion = Cotizacion.ObtenerUltima(Moneda.TipoMonedaEnum.DOLAR_USD);
```

### Última cotización de las monedas solicitadas
```csharp
var solicitadas = new List<Moneda.TipoMonedaEnum>();
solicitadas.Add(Moneda.TipoMonedaEnum.EURO);
solicitadas.Add(Moneda.TipoMonedaEnum.DOLAR_USD);
List<Moneda> cotizaciones = Cotizacion.ObtenerUltima(solicitadas);
```


## Plataformas

* .NET Core 2.0
* .NET Framework 4.6.1
* Mono 5.4
* Xamarin.iOS 10.14
* Xamarin.Mac 3.8
* Xamarin.Android 8.0
* Universal Windows Platform 10.0.16299
