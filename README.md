# Pingi

Una simple aplicación de consola para realizar Ping a una IP por protocolo ICMP.
Pero.. también puede relizar Ping a un puerto TCP específico. Es decir, comprueba si el puerto indicado esta abierto y devuelve una respuesta.

Digamos que es util para saber si un equipo responde a un puerto específico.

Puede escribir los resultados en un archivo de texto

### Argumentos admitidos

``-h`` o ``-host`` ``<IP_destino>`` IP o nombre de host
``-o`` o ``-outputfile`` ``<path_fichero.txt>`` indica que se quieren escribir los resultados en un archivo de texto
``-p`` o ``-port`` ``<port_to_ping>`` puerto al que queremos hacer Ping. Si se indica se comprobará si el puerto está abierto, si no se indica, se realizará un Ping ICMP.

  - Ejemplo:
    ```pingi.exe -h 192.168.1.1 -o results.txt -p 80```

## Como compilar
Simplemente baja el proyecto, ábrelo en Visual Studio y presiona F5 o publicalo.
