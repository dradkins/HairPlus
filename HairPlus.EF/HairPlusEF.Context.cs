﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HairPlus.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HairPlusDBEntities : DbContext
    {
        public HairPlusDBEntities()
            : base("name=HairPlusDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerPhoto> CustomerPhotoes { get; set; }
        public virtual DbSet<CutomerHairLossSolution> CutomerHairLossSolutions { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<HairLossSolution> HairLossSolutions { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<NonSurgicalPatient> NonSurgicalPatients { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<SurgicalPatient> SurgicalPatients { get; set; }
    }
}