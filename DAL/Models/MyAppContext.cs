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

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Paymentstatus> Paymentstatuses { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgagent", "pgagent");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Eventdatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("eventdatetime");
            entity.Property(e => e.Eventstatusid).HasColumnName("eventstatusid");
            entity.Property(e => e.Manifestid).HasColumnName("manifestid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Venueid).HasColumnName("venueid");

            entity.HasOne(d => d.Eventstatus).WithMany(p => p.Events)
                .HasForeignKey(d => d.Eventstatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_status");

            entity.HasOne(d => d.Manifest).WithMany(p => p.Events)
                .HasForeignKey(d => d.Manifestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_manifest");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.Venueid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_venue");
        });

        modelBuilder.Entity<Eventstatus>(entity =>
        {
            entity.HasKey(e => e.Eventstatusid).HasName("eventstatuses_pkey");

            entity.ToTable("eventstatuses");

            entity.HasIndex(e => e.Eventstatusname, "eventstatuses_eventstatusname_key").IsUnique();

            entity.Property(e => e.Eventstatusid).HasColumnName("eventstatusid");
            entity.Property(e => e.Eventstatusname)
                .HasMaxLength(100)
                .HasColumnName("eventstatusname");
        });

        modelBuilder.Entity<Manifest>(entity =>
        {
            entity.HasKey(e => e.Manifestid).HasName("manifests_pkey");

            entity.ToTable("manifests");

            entity.Property(e => e.Manifestid).HasColumnName("manifestid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Manifesttypeid).HasColumnName("manifesttypeid");
            entity.Property(e => e.Venueid).HasColumnName("venueid");

            entity.HasOne(d => d.Manifesttype).WithMany(p => p.Manifests)
                .HasForeignKey(d => d.Manifesttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manifest_type");

            entity.HasOne(d => d.Venue).WithMany(p => p.Manifests)
                .HasForeignKey(d => d.Venueid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manifest_venue");
        });

        modelBuilder.Entity<Manifesttype>(entity =>
        {
            entity.HasKey(e => e.Manifesttypeid).HasName("manifesttypes_pkey");

            entity.ToTable("manifesttypes");

            entity.HasIndex(e => e.Manifesttypename, "manifesttypes_manifesttypename_key").IsUnique();

            entity.Property(e => e.Manifesttypeid).HasColumnName("manifesttypeid");
            entity.Property(e => e.Manifesttypename)
                .HasMaxLength(100)
                .HasColumnName("manifesttypename");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Offerid).HasName("offers_pkey");

            entity.ToTable("offers");

            entity.Property(e => e.Offerid)
                .ValueGeneratedNever()
                .HasColumnName("offerid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Offername)
                .HasMaxLength(100)
                .HasColumnName("offername");
            entity.Property(e => e.Validfrom)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("validfrom");
            entity.Property(e => e.Validto)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("validto");

            entity.HasOne(d => d.Event).WithMany(p => p.Offers)
                .HasForeignKey(d => d.Eventid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offer_event");
        });

        modelBuilder.Entity<Offerprice>(entity =>
        {
            entity.HasKey(e => e.Offerpriceid).HasName("offerprices_pkey");

            entity.ToTable("offerprices");

            entity.Property(e => e.Offerpriceid).HasColumnName("offerpriceid");
            entity.Property(e => e.Offerid).HasColumnName("offerid");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Pricelevelid).HasColumnName("pricelevelid");

            entity.HasOne(d => d.Offer).WithMany(p => p.Offerprices)
                .HasForeignKey(d => d.Offerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offerprice_offer");

            entity.HasOne(d => d.Pricelevel).WithMany(p => p.Offerprices)
                .HasForeignKey(d => d.Pricelevelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_offerprice_pricelevel");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedNever()
                .HasColumnName("paymentid");
            entity.Property(e => e.Creationdate).HasColumnName("creationdate");
            entity.Property(e => e.Lastupdated).HasColumnName("lastupdated");
            entity.Property(e => e.Paymentstatusid).HasColumnName("paymentstatusid");
            entity.Property(e => e.Purchaseid).HasColumnName("purchaseid");

            entity.HasOne(d => d.Paymentstatus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Paymentstatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_paymentstatuses");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Purchaseid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_purchases");
        });

        modelBuilder.Entity<Paymentstatus>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("paymentstatuses_pkey");

            entity.ToTable("paymentstatuses");

            entity.Property(e => e.Statusid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(100)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Pricelevel>(entity =>
        {
            entity.HasKey(e => e.Pricelevelid).HasName("pricelevels_pkey");

            entity.ToTable("pricelevels");

            entity.HasIndex(e => e.Levelname, "pricelevels_levelname_key").IsUnique();

            entity.Property(e => e.Pricelevelid).HasColumnName("pricelevelid");
            entity.Property(e => e.Levelname)
                .HasMaxLength(50)
                .HasColumnName("levelname");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Purchaseid).HasName("purchases_pkey");

            entity.ToTable("purchases");

            entity.Property(e => e.Purchaseid)
                .ValueGeneratedNever()
                .HasColumnName("purchaseid");
            entity.Property(e => e.Purchasedatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("purchasedatetime");
            entity.Property(e => e.Purchasestatusid).HasColumnName("purchasestatusid");
            entity.Property(e => e.Totalprice)
                .HasPrecision(10, 2)
                .HasColumnName("totalprice");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Purchasestatus).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Purchasestatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_status");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_user");
        });

        modelBuilder.Entity<Purchasestatus>(entity =>
        {
            entity.HasKey(e => e.Purchasestatusid).HasName("purchasestatuses_pkey");

            entity.ToTable("purchasestatuses");

            entity.HasIndex(e => e.Purchasestatusname, "purchasestatuses_purchasestatusname_key").IsUnique();

            entity.Property(e => e.Purchasestatusid).HasColumnName("purchasestatusid");
            entity.Property(e => e.Purchasestatusname)
                .HasMaxLength(50)
                .HasColumnName("purchasestatusname");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Seatid).HasName("seats_pkey");

            entity.ToTable("seats");

            entity.Property(e => e.Seatid).HasColumnName("seatid");
            entity.Property(e => e.Rownumber).HasColumnName("rownumber");
            entity.Property(e => e.Seatnumber).HasColumnName("seatnumber");
            entity.Property(e => e.Seattypeid).HasColumnName("seattypeid");
            entity.Property(e => e.Sectionid).HasColumnName("sectionid");

            entity.HasOne(d => d.Seattype).WithMany(p => p.Seats)
                .HasForeignKey(d => d.Seattypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_seat_type");

            entity.HasOne(d => d.Section).WithMany(p => p.Seats)
                .HasForeignKey(d => d.Sectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_seats_section");
        });

        modelBuilder.Entity<Seattype>(entity =>
        {
            entity.HasKey(e => e.Seattypeid).HasName("seattypes_pkey");

            entity.ToTable("seattypes");

            entity.HasIndex(e => e.Seattypename, "seattypes_seattypename_key").IsUnique();

            entity.Property(e => e.Seattypeid).HasColumnName("seattypeid");
            entity.Property(e => e.Seattypename)
                .HasMaxLength(100)
                .HasColumnName("seattypename");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("Section_pkey");

            entity.ToTable("Section");

            entity.Property(e => e.Sectionid)
                .ValueGeneratedNever()
                .HasColumnName("sectionid");
            entity.Property(e => e.Manifestid).HasColumnName("manifestid");
            entity.Property(e => e.Sectionname)
                .HasMaxLength(50)
                .HasColumnName("sectionname");

            entity.HasOne(d => d.Manifest).WithMany(p => p.Sections)
                .HasForeignKey(d => d.Manifestid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_section_manifest");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Ticketid).HasName("tickets_pkey");

            entity.ToTable("tickets");

            entity.HasIndex(e => e.Ticketcode, "tickets_ticketcode_key").IsUnique();

            entity.HasIndex(e => new { e.Eventid, e.Seatid }, "uq_event_seat").IsUnique();

            entity.Property(e => e.Ticketid).HasColumnName("ticketid");
            entity.Property(e => e.Createddatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddatetime");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Offerpriceid).HasColumnName("offerpriceid");
            entity.Property(e => e.Purchaseid).HasColumnName("purchaseid");
            entity.Property(e => e.Seatid).HasColumnName("seatid");
            entity.Property(e => e.Ticketcode)
                .HasMaxLength(100)
                .HasColumnName("ticketcode");
            entity.Property(e => e.Ticketstatusid).HasColumnName("ticketstatusid");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Eventid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_event");

            entity.HasOne(d => d.Offerprice).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Offerpriceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_offerprice");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Purchaseid)
                .HasConstraintName("fk_ticket_purchases");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Seatid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_seat");

            entity.HasOne(d => d.Ticketstatus).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Ticketstatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_status");
        });

        modelBuilder.Entity<Ticketstatus>(entity =>
        {
            entity.HasKey(e => e.Ticketstatusid).HasName("ticketstatuses_pkey");

            entity.ToTable("ticketstatuses");

            entity.HasIndex(e => e.Ticketstatusname, "ticketstatuses_ticketstatusname_key").IsUnique();

            entity.Property(e => e.Ticketstatusid).HasColumnName("ticketstatusid");
            entity.Property(e => e.Ticketstatusname)
                .HasMaxLength(50)
                .HasColumnName("ticketstatusname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });

        modelBuilder.Entity<Usersrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("usersrole_pkey");

            entity.ToTable("usersrole");

            entity.HasIndex(e => e.Rolename, "usersrole_rolename_key").IsUnique();

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Venueid).HasName("venues_pkey");

            entity.ToTable("venues");

            entity.Property(e => e.Venueid).HasColumnName("venueid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
