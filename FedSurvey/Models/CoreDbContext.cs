using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FedSurvey.Models
{
    public partial class CoreDbContext : DbContext
    {
        // Change this to false once we are actually ready to upload data in.
        private const bool INCLUDE_DUMMY_DATA = true;

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataGroup> DataGroups { get; set; }
        public virtual DbSet<Execution> Executions { get; set; }
        public virtual DbSet<PossibleResponseString> PossibleResponseStrings { get; set; }
        public virtual DbSet<PossibleResponse> PossibleResponses { get; set; }
        public virtual DbSet<QuestionExecution> QuestionExecutions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Response> Responses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Database=SurveyDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Execution>(entity =>
            {
                entity.HasIndex(e => e.Key)
                    .HasName("UQ__Executio__C41E02890F7150F6")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Key).IsUnicode(false);
            });

            modelBuilder.Entity<PossibleResponseString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.PossibleResponse)
                    .WithMany(p => p.PossibleResponseStrings)
                    .HasForeignKey(d => d.PossibleResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses");
            });

            modelBuilder.Entity<PossibleResponse>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.PossibleResponses)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PossibleResponses_QuestionTypes");
            });

            modelBuilder.Entity<QuestionExecution>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Execution)
                    .WithMany(p => p.QuestionExecutions)
                    .HasForeignKey(d => d.ExecutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionExecutions_Executions");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionExecutions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionExecutions_Questions");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_QuestionTypes");
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.DataGroup)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.DataGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_DataGroups");

                entity.HasOne(d => d.PossibleResponse)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.PossibleResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_PossibleResponses");

                entity.HasOne(d => d.QuestionExecution)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.QuestionExecutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Responses_QuestionExecutions");
            });

            OnModelCreatingPartial(modelBuilder);

            if (INCLUDE_DUMMY_DATA)
                DummySeed(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void DummySeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionType>().HasData(
                new QuestionType { Id = 1, Name = "Core Survey" }
            );

            modelBuilder.Entity<PossibleResponse>().HasData(
                new PossibleResponse { Id = 1, QuestionTypeId = 1 }
            );

            modelBuilder.Entity<PossibleResponseString>().HasData(
                new PossibleResponseString { Id = 1, PossibleResponseId = 1, Name = "Positive" }
            );

            modelBuilder.Entity<PossibleResponse>().HasData(
                new PossibleResponse { Id = 2, QuestionTypeId = 1 }
            );

            modelBuilder.Entity<PossibleResponseString>().HasData(
                new PossibleResponseString { Id = 2, PossibleResponseId = 2, Name = "Neutral" }
            );

            modelBuilder.Entity<PossibleResponse>().HasData(
                new PossibleResponse { Id = 3, QuestionTypeId = 1 }
            );

            modelBuilder.Entity<PossibleResponseString>().HasData(
                new PossibleResponseString { Id = 3, PossibleResponseId = 3, Name = "Negative" }
            );

            modelBuilder.Entity<DataGroup>().HasData(
                new DataGroup { Id = 1, Name = "Dummy" }
            );

            modelBuilder.Entity<Execution>().HasData(
                new Execution { Id = 1, Key = "2016" },
                new Execution { Id = 2, Key = "2017" },
                new Execution { Id = 3, Key = "2018" },
                new Execution { Id = 4, Key = "2019" },
                new Execution { Id = 5, Key = "2020" }
            );

            int QuestionId = 1;
            int QuestionExecutionId = 1;
            int ResponseId = 1;
        }
    }
}
