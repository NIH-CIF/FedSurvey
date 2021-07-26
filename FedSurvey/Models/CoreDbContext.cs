using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Models
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataGroup> DataGroups { get; set; }
        public virtual DbSet<DataGroupString> DataGroupStrings { get; set; }
        public virtual DbSet<Execution> Executions { get; set; }
        public virtual DbSet<PossibleResponseString> PossibleResponseStrings { get; set; }
        public virtual DbSet<PossibleResponse> PossibleResponses { get; set; }
        public virtual DbSet<QuestionExecution> QuestionExecutions { get; set; }
        public virtual DbSet<QuestionTypeString> QuestionTypeStrings { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<ResponseDTO> ResponseDTOs { get; set; }
        public virtual DbSet<ResultDTO> ResultDTOs { get; set; }

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
            });

            modelBuilder.Entity<DataGroupString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DataGroup)
                    .WithMany(p => p.DataGroupStrings)
                    .HasForeignKey(d => d.DataGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DataGroupStrings_DataGroups_DataGroupId");
            });
            modelBuilder.Entity<DataGroupString>().ToTable("DataGroupStrings");

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
            });

            modelBuilder.Entity<QuestionTypeString>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.QuestionTypeStrings)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId");
            });
            modelBuilder.Entity<QuestionTypeString>().ToTable("QuestionTypeStrings");

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

            // Later this should probably go away, as ResultDTO does the same thing but better.
            modelBuilder.Entity<ResponseDTO>().ToSqlQuery(
                @"SELECT
                  Responses.Id,
                  Responses.Count,
                  Responses.QuestionExecutionId,
                  Responses.PossibleResponseId,
                  Responses.DataGroupId,
                  CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 OR PossibleResponses.PartOfPercentage = 0 THEN 0 ELSE Responses.Count / SUM(QuestionExecutionResponses.Count) * 100 END AS Percentage
                FROM Responses
                JOIN PossibleResponses
                ON PossibleResponses.Id = Responses.PossibleResponseId
                LEFT JOIN (
                    SELECT Responses.*
                    FROM Responses
                    JOIN PossibleResponses
                        ON PossibleResponses.Id = Responses.PossibleResponseId
                    WHERE PossibleResponses.PartOfPercentage = 1
                ) QuestionExecutionResponses
                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId
                    AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId
                GROUP BY
                Responses.Id,
                Responses.Count,
                Responses.QuestionExecutionId,
                Responses.PossibleResponseId,
                Responses.DataGroupId,
                PossibleResponses.PartOfPercentage"
            );
            modelBuilder.Entity<ResponseDTO>().ToTable(null);

            // Percentage shold be NULL when not part of percentage, but this gave an error that was not worth dealing with.
            modelBuilder.Entity<ResultDTO>().ToSqlQuery(
                @"SELECT
                    Executions.""Key"" AS ExecutionName,
                    Executions.OccurredTime AS ExecutionTime,
                    PossibleResponseStrings.""Name"" AS PossibleResponseName,
                    Responses.Count,
                    CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 OR PossibleResponses.PartOfPercentage = 0 THEN NULL ELSE Responses.Count / SUM(QuestionExecutionResponses.Count) * 100 END AS Percentage,
                    QuestionExecutions.Body AS QuestionText,
                    DataGroupStrings.""Name"" AS DataGroupName,
                    QuestionExecutions.QuestionId AS QuestionId,
                    QuestionExecutions.Position AS QuestionNumber
                FROM Responses
                JOIN PossibleResponses
                ON PossibleResponses.Id = Responses.PossibleResponseId
                JOIN PossibleResponseStrings
                ON PossibleResponseStrings.PossibleResponseId = PossibleResponses.Id
                    AND PossibleResponseStrings.Preferred = 1
                JOIN QuestionExecutions
                ON QuestionExecutions.Id = Responses.QuestionExecutionId
                JOIN Executions
                ON Executions.Id = QuestionExecutions.ExecutionId
                JOIN DataGroups
                ON DataGroups.Id = Responses.DataGroupId
                JOIN DataGroupStrings
                ON DataGroupStrings.DataGroupId = DataGroups.Id
                    AND DataGroupStrings.Preferred = 1
                LEFT JOIN(
                    SELECT Responses.*
                    FROM Responses
                    JOIN PossibleResponses
                        ON PossibleResponses.Id = Responses.PossibleResponseId
                    WHERE PossibleResponses.PartOfPercentage = 1
                ) QuestionExecutionResponses
                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId
                    AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId
                GROUP BY
                Executions.""Key"",
                Executions.OccurredTime,
                PossibleResponseStrings.""Name"",
                Responses.Count,
                PossibleResponses.PartOfPercentage,
                QuestionExecutions.Body,
                DataGroupStrings.""Name"",
                QuestionExecutions.QuestionId,
                QuestionExecutions.Position"
            );
            modelBuilder.Entity<ResultDTO>().HasNoKey();
            modelBuilder.Entity<ResultDTO>().ToTable(null);

            OnModelCreatingPartial(modelBuilder);
        }

        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Added)
                .ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedTime").CurrentValue = DateTime.UtcNow;
                E.Property("UpdatedTime").CurrentValue = DateTime.UtcNow;
            });

            var EditedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Modified)
                .ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("UpdatedTime").CurrentValue = DateTime.UtcNow;
            });

            return base.SaveChanges();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
