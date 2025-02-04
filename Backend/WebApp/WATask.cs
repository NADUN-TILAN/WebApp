//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class WATask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WATask()
        {
            this.WATaskAssignments = new HashSet<WATaskAssignment>();
        }
    
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string Assignees { get; set; }
        public System.DateTime DueDate { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Assignor { get; set; }
        public string UploadedDocs { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WATaskAssignment> WATaskAssignments { get; set; }
    }
}
