//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExampleExam.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserService()
        {
            this.CommentApplications = new HashSet<CommentApplication>();
            this.UserApplications = new HashSet<UserApplication>();
        }
    
        public int IDUser { get; set; }
        public string LoginUser { get; set; }
        public string UserPassword { get; set; }
        public Nullable<int> UserRole { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentApplication> CommentApplications { get; set; }
        public virtual RoleUser RoleUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
