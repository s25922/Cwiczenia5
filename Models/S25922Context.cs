using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cw5.Models
{
    public partial class S25922Context : DbContext
    {
        public S25922Context()
        {
        }

        public S25922Context(DbContextOptions<S25922Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientTrip> ClientTrips { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("S25922Context");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient).HasName("Client_pk");

                entity.ToTable("Client", "trip");

                entity.Property(e => e.IdClient).ValueGeneratedNever();
                entity.Property(e => e.Email).HasMaxLength(120);
                entity.Property(e => e.FirstName).HasMaxLength(120);
                entity.Property(e => e.LastName).HasMaxLength(120);
                entity.Property(e => e.Pesel).HasMaxLength(120);
                entity.Property(e => e.Telephone).HasMaxLength(120);
            });

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

                entity.ToTable("Client_Trip", "trip");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Trip");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry).HasName("Country_pk");

                entity.ToTable("Country", "trip");

                entity.Property(e => e.IdCountry).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(120);

                entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                    .UsingEntity<Dictionary<string, object>>(
                        "CountryTrip",
                        r => r.HasOne<Trip>().WithMany()
                            .HasForeignKey("IdTrip")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("Country_Trip_Trip"),
                        l => l.HasOne<Country>().WithMany()
                            .HasForeignKey("IdCountry")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("Country_Trip_Country"),
                        j =>
                        {
                            j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                            j.ToTable("Country_Trip", "trip");
                        });
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

                entity.ToTable("Trip", "trip");

                entity.Property(e => e.IdTrip).ValueGeneratedNever();
                entity.Property(e => e.DateFrom).HasColumnType("datetime");
                entity.Property(e => e.DateTo).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(220);
                entity.Property(e => e.Name).HasMaxLength(120);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
