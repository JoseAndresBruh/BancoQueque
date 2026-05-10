# Banco Queque - Dominio Core

Este proyecto implementa la capa de dominio para "Banco Queque", una banca virtual privada con cobertura en Manta y a nivel nacional, utilizando estrictamente los principios de **Domain-Driven Design (DDD)**.

## Procesos Implementados

Se han modelado dos procesos fundamentales del área bancaria:

### 1. Gestión de Cuentas (Aggregate Root: `Cuenta`)
Este proceso encapsula la lógica de apertura, bloqueo y modificación de saldo de una cuenta bancaria. 
* **Contribución a DDD:** La entidad `Cuenta` protege sus propios invariantes. No se puede modificar el saldo directamente desde afuera; se deben usar los métodos internos `Acreditar` y `Debitar`. Además, el método `Debitar` utiliza el patrón `Resultado` para evitar excepciones si no hay fondos suficientes, tratando esto como una regla de negocio esperada y no como un error del sistema. Se emiten Eventos de Dominio (`CuentaAbiertaEventoDominio`, `CuentaBloqueadaEventoDominio`) para notificar a otras partes del sistema cuando el estado cambia.

### 2. Transferencias de Fondos (Aggregate Root: `Transferencia`)
Este proceso maneja el movimiento de dinero (`Dinero` como Value Object) entre dos cuentas distintas.
* **Contribución a DDD:** Actúa como un orquestador del dominio que asegura la consistencia eventual. En lugar de mutar las cuentas de forma descontrolada, el método `Iniciar` valida que ambas cuentas estén en un estado válido (`Activa`) y que el monto sea lógico. El método `Ejecutar` coordina el débito y el crédito, asegurando que si la cuenta origen falla al debitar (por ejemplo, fondos insuficientes), la transferencia pase a estado `Fallida` emitiendo el evento correspondiente (`TransferenciaFallidaEventoDominio`), manteniendo la integridad del sistema financiero en todo momento.

