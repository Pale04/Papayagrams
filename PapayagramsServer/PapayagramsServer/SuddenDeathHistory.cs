//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapayagramsServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class SuddenDeathHistory
    {
        public int id { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> wonGames { get; set; }
        public Nullable<int> lostGames { get; set; }
    
        public virtual User User { get; set; }
    }
}
