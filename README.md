# Estándar de codificación
---
## Introducción

El estándar de codificación es una herramienta que debe acompañar a cada programador que se involucre en la construcción de este software. El estándar de codificación funciona como una guía durante la escritura y estructuración del código, estableciendo pautas, reglas y principios que se deben seguir para mantener la consistencia en todo el código fuente.

Es importante acoplarse al estándar en todo momento para generar efectos positivos en  la comunicación y colaboración entre los desarrolladores, logrando un código más limpio, fácil de leer y comprensible. Además, en un estándar de codificación no solo se especifican pautas para dar estilo al código, si no que se hace énfasis en las buenas prácticas que se deben seguir para mantener la robustez del software.

Este estándar contiene las siguientes secciones:

- **Reglas de nombrado:** establece las pautas para el nombramiento de variables, constantes, métodos, clases, propiedades (una característica especial del lenguaje utilizado C#) y espacios de nombre (namespaces).
- **Estilo:** esta sección se enfoca en la estructura visual del código como la indentación, uso de comentarios y la distribución de elementos en las estructuras de control.
- **Manejo de excepciones:** aquí se abarcan las reglas que se deben seguir para implementar buenas prácticas en la captura de excepciones y cuales deben ser registradas en la bitácora de errores de la aplicación.
- **Prácticas seguras de construcción:** en este apartado se encuentran las estrategias que se deben seguir en determinadas áreas del código para mantener la robustez del software.

---

## Propósito
Éste estandar servirá como guia para la escritura homogenea, legible y segura del código en el proyecto de tecnologías para la construcción de software, el cual estará escrito en C# y apegandose mayormente al estandar establecido por microsoft.

El proyecto seguirá el paradigma de orientación a objetos y estará escrito en inglés.

El proyecto se trata de un juego multijugador en linea ...

---

## Reglas de nombrado

**Generales**
- Utilice nombres descriptivos para variables, constantes, métodos, clases, propiedades y espacios de nombre (namespaces).
- Utilice la notación PascalCase para los nombres de *propiedades, métodos, clases, proyectos y namespaces*. Esta consiste en comenzar cada palabra en mayúsculas, sin espacios ni guiones.
- Utilice la notación camelCase para los parámetros de métodos y las variables. Esta consiste en comenzar la primera palabra con minúscula y las siguiente con mayúscula, sin espacios ni guiones.
    - Las variables internas y privadas deberán tener el prefijo "\_" y se usará _readonly_ siempre que sea posible.
- Los nombres de las interfaces deben llevar el prefijo "I".
- Utilizar un sufijo en los nombres de los enums que haga alusión a su naturaleza, por ejemplo "Type" o "State".

***Ejemplo:***
```C#
namespace GameApplication.Accounts
{
    public enum AccountType
    {
        Basic,
        Premium
    }

    public interface IAccount
    {
        int AccountId { get; }
        AccountType Type { get; }
        void GrowLevel();
    }

    public class BasicAccount : IAccount
    {
        private int _accountId;

        public BasicAccount(int accountId)
        {
            //code
        }

        public int AccountId => _accountId;
        public AccountType Type => AccountType.Basic;

        public void GrowLevel()
        {
            //Code
        }
    }
}
```

***Así no:***
```C#
namespace application
{
    public enum types
    {
        Basic,
        Premium
    }

    public interface Account
    {
        int id { get; }
        types type { get; }
        void growLevel();
    }

    public class BasicAccount : Account
    {
        private int ID;

        public BasicAccount(int id)
        {
            //code
        }

        public int id => ID;
        public types type => types.Basic;

        public void growLevel()
        {
            //Code
        }
    }
}
```

**Constantes:**
- Utilizar la notación de UPPER_SNAKE_CASE para el nombramiento de constantes.

    ***Ejemplo:***
    ```C#
    const double PI = 3.1416;
    ```

    ***Así no:***
    ```C#
    const double pi = 3.1416;
    ```

**Consejos:**
- Evite el uso de abreviaturas o acrónimos en los nombres, a excepción de las abreviaturas ampliamente conocidas y aceptadas.
- Prefiere la claridad a la brevedad.
- Evite usar nombres de una sola letra, excepto para contadores de bucle simples.

---

## Estilo
- Utilizar 4 espacios para la indentación, no tabulación.
- Se usará estilo de llaves Allman, es decir, las llaves que abren y cierran bloques de código, deben ocupar una sola línea, y empezar a escribir código en la siguiente línea.
- Evitar usar _this_, a menos que sea absolutamente necesario.
- Espacios entre operadores binarios.

***Ejemplo***
```C#
private Foo()
{
   if (Bar == null)
   {
       DoThing();
   }
   
   while (true)
   {
       Do();
   }
}
```
***Así no:***
```C#
private Foo() {
    if (Bar==null) {
        DoThing();
    }
    else {
        DoTheOtherThing();
    }
}
```

**Comentarios**

Solo se utilizarán comentarios de linea, y se escribirán sobre la linea o lineas de codigo del que se habla, al mismo nivel de indentación, y con un espacio despues de los diagonales.

Procurar utilizarlos solamente cuando sea necesario explicar la razón por la que se codificó de una manera específica en alguna parte o con finalidades de documentación para la API de la aplicación
- **Comentarios de documentación:** Todas las clases, structs, enum, funciones, propiedades y campos públicos deben describirse en cuanto a su propósito y uso. Esta regla garantiza que la documentación se genere y difunda correctamente para todas las clases, métodos y propiedades.
- **Marcas de codigo incompleto:** Se utilizarán comentarios "*todo*" (por hacer) en ubicaciones del codigo que se terminarán despues.

***Ejemplo:***
```c#
// <summary>
// The Controller definition defines ...
// </summary>
public class Controller
{
    /// <summary>
    /// The ID assigned to the Controller
    /// </summary>
    public int id;
}
```

***Así no:***
```c#
// The Controller class
public class Controller
{
    public int id; // The ID assigned to the Controller
}
```


### Estructuras de control
- Debe haber un espacio entre la declaración de la estructura de control y el paréntesis.
- Las llaves son necesarias, incluso si solo contendrán una linea.

***Ejemplo***
```C#
if (bar == null) 
{
    //code
}

while (true) 
{
    //code
}
```

***Así no:***
```C#
if(Bar == null)
{
    //code
}
while  (true) 
{
    //code
}
```

---

## Manejo de excepciones
- Las excepciones se manejarán con bloques *try-catch,* y se usará *using* cuando sea posible y más legible que un bloque try-catch.
- No se usará en bloques try-catch el tipo de excepción más alto (Exception), sino errores especificos.
- Se usarán filtros de excepciones en vez de bloques _if_ cuando sean necesarios.
- Las excepciones atrapadas por bloques catch se nombrarán "error".

### Bitácora
Se usará la librería _log4net_ para la bitácora.

Se regitrarán mensajes de información varia (**Info**):
- Guardado de datos de usuario

Los errores se registrarán a la bitacora en ...(que capa o clase) , y si es necesario, serán propagados a las capas superiores para informar al usuario del error.

**Fatal**:
- FileNotFoundException
- SqlException

**Warning**:
- (excepción de falta de conexión)

### Errores personalizados
Para facilitar el registro de errores en bitacora y mensajes de error para el usuario, se manejará la clase _(nombre de clase)_ para propagar los errores y mensajes para el usuario y bitacora.

---

## Prácticas seguras de construcción

1. **Utilizar una cuenta solo con los permisos esenciales para acceder a la base de datos.** Con el fin de reducir el daño potencial en caso de un compromiso de la cuenta, debido a un atacante externo o por un error de programación.
2. **Las credenciales de la cuenta de acceso a la base de datos no deben estar situada dentro del código.** Ya que el código de este proyecto es público y cualquier persona puede utilizarlas para acceder a la base de datos.
3. **Liberar los recursos después de utilzarlos.** Liberar los recursos (como conexiones a bases de datos, Flujos a archivos, etc.) evita consumo innecesario de memoria. Si no se liberan, pueden agotarse los recursos del sistema y llevar a errores durante la ejecución de la aplicación.
4. **Validación de parámetros de entrada en una función.** Para asegurarse que los datos se encuentren como se esperan y garantizar el correcto funcionamiento de la función.
5. **Ningún método debe devolver null como valor de retorno.** Devolver null puede llevar a NullPointerExceptions. Considere devolver un *valor por defecto, una excepción o un objeto opcional* para indicar la ausencia de un valor.
6. **Validación de las entradas de datos del usuario por medio de campos.** Para asegurarse que el usuario introduzca datos válidos para la aplicación y evitar errores en el procesamiento y consistencia de estos.
7. **Respetar el principio de encapsulación.** Utilice siempre campos privados y propiedades públicas si se necesita acceder al campo desde fuera de la clase o struct. Asegúrese de ubicar en el mismo lugar el campo privado y la propiedad pública.
8. **Limitar la cantidad de información crítica al usuario.** No mostrar el error especifico al usuario para evitar revelar información sensible sobre la aplicación, en su lugar, notificarle con un mensaje breve y descriptivo de lo que sucedió.
9. **Encriptación de información de contraseñas de usuario en la base de datos.**

---

## Referencias
[C# identifier naming rules and conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)

[C# Coding Style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)

[Coding guidelines](https://learn.microsoft.com/en-us/mixed-reality/world-locking-tools/documentation/howtos/codingconventions)

[Common C# code conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
