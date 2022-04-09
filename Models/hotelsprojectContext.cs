using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HotelBooking.Models
{
    public partial class hotelsprojectContext : DbContext
    {
       

        public hotelsprojectContext(DbContextOptions<hotelsprojectContext> options)
            : base(options)
        { }
        

        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=hotelsproject;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photos");

                entity.Property(e => e.Photoid).HasColumnName("photoid");

                entity.Property(e => e.Photo1).HasColumnName("photo");

                entity.Property(e => e.Roomid).HasColumnName("roomid");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.Roomid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_photos_rooms");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Reserveid);

                entity.ToTable("reservations");

                entity.Property(e => e.Reserveid).HasColumnName("reserveid");

                entity.Property(e => e.Clientid).HasColumnName("clientid");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Roomid).HasColumnName("roomid");

                entity.Property(e => e.Startdate)
    .HasMaxLength(50)
    .HasColumnName("startdate");

                entity.Property(e => e.Enddate)
    .HasMaxLength(50)
    .HasColumnName("enddate");

                entity.Property(e => e.Totalcost).HasColumnName("totalcost");
                entity.Property(e => e.Number).HasColumnName("number");


                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Clientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reservations_users");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Roomid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reservations_rooms");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");

                entity.Property(e => e.Reviewid).HasColumnName("reviewid");

                entity.Property(e => e.Clientid).HasColumnName("clientid");

                entity.Property(e => e.Hotelid).HasColumnName("hotelid");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Ishappy)
                    .HasMaxLength(50)
                    .HasColumnName("ishappy");

                entity.Property(e => e.Review1)
                    .IsRequired()
                    .HasColumnName("review");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ReviewClients)
                    .HasForeignKey(d => d.Clientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_users1");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.ReviewHotels)
                    .HasForeignKey(d => d.Hotelid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_users");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("rooms");

                entity.Property(e => e.Roomid).HasColumnName("roomid");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Hotelid).HasColumnName("hotelid");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Isreserved).HasColumnName("isreserved");

                entity.Property(e => e.Time)
                    .HasMaxLength(50)
                    .HasColumnName("time");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");


                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.Hotelid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_rooms_users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("role");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Lng).HasColumnName("lng");
                entity.Property(e => e.Lat).HasColumnName("lat");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
