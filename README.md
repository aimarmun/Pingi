# Pingi

Una simple aplicación de consola para realizar Ping a una IP por protocolo ICMP.
Pero.. también puede relizar Ping a un puerto TCP específico. Es decir, comprueba si el puerto indicado esta abierto y devuelve una respuesta.

Digamos que es util para saber si un equipo responde a un puerto específico.

Puede escribir los resultados en un archivo de texto

## Argumentos admitidos

```shell
  -h, --host          Obligatorio. Indica el nombre de host remoto.

  -o, --outputfile    Archivo de salida donde se guardan los resultados

  -p, --port          Indica un puerto TCP específico, Pingi intentará comprobar si el puerto se encuentra abierto y lo considerará como "responde"

  -n, --nodelay       Indica si se tiene que enviar inmediatamente otro Ping despues de terminar otro.

  --help              Display this help screen.

  --version           Display version information.
```

> NOTA: Cuando se escriben los resultados en un archivo de texto, solo se escriben los cambios en la respuesta, es decir, si falla 200 veces solo se escribe una vez a que hora ha fallado, si luego cambia el estado a _"responde"_ entonces se vuelve a escribir un registro con la hora en la que respondió. Al finalizar Pingi siempre se escribe un resumen con las estadísticas de respuesta.

  - Ejemplo:
    ```pingi.exe -h 192.168.1.1 -o results.txt -p 80```

  - Ejemplo de archivo de salida:
    ```shell
    [4/27/2024 12:43:31PM]	 Estado: Success  Tiempo: 113ms Dirección: 192.168.1.64
    [4/27/2024 12:43:42PM]	 Estado: TimedOut  Tiempo: 3000ms. Dirección: 192.168.1.64
    [4/27/2024 12:44:00PM]	 Estado: Success  Tiempo: 239ms. Dirección: 192.168.1.64
    [4/27/2024 12:44:05PM]	 Finalizado!
	Estadisticas: 
	Aciertos: 13
	Fallos:   5
	72% a una media de 97ms. 
    ```

## Como compilar
- Desde Visual Studio: Clona el repositorio, ábrelo en Visual Studio y presiona F5 o publicalo.
- Desde Visual Stuido Code: Clona el repositorio, ve a la carpeta ```Pingi```dentro de la carpeta del proyecto y ejecuta ```dotnet run``` o ```dotnet publish```para publicar el proyecto.
