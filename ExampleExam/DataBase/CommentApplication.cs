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
    
    public partial class CommentApplication
    {
        public int IDComment { get; set; }
        public string Comment { get; set; }
        public Nullable<int> Specialist { get; set; }
        public Nullable<int> AppComment { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual UserService UserService { get; set; }
    }
}
