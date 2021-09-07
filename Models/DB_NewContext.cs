using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SIP.Models
{
    public partial class DB_NewContext : DbContext
    {
        public DB_NewContext()
        {
        }

        public DB_NewContext(DbContextOptions<DB_NewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Coa> Coa { get; set; }
        public virtual DbSet<DataBadan> DataBadan { get; set; }
        public virtual DbSet<DataIzin> DataIzin { get; set; }
        public virtual DbSet<DataSubjek> DataSubjek { get; set; }
        public virtual DbSet<DataUsaha> DataUsaha { get; set; }
        public virtual DbSet<Fiskal> Fiskal { get; set; }
        public virtual DbSet<IndKabKota> IndKabKota { get; set; }
        public virtual DbSet<IndKecamatan> IndKecamatan { get; set; }
        public virtual DbSet<IndKelurahan> IndKelurahan { get; set; }
        public virtual DbSet<IndProvinsi> IndProvinsi { get; set; }
        public virtual DbSet<Lra> Lra { get; set; }
        public virtual DbSet<Lspop> Lspop { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Ml> Ml { get; set; }
        public virtual DbSet<Nsr> Nsr { get; set; }
        public virtual DbSet<NsrLain> NsrLain { get; set; }
        public virtual DbSet<NsrLed> NsrLed { get; set; }
        public virtual DbSet<NsrNjop> NsrNjop { get; set; }
        public virtual DbSet<PAirTanah> PAirTanah { get; set; }
        public virtual DbSet<PAirTanahDt> PAirTanahDt { get; set; }
        public virtual DbSet<PBphtb> PBphtb { get; set; }
        public virtual DbSet<PHiburan> PHiburan { get; set; }
        public virtual DbSet<PHiburanDt> PHiburanDt { get; set; }
        public virtual DbSet<PHotel> PHotel { get; set; }
        public virtual DbSet<PHotelDt> PHotelDt { get; set; }
        public virtual DbSet<PHotelKm> PHotelKm { get; set; }
        public virtual DbSet<PMineral> PMineral { get; set; }
        public virtual DbSet<PParkir> PParkir { get; set; }
        public virtual DbSet<PPenerangan> PPenerangan { get; set; }
        public virtual DbSet<PReklame> PReklame { get; set; }
        public virtual DbSet<PRestoran> PRestoran { get; set; }
        public virtual DbSet<PRestoranDt> PRestoranDt { get; set; }
        public virtual DbSet<PWalet> PWalet { get; set; }
        public virtual DbSet<Pegawai> Pegawai { get; set; }
        public virtual DbSet<Pemda> Pemda { get; set; }
        public virtual DbSet<PrintSetting> PrintSetting { get; set; }
        public virtual DbSet<RefBadanHukum> RefBadanHukum { get; set; }
        public virtual DbSet<RefBank> RefBank { get; set; }
        public virtual DbSet<RefBobotHda> RefBobotHda { get; set; }
        public virtual DbSet<RefBobotKp> RefBobotKp { get; set; }
        public virtual DbSet<RefBobotKpBatas> RefBobotKpBatas { get; set; }
        public virtual DbSet<RefBobotSda> RefBobotSda { get; set; }
        public virtual DbSet<RefDokumen> RefDokumen { get; set; }
        public virtual DbSet<RefHargaKwh> RefHargaKwh { get; set; }
        public virtual DbSet<RefHargaPasar> RefHargaPasar { get; set; }
        public virtual DbSet<RefHotel> RefHotel { get; set; }
        public virtual DbSet<RefJabatan> RefJabatan { get; set; }
        public virtual DbSet<RefJenisBangunan> RefJenisBangunan { get; set; }
        public virtual DbSet<RefJenisBank> RefJenisBank { get; set; }
        public virtual DbSet<RefJenisSetoran> RefJenisSetoran { get; set; }
        public virtual DbSet<RefNjoptkp> RefNjoptkp { get; set; }
        public virtual DbSet<RefPekerjaan> RefPekerjaan { get; set; }
        public virtual DbSet<RefPerolehan> RefPerolehan { get; set; }
        public virtual DbSet<RefReklame> RefReklame { get; set; }
        public virtual DbSet<RefRestoran> RefRestoran { get; set; }
        public virtual DbSet<RefRetribusi> RefRetribusi { get; set; }
        public virtual DbSet<RefSuratIzin> RefSuratIzin { get; set; }
        public virtual DbSet<RefUsaha> RefUsaha { get; set; }
        public virtual DbSet<SaldoAnggaran> SaldoAnggaran { get; set; }
        public virtual DbSet<Skpd> Skpd { get; set; }
        public virtual DbSet<Skpdkb> Skpdkb { get; set; }
        public virtual DbSet<Skpdkbt> Skpdkbt { get; set; }
        public virtual DbSet<Skpdn> Skpdn { get; set; }
        public virtual DbSet<Skrd> Skrd { get; set; }
        public virtual DbSet<SkrdDt> SkrdDt { get; set; }
        public virtual DbSet<Spop> Spop { get; set; }
        public virtual DbSet<Sppt> Sppt { get; set; }
        public virtual DbSet<Sptpd> Sptpd { get; set; }
        public virtual DbSet<Sspd> Sspd { get; set; }
        public virtual DbSet<Ssrd> Ssrd { get; set; }
        public virtual DbSet<Ssrdr> Ssrdr { get; set; }
        public virtual DbSet<SsrdrDt> SsrdrDt { get; set; }
        public virtual DbSet<Stpd> Stpd { get; set; }
        public virtual DbSet<Sts> Sts { get; set; }
        public virtual DbSet<StsDt> StsDt { get; set; }
        public virtual DbSet<Stts> Stts { get; set; }
        public virtual DbSet<SuratTeguran> SuratTeguran { get; set; }
        public virtual DbSet<THistory> THistory { get; set; }
        public virtual DbSet<TandaTangan> TandaTangan { get; set; }
        public virtual DbSet<TarifPajak> TarifPajak { get; set; }
        public virtual DbSet<TarifRetribusi> TarifRetribusi { get; set; }
        public virtual DbSet<TransaksiLra> TransaksiLra { get; set; }
        public virtual DbSet<UserSetting> UserSetting { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseNpgsql("Server=localhost;Database=dbsip_dogiyai;Username=postgres;Password=postgres;Port=5432");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(e => e.IdRef);

                entity.HasIndex(e => e.IdSetoran);

                entity.Property(e => e.IdBank).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdRefNavigation)
                    .WithMany(p => p.Bank)
                    .HasForeignKey(d => d.IdRef)
                    .HasConstraintName("FK_Bank_Ref_Bank");

                entity.HasOne(d => d.IdSetoranNavigation)
                    .WithMany(p => p.Bank)
                    .HasForeignKey(d => d.IdSetoran)
                    .HasConstraintName("FK_Bank_Ref_JenisSetoran");
            });

            modelBuilder.Entity<DataBadan>(entity =>
            {
                entity.HasIndex(e => e.IdSubjek);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.HasIndex(e => e.IndProvinsiId);

                entity.Property(e => e.IdBadan).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.PekerjaanLain).HasDefaultValueSql("'-'::character varying");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.DataBadan)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_Data_Badan_Data_Subjek");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.DataBadan)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_Data_Badan_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.DataBadan)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_Data_Badan_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.DataBadan)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_Data_Badan_IndKelurahan");

                entity.HasOne(d => d.IndProvinsi)
                    .WithMany(p => p.DataBadan)
                    .HasForeignKey(d => d.IndProvinsiId)
                    .HasConstraintName("FK_Data_Badan_IndProvinsi");
            });

            modelBuilder.Entity<DataIzin>(entity =>
            {
                entity.HasIndex(e => e.IdSuratIzin);

                entity.HasIndex(e => e.IdUsaha);

                entity.Property(e => e.IdIzin).ValueGeneratedNever();

                entity.HasOne(d => d.IdSuratIzinNavigation)
                    .WithMany(p => p.DataIzin)
                    .HasForeignKey(d => d.IdSuratIzin)
                    .HasConstraintName("FK_Data_Izin_Ref_SuratIzin");

                entity.HasOne(d => d.IdUsahaNavigation)
                    .WithMany(p => p.DataIzin)
                    .HasForeignKey(d => d.IdUsaha)
                    .HasConstraintName("FK_Data_Izin_Data_Usaha");
            });

            modelBuilder.Entity<DataSubjek>(entity =>
            {
                entity.HasIndex(e => e.IdBadanHukum);

                entity.HasIndex(e => e.IdPekerjaan);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.HasIndex(e => e.IndProvinsiId);

                entity.Property(e => e.IdSubjek).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.NamaInstansi).HasDefaultValueSql("'-'::character varying");

                entity.Property(e => e.PekerjaanLain).HasDefaultValueSql("'-'::character varying");

                entity.HasOne(d => d.IdBadanHukumNavigation)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IdBadanHukum)
                    .HasConstraintName("FK_Data_Subjek_Ref_BadanHukum");

                entity.HasOne(d => d.IdPekerjaanNavigation)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IdPekerjaan)
                    .HasConstraintName("FK_Data_Subjek_Ref_Pekerjaan");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_Data_Subjek_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_Data_Subjek_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_Data_Subjek_IndKelurahan");

                entity.HasOne(d => d.IndProvinsi)
                    .WithMany(p => p.DataSubjek)
                    .HasForeignKey(d => d.IndProvinsiId)
                    .HasConstraintName("FK_Data_Subjek_IndProvinsi");
            });

            modelBuilder.Entity<DataUsaha>(entity =>
            {
                entity.HasIndex(e => e.IdJenis);

                entity.HasIndex(e => e.IdSubjek);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.HasIndex(e => e.IndProvinsiId);

                entity.Property(e => e.IdUsaha).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdJenisNavigation)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IdJenis)
                    .HasConstraintName("FK_Data_Usaha_Ref_Usaha");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_Data_Usaha_Data_Subjek");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_Data_Usaha_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_Data_Usaha_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_Data_Usaha_IndKelurahan");

                entity.HasOne(d => d.IndProvinsi)
                    .WithMany(p => p.DataUsaha)
                    .HasForeignKey(d => d.IndProvinsiId)
                    .HasConstraintName("FK_Data_Usaha_IndProvinsi");
            });

            modelBuilder.Entity<Fiskal>(entity =>
            {
                entity.HasIndex(e => e.IdSubjek);

                entity.Property(e => e.IdFiskal).ValueGeneratedNever();

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Fiskal)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_Fiskal_Data_Subjek");
            });

            modelBuilder.Entity<IndKabKota>(entity =>
            {
                entity.HasIndex(e => e.IndProvinsiId);

                entity.Property(e => e.IndKabKotaId).ValueGeneratedNever();

                entity.HasOne(d => d.IndProvinsi)
                    .WithMany(p => p.IndKabKota)
                    .HasForeignKey(d => d.IndProvinsiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IndKabKota_IndProvinsi");
            });

            modelBuilder.Entity<IndKecamatan>(entity =>
            {
                entity.HasIndex(e => e.IndKabKotaId);

                entity.Property(e => e.IndKecamatanId).ValueGeneratedNever();

                entity.Property(e => e.NoUrutKecamatan).IsFixedLength();

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.IndKecamatan)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_IndKecamatan_IndKabKota");
            });

            modelBuilder.Entity<IndKelurahan>(entity =>
            {
                entity.HasIndex(e => e.IndKecamatanId);

                entity.Property(e => e.IndKelurahanId).ValueGeneratedNever();

                entity.Property(e => e.NoUrutKelurahan).IsFixedLength();

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.IndKelurahan)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IndKelurahan_IndKecamatan");
            });

            modelBuilder.Entity<IndProvinsi>(entity =>
            {
                entity.Property(e => e.IndProvinsiId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Lra>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.Property(e => e.IdLra).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Lra)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_LRA_Coa");
            });

            modelBuilder.Entity<Lspop>(entity =>
            {
                entity.HasIndex(e => e.IdJenis);

                entity.HasIndex(e => e.IdSpop);

                entity.Property(e => e.IdLspop).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdJenisNavigation)
                    .WithMany(p => p.Lspop)
                    .HasForeignKey(d => d.IdJenis)
                    .HasConstraintName("FK_LSPOP_Ref_JenisBangunan");

                entity.HasOne(d => d.IdSpopNavigation)
                    .WithMany(p => p.Lspop)
                    .HasForeignKey(d => d.IdSpop)
                    .HasConstraintName("FK_LSPOP_SPOP");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Nsr>(entity =>
            {
                entity.Property(e => e.SatuanUkuran).IsFixedLength();
            });

            modelBuilder.Entity<NsrLain>(entity =>
            {
                entity.Property(e => e.SatuanUkuran).IsFixedLength();
            });

            modelBuilder.Entity<NsrNjop>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.Property(e => e.IdNsrNjop).ValueGeneratedNever();

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.NsrNjop)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_NSR_NJOP_Coa");
            });

            modelBuilder.Entity<PAirTanah>(entity =>
            {
                entity.HasIndex(e => e.IdDetail);

                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdAirTanah).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Dpp).HasDefaultValueSql("0");

                entity.Property(e => e.PajakTerhutang).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdDetailNavigation)
                    .WithMany(p => p.PAirTanah)
                    .HasForeignKey(d => d.IdDetail)
                    .HasConstraintName("FK_P_AirTanah_P_AirTanah_Dt");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PAirTanah)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_AirTanah_SPTPD");
            });

            modelBuilder.Entity<PAirTanahDt>(entity =>
            {
                entity.HasIndex(e => e.IdKel);

                entity.HasIndex(e => e.IdSumber);

                entity.HasIndex(e => e.IdUsaha);

                entity.Property(e => e.IdDetail).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdKelNavigation)
                    .WithMany(p => p.PAirTanahDt)
                    .HasForeignKey(d => d.IdKel)
                    .HasConstraintName("FK_P_AirTanah_Dt_Ref_BobotKP");

                entity.HasOne(d => d.IdSumberNavigation)
                    .WithMany(p => p.PAirTanahDt)
                    .HasForeignKey(d => d.IdSumber)
                    .HasConstraintName("FK_P_AirTanah_Dt_Ref_BobotSDA");

                entity.HasOne(d => d.IdUsahaNavigation)
                    .WithMany(p => p.PAirTanahDt)
                    .HasForeignKey(d => d.IdUsaha)
                    .HasConstraintName("FK_P_AirTanah_Dt_Data_Usaha");
            });

            modelBuilder.Entity<PBphtb>(entity =>
            {
                entity.HasIndex(e => e.IdPerolehan);

                entity.HasIndex(e => e.IdTarif);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.Property(e => e.IdBphtb).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdPerolehanNavigation)
                    .WithMany(p => p.PBphtb)
                    .HasForeignKey(d => d.IdPerolehan)
                    .HasConstraintName("FK_P_BPHTB_Ref_Perolehan");

                entity.HasOne(d => d.IdTarifNavigation)
                    .WithMany(p => p.PBphtb)
                    .HasForeignKey(d => d.IdTarif)
                    .HasConstraintName("FK_P_BPHTB_Tarif_Pajak");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.PBphtb)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_P_BPHTB_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.PBphtb)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_P_BPHTB_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.PBphtb)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_P_BPHTB_IndKelurahan");
            });

            modelBuilder.Entity<PHiburan>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdHiburan).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Dpp).HasDefaultValueSql("0");

                entity.Property(e => e.JkartuBebas).HasDefaultValueSql("0");

                entity.Property(e => e.Jmesin).HasDefaultValueSql("0");

                entity.Property(e => e.JpengunjungHb).HasDefaultValueSql("0");

                entity.Property(e => e.JpengunjungHl).HasDefaultValueSql("0");

                entity.Property(e => e.JpertunjukanHb).HasDefaultValueSql("0");

                entity.Property(e => e.JpertunjukanHl).HasDefaultValueSql("0");

                entity.Property(e => e.Jruangan).HasDefaultValueSql("0");

                entity.Property(e => e.PajakTerhutang).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PHiburan)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Hiburan_SPTPD");
            });

            modelBuilder.Entity<PHiburanDt>(entity =>
            {
                entity.HasIndex(e => e.IdHiburan);

                entity.Property(e => e.IdHiburanDt).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdHiburanNavigation)
                    .WithMany(p => p.PHiburanDt)
                    .HasForeignKey(d => d.IdHiburan)
                    .HasConstraintName("FK_P_Hiburan_Dt_P_Hiburan");
            });

            modelBuilder.Entity<PHotel>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdHotel).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Dpp).HasDefaultValueSql("0");

                entity.Property(e => e.PajakTerhutang).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PHotel)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Hotel_P_Hotel");
            });

            modelBuilder.Entity<PHotelDt>(entity =>
            {
                entity.HasIndex(e => e.IdHotel);

                entity.HasIndex(e => e.IdRef);

                entity.Property(e => e.IdHotelDt).ValueGeneratedNever();

                entity.HasOne(d => d.IdHotelNavigation)
                    .WithMany(p => p.PHotelDt)
                    .HasForeignKey(d => d.IdHotel)
                    .HasConstraintName("FK_P_Hotel_Dt_P_Hotel");

                entity.HasOne(d => d.IdRefNavigation)
                    .WithMany(p => p.PHotelDt)
                    .HasForeignKey(d => d.IdRef)
                    .HasConstraintName("FK_P_Hotel_Dt_Ref_Hotel");
            });

            modelBuilder.Entity<PHotelKm>(entity =>
            {
                entity.HasIndex(e => e.IdHotel);

                entity.Property(e => e.IdHotelKm).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdHotelNavigation)
                    .WithMany(p => p.PHotelKm)
                    .HasForeignKey(d => d.IdHotel)
                    .HasConstraintName("FK_P_Hotel_Km_P_Hotel");
            });

            modelBuilder.Entity<PMineral>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdMineral).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PMineral)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Mineral_SPTPD");
            });

            modelBuilder.Entity<PParkir>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdParkir).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PParkir)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Parkir_SPTPD");
            });

            modelBuilder.Entity<PPenerangan>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdPenerangan).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PPenerangan)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Penerangan_SPTPD");
            });

            modelBuilder.Entity<PReklame>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.Property(e => e.IdReklame).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Dpp).HasDefaultValueSql("0");

                entity.Property(e => e.Jumlah).HasDefaultValueSql("0");

                entity.Property(e => e.JumlahDetik).HasDefaultValueSql("0");

                entity.Property(e => e.JumlahHari).HasDefaultValueSql("0");

                entity.Property(e => e.L1).HasDefaultValueSql("0");

                entity.Property(e => e.L2).HasDefaultValueSql("0");

                entity.Property(e => e.L3).HasDefaultValueSql("0");

                entity.Property(e => e.L4).HasDefaultValueSql("0");

                entity.Property(e => e.Luas).HasDefaultValueSql("0");

                entity.Property(e => e.NilaiKontrak).HasDefaultValueSql("0");

                entity.Property(e => e.Nsr).HasDefaultValueSql("0");

                entity.Property(e => e.P1).HasDefaultValueSql("0");

                entity.Property(e => e.P2).HasDefaultValueSql("0");

                entity.Property(e => e.P3).HasDefaultValueSql("0");

                entity.Property(e => e.P4).HasDefaultValueSql("0");

                entity.Property(e => e.PajakTerhutang).HasDefaultValueSql("0");

                entity.Property(e => e.Tinggi).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PReklame)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Reklame_SPTPD");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.PReklame)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_P_Reklame_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.PReklame)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_P_Reklame_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.PReklame)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_P_Reklame_IndKelurahan");
            });

            modelBuilder.Entity<PRestoran>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdRestoran).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PRestoran)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Restoran_P_Restoran");
            });

            modelBuilder.Entity<PRestoranDt>(entity =>
            {
                entity.HasIndex(e => e.IdRef);

                entity.HasIndex(e => e.IdRestoran);

                entity.Property(e => e.IdRestoranDt).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IdRestoran).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdRefNavigation)
                    .WithMany(p => p.PRestoranDt)
                    .HasForeignKey(d => d.IdRef)
                    .HasConstraintName("FK_P_Restoran_Dt_Ref_Restoran");

                entity.HasOne(d => d.IdRestoranNavigation)
                    .WithMany(p => p.PRestoranDt)
                    .HasForeignKey(d => d.IdRestoran)
                    .HasConstraintName("FK_P_Restoran_Dt_P_Restoran");
            });

            modelBuilder.Entity<PWalet>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdWalet).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.PWalet)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_P_Walet_SPTPD");
            });

            modelBuilder.Entity<Pegawai>(entity =>
            {
                entity.HasIndex(e => e.IdJabatan);

                entity.Property(e => e.IdPegawai).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdJabatanNavigation)
                    .WithMany(p => p.Pegawai)
                    .HasForeignKey(d => d.IdJabatan)
                    .HasConstraintName("FK_Pegawai_Ref_Jabatan");
            });

            modelBuilder.Entity<Pemda>(entity =>
            {
                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.HasIndex(e => e.IndProvinsiId);

                entity.Property(e => e.IdPemda).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.NoUrutKecamatan).IsFixedLength();

                entity.Property(e => e.NoUrutKelurahan).IsFixedLength();

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.Pemda)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_Pemda_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.Pemda)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_Pemda_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.Pemda)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_Pemda_IndKelurahan");

                entity.HasOne(d => d.IndProvinsi)
                    .WithMany(p => p.Pemda)
                    .HasForeignKey(d => d.IndProvinsiId)
                    .HasConstraintName("FK_Pemda_Pemda");
            });

            modelBuilder.Entity<PrintSetting>(entity =>
            {
                entity.HasKey(e => e.IdSetting)
                    .HasName("PrintSetting_pkey");

                entity.HasIndex(e => e.IdDok)
                    .HasName("fki_FK_PrintSetting_Ref_Dokumen");

                entity.Property(e => e.IdSetting).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdDokNavigation)
                    .WithMany(p => p.PrintSetting)
                    .HasForeignKey(d => d.IdDok)
                    .HasConstraintName("FK_PrintSetting_Ref_Dokumen");
            });

            modelBuilder.Entity<RefBank>(entity =>
            {
                entity.Property(e => e.IdBank).ValueGeneratedNever();
            });

            modelBuilder.Entity<RefHargaPasar>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.RefHargaPasar)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_Ref_HargaPasar_Coa");
            });

            modelBuilder.Entity<RefJenisBank>(entity =>
            {
                entity.Property(e => e.IdJenis).ValueGeneratedNever();
            });

            modelBuilder.Entity<RefRetribusi>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.RefRetribusi)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_Ref_Retribusi_Coa");
            });

            modelBuilder.Entity<SaldoAnggaran>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Skpd>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdSkpd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Bunga).HasDefaultValueSql("0");

                entity.Property(e => e.IdSptpd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Kenaikan).HasDefaultValueSql("0");

                entity.Property(e => e.KreditPajak).HasDefaultValueSql("0");

                entity.Property(e => e.Terhutang).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.Skpd)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_SKPD_SKPD");
            });

            modelBuilder.Entity<Skpdkb>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdSkpdkb).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.KompKelebihan).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.Skpdkb)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_SKPDKB_SPTPD");
            });

            modelBuilder.Entity<Skpdkbt>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdSkpdkbt).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.KompKelebihan).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.Skpdkbt)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_SKPDKBT_SPTPD");
            });

            modelBuilder.Entity<Skpdn>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSspd);

                entity.Property(e => e.IdSkpdn).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.KompKelebihan).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Skpdn)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SKPDN_SKPDN");

                entity.HasOne(d => d.IdSspdNavigation)
                    .WithMany(p => p.Skpdn)
                    .HasForeignKey(d => d.IdSspd)
                    .HasConstraintName("FK_SKPDN_SSPD");
            });

            modelBuilder.Entity<Skrd>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSubjek);

                entity.Property(e => e.IdSkrd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Bunga).HasDefaultValueSql("0");

                entity.Property(e => e.Kenaikan).HasDefaultValueSql("0");

                entity.Property(e => e.Kredit).HasDefaultValueSql("0");

                entity.Property(e => e.Tahun).IsFixedLength();

                entity.Property(e => e.Terhutang).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Skrd)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SKRD_SKRD");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Skrd)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_SKRD_Data_Subjek");
            });

            modelBuilder.Entity<SkrdDt>(entity =>
            {
                entity.HasIndex(e => e.IdSkrd);

                entity.HasIndex(e => e.IdTarif);

                entity.Property(e => e.IdSkrdDt).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSkrdNavigation)
                    .WithMany(p => p.SkrdDt)
                    .HasForeignKey(d => d.IdSkrd)
                    .HasConstraintName("FK_SKRD_Dt_SKRD");

                entity.HasOne(d => d.IdTarifNavigation)
                    .WithMany(p => p.SkrdDt)
                    .HasForeignKey(d => d.IdTarif)
                    .HasConstraintName("FK_SKRD_Dt_Tarif_Retribusi");
            });

            modelBuilder.Entity<Spop>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSubjek);

                entity.HasIndex(e => e.IndKabKotaId);

                entity.HasIndex(e => e.IndKecamatanId);

                entity.HasIndex(e => e.IndKelurahanId);

                entity.Property(e => e.IdSpop).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Spop)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SPOP_Coa");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Spop)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_SPOP_Data_Subjek");

                entity.HasOne(d => d.IndKabKota)
                    .WithMany(p => p.Spop)
                    .HasForeignKey(d => d.IndKabKotaId)
                    .HasConstraintName("FK_SPOP_IndKabKota");

                entity.HasOne(d => d.IndKecamatan)
                    .WithMany(p => p.Spop)
                    .HasForeignKey(d => d.IndKecamatanId)
                    .HasConstraintName("FK_SPOP_IndKecamatan");

                entity.HasOne(d => d.IndKelurahan)
                    .WithMany(p => p.Spop)
                    .HasForeignKey(d => d.IndKelurahanId)
                    .HasConstraintName("FK_SPOP_IndKelurahan");
            });

            modelBuilder.Entity<Sppt>(entity =>
            {
                entity.HasIndex(e => e.IdSpop);

                entity.HasIndex(e => e.IdTarif);

                entity.Property(e => e.IdSppt).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSpopNavigation)
                    .WithMany(p => p.Sppt)
                    .HasForeignKey(d => d.IdSpop)
                    .HasConstraintName("FK_SPPT_SPOP");

                entity.HasOne(d => d.IdTarifNavigation)
                    .WithMany(p => p.Sppt)
                    .HasForeignKey(d => d.IdTarif)
                    .HasConstraintName("FK_SPPT_Tarif_Pajak");
            });

            modelBuilder.Entity<Sptpd>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSubjek);

                entity.HasIndex(e => e.IdTarif);

                entity.HasIndex(e => e.IdUsaha);

                entity.Property(e => e.IdSptpd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.KreditPajak).HasDefaultValueSql("0");

                entity.Property(e => e.Tahun).IsFixedLength();

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Sptpd)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SPTPD_Coa");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Sptpd)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_SPTPD_Data_Subjek");

                entity.HasOne(d => d.IdTarifNavigation)
                    .WithMany(p => p.Sptpd)
                    .HasForeignKey(d => d.IdTarif)
                    .HasConstraintName("FK_SPTPD_Tarif_Pajak");

                entity.HasOne(d => d.IdUsahaNavigation)
                    .WithMany(p => p.Sptpd)
                    .HasForeignKey(d => d.IdUsaha)
                    .HasConstraintName("FK_SPTPD_Data_Usaha");
            });

            modelBuilder.Entity<Sspd>(entity =>
            {
                entity.HasIndex(e => e.IdBank);

                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSetoran);

                entity.HasIndex(e => e.IdSkpd);

                entity.HasIndex(e => e.IdSkpdkb);

                entity.HasIndex(e => e.IdSkpdkbt);

                entity.HasIndex(e => e.IdSptpd);

                entity.HasIndex(e => e.IdStpd);

                entity.HasIndex(e => e.IdSubjek);

                entity.HasIndex(e => e.IdUsaha);

                entity.Property(e => e.IdSspd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Tahun).IsFixedLength();

                entity.HasOne(d => d.IdBankNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdBank)
                    .HasConstraintName("FK_SSPD_Bank");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SSPD_Coa");

                entity.HasOne(d => d.IdSetoranNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSetoran)
                    .HasConstraintName("FK_SSPD_Ref_JenisSetoran");

                entity.HasOne(d => d.IdSkpdNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSkpd)
                    .HasConstraintName("FK_SSPD_SKPD");

                entity.HasOne(d => d.IdSkpdkbNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSkpdkb)
                    .HasConstraintName("FK_SSPD_SKPDKB");

                entity.HasOne(d => d.IdSkpdkbtNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSkpdkbt)
                    .HasConstraintName("FK_SSPD_SKPDKBT");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_SSPD_SPTPD");

                entity.HasOne(d => d.IdStpdNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdStpd)
                    .HasConstraintName("FK_SSPD_STPD");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_SSPD_Data_Subjek");

                entity.HasOne(d => d.IdUsahaNavigation)
                    .WithMany(p => p.Sspd)
                    .HasForeignKey(d => d.IdUsaha)
                    .HasConstraintName("FK_SSPD_Data_Usaha");
            });

            modelBuilder.Entity<Ssrd>(entity =>
            {
                entity.HasIndex(e => e.IdBank);

                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSetoran);

                entity.HasIndex(e => e.IdSkrd);

                entity.HasIndex(e => e.IdSubjek);

                entity.Property(e => e.IdSsrd).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdBankNavigation)
                    .WithMany(p => p.Ssrd)
                    .HasForeignKey(d => d.IdBank)
                    .HasConstraintName("FK_SSRD_Bank");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Ssrd)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SSRD_Coa");

                entity.HasOne(d => d.IdSetoranNavigation)
                    .WithMany(p => p.Ssrd)
                    .HasForeignKey(d => d.IdSetoran)
                    .HasConstraintName("FK_SSRD_Ref_JenisSetoran");

                entity.HasOne(d => d.IdSkrdNavigation)
                    .WithMany(p => p.Ssrd)
                    .HasForeignKey(d => d.IdSkrd)
                    .HasConstraintName("FK_SSRD_SKRD");

                entity.HasOne(d => d.IdSubjekNavigation)
                    .WithMany(p => p.Ssrd)
                    .HasForeignKey(d => d.IdSubjek)
                    .HasConstraintName("FK_SSRD_Data_Subjek");
            });

            modelBuilder.Entity<Ssrdr>(entity =>
            {
                entity.HasIndex(e => e.IdBank);

                entity.HasIndex(e => e.IdCoa);

                entity.HasIndex(e => e.IdSetoran);

                entity.Property(e => e.IdSsrdr).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Total).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdBankNavigation)
                    .WithMany(p => p.Ssrdr)
                    .HasForeignKey(d => d.IdBank)
                    .HasConstraintName("FK_SSRDR_Bank");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.Ssrdr)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_SSRDR_Coa");

                entity.HasOne(d => d.IdSetoranNavigation)
                    .WithMany(p => p.Ssrdr)
                    .HasForeignKey(d => d.IdSetoran)
                    .HasConstraintName("FK_SSRDR_Ref_JenisSetoran");
            });

            modelBuilder.Entity<SsrdrDt>(entity =>
            {
                entity.HasIndex(e => e.IdSsrdr);

                entity.HasIndex(e => e.IdTarif);

                entity.Property(e => e.IdSsrdrDt).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSsrdrNavigation)
                    .WithMany(p => p.SsrdrDt)
                    .HasForeignKey(d => d.IdSsrdr)
                    .HasConstraintName("FK_SSRDR_Dt_SSRDR");

                entity.HasOne(d => d.IdTarifNavigation)
                    .WithMany(p => p.SsrdrDt)
                    .HasForeignKey(d => d.IdTarif)
                    .HasConstraintName("FK_SSRDR_Dt_Tarif_Retribusi");
            });

            modelBuilder.Entity<Stpd>(entity =>
            {
                entity.HasIndex(e => e.IdSkpd);

                entity.Property(e => e.IdStpd).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.KreditPajak).HasDefaultValueSql("0");

                entity.HasOne(d => d.IdSkpdNavigation)
                    .WithMany(p => p.Stpd)
                    .HasForeignKey(d => d.IdSkpd)
                    .HasConstraintName("FK_STPD_SKPD");
            });

            modelBuilder.Entity<Sts>(entity =>
            {
                entity.HasIndex(e => e.IdBank);

                entity.Property(e => e.IdSts).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Tahun).IsFixedLength();

                entity.HasOne(d => d.IdBankNavigation)
                    .WithMany(p => p.Sts)
                    .HasForeignKey(d => d.IdBank)
                    .HasConstraintName("FK_STS_STS");
            });

            modelBuilder.Entity<StsDt>(entity =>
            {
                entity.HasIndex(e => e.IdSspd);

                entity.HasIndex(e => e.IdSsrd);

                entity.HasIndex(e => e.IdSsrdr);

                entity.HasIndex(e => e.IdSts);

                entity.HasIndex(e => e.IdStts);

                entity.Property(e => e.IdDetailSts).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSspdNavigation)
                    .WithMany(p => p.StsDt)
                    .HasForeignKey(d => d.IdSspd)
                    .HasConstraintName("FK_STS_Dt_SSPD");

                entity.HasOne(d => d.IdSsrdNavigation)
                    .WithMany(p => p.StsDt)
                    .HasForeignKey(d => d.IdSsrd)
                    .HasConstraintName("FK_STS_Dt_SSRD");

                entity.HasOne(d => d.IdSsrdrNavigation)
                    .WithMany(p => p.StsDt)
                    .HasForeignKey(d => d.IdSsrdr)
                    .HasConstraintName("FK_STS_Dt_SSRDR");

                entity.HasOne(d => d.IdStsNavigation)
                    .WithMany(p => p.StsDt)
                    .HasForeignKey(d => d.IdSts)
                    .HasConstraintName("FK_STS_Dt_STS");

                entity.HasOne(d => d.IdSttsNavigation)
                    .WithMany(p => p.StsDt)
                    .HasForeignKey(d => d.IdStts)
                    .HasConstraintName("FK_STS_Dt_STTS");
            });

            modelBuilder.Entity<Stts>(entity =>
            {
                entity.HasIndex(e => e.IdBank);

                entity.HasIndex(e => e.IdSetoran);

                entity.HasIndex(e => e.IdSppt);

                entity.Property(e => e.IdStts).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdBankNavigation)
                    .WithMany(p => p.Stts)
                    .HasForeignKey(d => d.IdBank)
                    .HasConstraintName("FK_STTS_Bank");

                entity.HasOne(d => d.IdSetoranNavigation)
                    .WithMany(p => p.Stts)
                    .HasForeignKey(d => d.IdSetoran)
                    .HasConstraintName("FK_STTS_Ref_JenisSetoran");

                entity.HasOne(d => d.IdSpptNavigation)
                    .WithMany(p => p.Stts)
                    .HasForeignKey(d => d.IdSppt)
                    .HasConstraintName("FK_STTS_SPPT");
            });

            modelBuilder.Entity<SuratTeguran>(entity =>
            {
                entity.HasIndex(e => e.IdSptpd);

                entity.Property(e => e.IdTeguran).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdSptpdNavigation)
                    .WithMany(p => p.SuratTeguran)
                    .HasForeignKey(d => d.IdSptpd)
                    .HasConstraintName("FK_SuratTeguran_SPTPD");
            });

            modelBuilder.Entity<THistory>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Tstamp).HasDefaultValueSql("now()");

                entity.Property(e => e.Who).HasDefaultValueSql("CURRENT_USER");
            });

            modelBuilder.Entity<TandaTangan>(entity =>
            {
                entity.HasIndex(e => e.IdDokumen);

                entity.HasIndex(e => e.IdPegawai);

                entity.Property(e => e.IdTtd).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdDokumenNavigation)
                    .WithMany(p => p.TandaTangan)
                    .HasForeignKey(d => d.IdDokumen)
                    .HasConstraintName("FK_TandaTangan_Ref_Dokumen");

                entity.HasOne(d => d.IdPegawaiNavigation)
                    .WithMany(p => p.TandaTangan)
                    .HasForeignKey(d => d.IdPegawai)
                    .HasConstraintName("FK_TandaTangan_Pegawai");
            });

            modelBuilder.Entity<TarifPajak>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.Property(e => e.IdTarif).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.TarifPajak)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_Tarif_Pajak_Coa");
            });

            modelBuilder.Entity<TarifRetribusi>(entity =>
            {
                entity.HasIndex(e => e.IdCoa);

                entity.Property(e => e.IdTarif).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Satuan1).HasDefaultValueSql("'-'::character varying");

                entity.Property(e => e.Satuan2).HasDefaultValueSql("'-'::character varying");

                entity.HasOne(d => d.IdCoaNavigation)
                    .WithMany(p => p.TarifRetribusi)
                    .HasForeignKey(d => d.IdCoa)
                    .HasConstraintName("FK_Tarif_Retribusi_Coa");
            });

            modelBuilder.Entity<TransaksiLra>(entity =>
            {
                entity.HasIndex(e => e.IdLra);

                entity.Property(e => e.IdTransaksi).HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.IdLraNavigation)
                    .WithMany(p => p.TransaksiLra)
                    .HasForeignKey(d => d.IdLra)
                    .HasConstraintName("FK_TransaksiLRA_LRA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
