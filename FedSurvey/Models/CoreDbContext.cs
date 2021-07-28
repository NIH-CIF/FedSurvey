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

            modelBuilder.Entity<DataGroupLink>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.ParentLinks)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_DataGroupLinks_DataGroups_ParentId");

                entity.HasOne(d => d.Child)
                    .WithMany(p => p.ChildLinks)
                    .HasForeignKey(d => d.ChildId)
                    .HasConstraintName("FK_DataGroupLinks_DataGroups_ChildId");
            });
            modelBuilder.Entity<DataGroupLink>().ToTable("DataGroupLinks");

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

            modelBuilder.Entity<ResultDTO>().ToSqlQuery(
                @"WITH BottomLevel AS (
                    SELECT
                        Executions.""Key"" AS ExecutionName,
                        Executions.OccurredTime AS ExecutionTime,
                        PossibleResponseStrings.Name AS PossibleResponseName,
                        PossibleResponses.PartOfPercentage AS PartOfPercentage,
                        Responses.Count,
                        CASE WHEN PossibleResponses.PartOfPercentage = 0 THEN NULL ELSE Responses.Count / SUM(QuestionExecutionResponses.Count) * 100 END AS Percentage,
                        QuestionExecutions.Body AS QuestionText,
                        DataGroups.Id AS DataGroupId,
                        DataGroupStrings.Name AS DataGroupName,
                        QuestionExecutions.QuestionId AS QuestionId,
                        QuestionExecutions.Position AS QuestionNumber
                    FROM (
                        SELECT
                            SUM(Responses.Count) AS Count,
                            Responses.QuestionExecutionId,
                            Responses.PossibleResponseId,
                            Responses.DataGroupId
                        FROM Responses
                        GROUP BY
                        Responses.QuestionExecutionId,
                        Responses.PossibleResponseId,
                        Responses.DataGroupId
                    ) Responses
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
                    Executions.""Key"",
                    Executions.OccurredTime,
                    PossibleResponseStrings.Name,
                    Responses.Count,
                    PossibleResponses.PartOfPercentage,
                    QuestionExecutions.Body,
                    DataGroups.Id,
                    DataGroupStrings.Name,
                    QuestionExecutions.QuestionId,
                    QuestionExecutions.Position
                ),
                MiddleLevel AS (
                    SELECT
                        BottomLevel.ExecutionName,
                        BottomLevel.ExecutionTime,
                        BottomLevel.PossibleResponseName,
                        BottomLevel.PartOfPercentage,
                        SUM(BottomLevel.Count) AS Count,
                        BottomLevel.QuestionText,
                        DataGroups.Id AS DataGroupId,
                        BottomLevel.QuestionId,
                        BottomLevel.QuestionNumber
                    FROM DataGroups
                    JOIN DataGroupLinks
                    ON DataGroupLinks.ParentId = DataGroups.Id
                    JOIN BottomLevel
                    ON BottomLevel.DataGroupId = DataGroupLinks.ChildId
                    GROUP BY
                    BottomLevel.ExecutionName,
                    BottomLevel.ExecutionTime,
                    BottomLevel.PossibleResponseName,
                    BottomLevel.PartOfPercentage,
                    BottomLevel.QuestionText,
                    DataGroups.Id,
                    BottomLevel.QuestionId,
                    BottomLevel.QuestionNumber
                ),
                ComputedTotals AS (
                    SELECT
                        MiddleLevel.QuestionId,
                        MiddleLevel.ExecutionName,
                        SUM(MiddleLevel.Count) AS Count
                    FROM MiddleLevel
                    WHERE MiddleLevel.PartOfPercentage = 1
                    GROUP BY
                    MiddleLevel.QuestionId,
                    MiddleLevel.ExecutionName
                )

                SELECT
                    ExecutionName,
                    ExecutionTime,
                    PossibleResponseName,
                    Count,
                    Percentage,
                    QuestionText,
                    DataGroupName,
                    QuestionId,
                    QuestionNumber
                FROM BottomLevel

                UNION

                SELECT
                    MiddleLevel.ExecutionName,
                    MiddleLevel.ExecutionTime,
                    MiddleLevel.PossibleResponseName,
                    MiddleLevel.Count,
                    CASE WHEN MiddleLevel.PartOfPercentage = 0 THEN NULL ELSE MiddleLevel.Count / ComputedTotals.Count * 100 END AS Percentage,
                    MiddleLevel.QuestionText,
                    DataGroupStrings.Name AS DataGroupName,
                    MiddleLevel.QuestionId,
                    MiddleLevel.QuestionNumber
                FROM MiddleLevel
                JOIN ComputedTotals
                ON ComputedTotals.QuestionId = MiddleLevel.QuestionId
                AND ComputedTotals.ExecutionName = MiddleLevel.ExecutionName
                JOIN DataGroupStrings
                ON DataGroupStrings.DataGroupId = MiddleLevel.DataGroupId
                AND DataGroupStrings.Preferred = 1"
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
