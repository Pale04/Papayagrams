﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class papayagramsEntities : DbContext
    {
        public papayagramsEntities()
            : base("name=papayagramsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Achievement> Achievement { get; set; }
        public virtual DbSet<OriginalGameHistory> OriginalGameHistory { get; set; }
        public virtual DbSet<SuddenDeathHistory> SuddenDeathHistory { get; set; }
        public virtual DbSet<TimeAtackHistory> TimeAtackHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAchieved> UserAchieved { get; set; }
        public virtual DbSet<UserConfiguration> UserConfiguration { get; set; }
        public virtual DbSet<UserRelationship> UserRelationship { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }
    
        public virtual int register_user(string username, string email, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("register_user", usernameParameter, emailParameter, passwordParameter);
        }
    
        public virtual ObjectResult<login_Result> login(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<login_Result>("login", usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<get_player_Result> get_player(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_player_Result>("get_player", usernameParameter);
        }
    
        public virtual ObjectResult<get_player_by_email_Result> get_player_by_email(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_player_by_email_Result>("get_player_by_email", emailParameter);
        }
    
        public virtual ObjectResult<get_player_by_username_Result> get_player_by_username(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_player_by_username_Result>("get_player_by_username", usernameParameter);
        }
    
        public virtual int log_in(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("log_in", usernameParameter, passwordParameter);
        }
    
        public virtual int update_user_status(string username, string status, Nullable<System.DateTime> date)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("update_user_status", usernameParameter, statusParameter, dateParameter);
        }
    
        public virtual int log_out(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("log_out", usernameParameter);
        }
    
        public virtual int send_friend_request(string senderUsername, string receiverUsername)
        {
            var senderUsernameParameter = senderUsername != null ?
                new ObjectParameter("senderUsername", senderUsername) :
                new ObjectParameter("senderUsername", typeof(string));
    
            var receiverUsernameParameter = receiverUsername != null ?
                new ObjectParameter("receiverUsername", receiverUsername) :
                new ObjectParameter("receiverUsername", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("send_friend_request", senderUsernameParameter, receiverUsernameParameter);
        }
    }
}
