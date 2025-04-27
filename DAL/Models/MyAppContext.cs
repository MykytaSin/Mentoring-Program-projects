using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class MyAppContext : DbContext
{
    public MyAppContext()
    {
    }

    public MyAppContext(DbContextOptions<MyAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Eventstatus> Eventstatuses { get; set; }

    public virtual DbSet<Manifest> Manifests { get; set; }

    public virtual DbSet<Manifesttype> Manifesttypes { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<Offerprice> Offerprices { get; set; }

    public virtual DbSet<Pricelevel> Pricelevels { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Purchasestatus> Purchasestatuses { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Seattype> Seattypes { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticketstatus> Ticketstatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usersrole> Usersroles { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345;Include Error Detail=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgagent", "pgagent");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("events_pkey");

            entity.HasOne(d => d.Eventstatus).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_status");

            entity.HasOne(d => d.Manifest).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_manifest");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_venue");
        });

        modelBuilder.Entity<Eventstatus>(entity =>
        {
            entity.HasKey(e => e.Eventstatusid).HasName("eventstatuses_pkey");
        });

        modelBuilder.Entity<Manifest>(entity =>
        {
            entity.HasKey(e => e.Manifestid).HasName("manifests_pkey");

            entity.HasOne(d => d.Manifesttype).WithMany(p => p.Manifests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manifest_type");

            entity.HasOne(d => d.Venue).WithMany(p => p.Manifests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manifest_venue");
        });

        modelBuilder.Entity<Manifesttype>(entity =>
        {
            entity.HasKey(e => e.Manifesttypeid).HasName("manifesttypes_pkey");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Offerid).HasName("offers_pkey");

            entity.HasOne(d => d.Event).WithMany(p => p.Offers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offer_event");
        });

        modelBuilder.Entity<Offerprice>(entity =>
        {
            entity.HasKey(e => e.Offerpriceid).HasName("offerprices_pkey");

            entity.HasOne(d => d.Offer).WithMany(p => p.Offerprices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offerprice_offer");

            entity.HasOne(d => d.Pricelevel).WithMany(p => p.Offerprices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offerprice_pricelevel");
        });

        modelBuilder.Entity<Pricelevel>(entity =>
        {
            entity.HasKey(e => e.Pricelevelid).HasName("pricelevels_pkey");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Purchaseid).HasName("purchases_pkey");

            entity.Property(e => e.Purchasedatetime).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Purchasestatus).WithMany(p => p.Purchases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_status");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_user");
        });

        modelBuilder.Entity<Purchasestatus>(entity =>
        {
            entity.HasKey(e => e.Purchasestatusid).HasName("purchasestatuses_pkey");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Seatid).HasName("seats_pkey");

            entity.HasOne(d => d.Seattype).WithMany(p => p.Seats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_seat_type");

            entity.HasOne(d => d.Section).WithMany(p => p.Seats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_seats_section");
        });

        modelBuilder.Entity<Seattype>(entity =>
        {
            entity.HasKey(e => e.Seattypeid).HasName("seattypes_pkey");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("Section_pkey");

            entity.Property(e => e.Sectionid).ValueGeneratedNever();

            entity.HasOne(d => d.Manifest).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_section_manifest");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Ticketid).HasName("tickets_pkey");

            entity.Property(e => e.Createddatetime).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_event");

            entity.HasOne(d => d.Offerprice).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_offerprice");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Tickets).HasConstraintName("fk_ticket_purchase");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_seat");

            entity.HasOne(d => d.Ticketstatus).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_status");
        });

        modelBuilder.Entity<Ticketstatus>(entity =>
        {
            entity.HasKey(e => e.Ticketstatusid).HasName("ticketstatuses_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });

        modelBuilder.Entity<Usersrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("usersrole_pkey");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Venueid).HasName("venues_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
