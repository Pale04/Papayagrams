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
    
    public partial class UserRelationship
    {
        public int id { get; set; }
        public Nullable<int> senderId { get; set; }
        public Nullable<int> receiverId { get; set; }
        public string requestState { get; set; }
        public string relationType { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
