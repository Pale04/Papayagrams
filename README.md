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

## Propósito (david)
Éste estandar servirá como guia para la escritura homogenea, legible y segura del codigo en el proyecto de tecnologías para la construcción de software, el cual estará escrito en C# y apegandose mayormente al estandar establecido por microsoft.
El proyecto seguirá el paradigma de orientación a objetos y estará escrito en inglés.
El proyecto se trata de un juego multijugador en linea ...

---

## Reglas de nombrado

**Variables**
- Utilice nombres descriptivos para variables, métodos y clases.
- Utilice espacios de nombres (namespaces) significativos y descriptivos.
- Los nombres de las interfaces deben llevar el prefijo "I".
- Utilizar un sufijo en los nombres de los enums que haga alusión a su naturaleza, por ejemplo "Type" o "State".
- Los identificadores no deben contener dos caracteres de guión bajo ( _ ) consecutivos.
- Utilice PascalCase para los nombres de clase y los nombres de métodos.
- Utilice camelCase para los parámetros de métodos y las variables locales.
- Utilice PascalCase para los nombres de las constantes.
- Evite el uso de abreviaturas o acrónimos en los nombres, a excepción de las abreviaturas ampliamente conocidas y aceptadas.
- Prefiere la claridad a la brevedad.
- Evite usar nombres de una sola letra, excepto para contadores de bucle simples.

---

## Estilo
- 4 espacios de indentación.
- Las llaves que abren y cierran bloques de código, deben ocupar una sola línea, y empezar a escribir código en la siguiente línea.

***Ejemplo***
```C#
private Foo()
{
   if (Bar==null)
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

- Las clases, structs y enums públicos deben contenerse en su propio archivo

---

### Estructuras de control
- Debe haber un espacio entre la declaración de la estructura de control y el paréntesis.

***Ejemplo***
```C#
if (Bar == null)
while (true)
```
***Así no:***
```C#
if(Bar == null)
while  (true)
```

---

## Manejo de excepciones (david)
Las excepciones se manejarán con bloques *try-catch,* y se usará *using* cuando sea posible y más legible que un bloque try-catch.
No se usará en bloques try-catch el tipo de excepción más alto (Exception), sino errores especificos.
Se usarán filtros de excepciones en vez de bloques _if_ cuando sean necesarios.

### Bitacora de errores
Se usará la librería _log4net_ para la bitacora de errores.
Los errores se registrarán a la bitacora en ..., y si es necesario, serán propagados a las capas superiores para informar al usuario del error.

**Fatal**:
- FileNotFoundException
- SqlException

**Warning**:
- 

**Info**:
-

---

## Prácticas seguras de construcción (pale)

1. Utilizar una cuenta solo con los permisos esenciales para acceder a la base de datos.
2. Liberar los recursos después de utilzarlos.
3. Validación de parámetros.
4. Validación de entrada de datos del usuario

---

## Referencias
[C# identifier naming rules and conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)

[C# Coding Style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)

[Coding guidelines](https://learn.microsoft.com/en-us/mixed-reality/world-locking-tools/documentation/howtos/codingconventions)

[Common C# code conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
